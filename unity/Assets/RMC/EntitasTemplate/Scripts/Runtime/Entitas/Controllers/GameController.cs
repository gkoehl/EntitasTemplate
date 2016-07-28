using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;
using RMC.Common.Entitas.Systems.Render;
using RMC.Common.Entitas.Systems.Transform;
using RMC.EntitasTemplate.Entitas.Systems.Collision;
using RMC.Common.Entitas.Systems;

// This is required because the entitas class path is similar to my namespaces. This prevents collision - srivello
using Feature = Entitas.Systems;
//
using RMC.Common.Entitas.Systems.Destroy;
using RMC.Common.Singleton;
using UnityEngine.SceneManagement;
using RMC.Common.Entitas.Systems.GameState;
using RMC.EntitasTemplate.Entitas.Components;

namespace RMC.EntitasTemplate.Entitas.Controllers
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class GameController : SingletonMonobehavior<GameController> 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Feature _pausableSystems;
		private Feature _unpausableSystems;
		private Pool _pool;
		private PoolObserver _poolObserver;
		private Entity _gameEntity;


		// ------------------ Methods

		override protected void Awake () 
		{
			base.Awake();
			Debug.Log ("GC.Awake()");

			Application.targetFrameRate = 30;

			SetupPools ();
			SetupPoolObserver();

			//order matters
			//1 - Systems that depend on entities will internally listen for entity creation before reacting - nice!
			SetupSystems ();

			//2
			SetupEntities ();


			GameController.OnDestroying += OnGameControllerDestroying;

			//place a ball in the middle of the screen w/ velocity
			_pool.CreateEntity().willStartNextRound = true;

		}

		protected void Update () 
		{
			if (!_gameEntity.time.isPaused)
			{
				_pausableSystems.Execute ();
			}
			_unpausableSystems.Execute();
		}



        private Bounds GetBounds()
        {
            var size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            return new Bounds(Vector3.zero, new Vector3(size.x * 2, size.y * 2));
        }

		private void SetupPools ()
		{
			_pool = new Pool (ComponentIds.TotalComponents, 0, new PoolMetaData ("Pool", ComponentIds.componentNames, ComponentIds.componentTypes));
			
			//	TODO: Not sure why I must do this, but I must or other classes can't do pool lookups - srivello
			_pool = Pools.pool;
			


		}


		private void SetupPoolObserver()
		{

			//	Optional debugging (Its helpful.)
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
			//TODO: Sometimes there are two of these in the hierarchy. Prevent that - srivello
			PoolObserverBehaviour poolObserverBehaviour = GameObject.FindObjectOfType<PoolObserverBehaviour>();
			if (poolObserverBehaviour == null)
			{
				_poolObserver = new PoolObserver (_pool);
				//Set as a child to unclutter hierarchy
				_poolObserver.entitiesContainer.transform.SetParent (transform);	
			}
			//Helpful if you want to see the pools in the hierarchy after you STOP the scene. Rarely needed - srivello
			//Object.DontDestroyOnLoad (poolObserver.entitiesContainer);
#endif
			
		}


		private void SetupEntities ()
		{

			var bounds = GetBounds();
			_gameEntity = _pool.CreateEntity();
			_gameEntity.AddGame(0);
			_gameEntity.AddBounds(bounds);
			_gameEntity.AddScore(0,0);
			_gameEntity.AddTime (0, 0, false);

			Entity entityWhite = _pool.CreateEntity ();
            entityWhite.AddPaddle(PaddleComponent.PaddleType.White);
            entityWhite.AddResource ("Prefabs/PaddleWhite");
			entityWhite.AddPosition (new Vector3 (25, 0, 0));
			entityWhite.AddVelocity (Vector3.zero);
			entityWhite.HasInput (true);
			

			Entity entityBlack = _pool.CreateEntity ();
            entityBlack.AddPaddle(PaddleComponent.PaddleType.Black);
            entityBlack.AddResource ("Prefabs/PaddleBlack");
			entityBlack.AddPosition (new Vector3 (-25, 0, 0));
			entityBlack.AddVelocity (Vector3.zero);
			entityBlack.AddAI (entityWhite, 1, 0.5f);
			

			Group _scoreGroup = Pools.pool.GetGroup(Matcher.AllOf (Matcher.Game, Matcher.Score));
			Debug.LogWarning ("START should have zero and : " + _scoreGroup.count);


		}

		private void SetupSystems ()
		{

			//a feature is a group of systems
			_pausableSystems = new Feature ();
			
			_pausableSystems.Add (_pool.CreateSystem<StartNextRoundSystem> ());
			_pausableSystems.Add (_pool.CreateSystem<VelocitySystem> ());
			_pausableSystems.Add (_pool.CreateSystem<ViewSystem> ());

			_pausableSystems.Add (_pool.CreateSystem<AddResourceSystem> ());
			_pausableSystems.Add (_pool.CreateSystem<RemoveResourceSystem> ());

			_pausableSystems.Add (_pool.CreateSystem<InputSystem> ());
			_pausableSystems.Add (_pool.CreateSystem<AISystem> ());
			_pausableSystems.Add (_pool.CreateSystem<GoalSystem> ());
			_pausableSystems.Add (_pool.CreateSystem<DestroySystem> ());

			//	Not physics based - as an example
			_pausableSystems.Add (_pool.CreateSystem<BoundsBounceSystem> ());
            _pausableSystems.Add (_pool.CreateSystem<BoundsConstrainSystem> ());

			//	Physics based - as an example.
			_pausableSystems.Add (_pool.CreateSystem<CollisionSystem> ());

			_pausableSystems.Initialize();
			_pausableSystems.ActivateReactiveSystems();


			//for demo only, an example of an unpausable system
			_unpausableSystems = new Feature ();
			_unpausableSystems.Add (_pool.CreateSystem<TimeSystem> ());
			_unpausableSystems.Initialize();
			_unpausableSystems.ActivateReactiveSystems();



		}


		public void TogglePause ()
		{
			_gameEntity.ReplaceTime
			(
				_gameEntity.time.timeSinceGameStartUnpaused, 
				_gameEntity.time.timeSinceGameStartTotal, 
				!_gameEntity.time.isPaused
			);

			//Keep
			//Debug.Log ("TogglePause() isPaused: " + _gameEntity.time.isPaused);	

		}



		//ADVICE ON RESTARTING: https://github.com/sschmid/Entitas-CSharp/issues/82
		//TODO: Restart is not complete. It doesn't truely represent a fresh start yet - srivello
		public void Restart ()
		{
			GameController.Destroy();
		}


		//Called during GameController.Destroy();
		private void OnGameControllerDestroying (GameController instance) 
		{
			Debug.Log ("OnGameControllerDestroying()");

			_pausableSystems.DeactivateReactiveSystems();
			_unpausableSystems.DeactivateReactiveSystems ();

			Pools.pool.Reset ();
			DestroyPoolObserver();

			Group _scoreGroup = Pools.pool.GetGroup(Matcher.AllOf (Matcher.Game, Matcher.Score));
			Debug.LogWarning ("DESTROY should have zero and : " + _scoreGroup.count);

			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private void DestroyPoolObserver()
		{
			if (_poolObserver != null)
			{
				_poolObserver.Deactivate();

				if (_poolObserver.entitiesContainer != null)
				{
					Destroy (_poolObserver.entitiesContainer);
				}

				_poolObserver = null;
			}
		}



	}


}
