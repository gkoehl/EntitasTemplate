using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;
using RMC.Common.Entitas.Systems.Render;
using RMC.Common.Entitas.Systems.Transform;
using RMC.EntitasTemplate.Entitas.Systems.Collision;
using RMC.Common.Entitas.Systems;

// This is required because the entitas class path is similar to my namespaces. This prevents collision - srivello
using EntitasSystems = Entitas.Systems;
//
using RMC.Common.Entitas.Systems.Destroy;
using RMC.Common.Singleton;
using UnityEngine.SceneManagement;
using RMC.Common.Entitas.Systems.GameState;

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
		private EntitasSystems _systems;
		private Pool _pool;
		private PoolObserver _poolObserver;

		private bool _isPaused = false;


		// ------------------ Methods

		override protected void Awake () 
		{
			base.Awake();
			Debug.Log ("GC.Awake()");

			Application.targetFrameRate = 30;

			SetupPools ();
			SetupPoolObserver();
			SetupSystems ();
			SetupEntities ();


			_pool.CreateEntity().willStartNextRound = true;

		}

		protected void Update () 
		{
			if (!_isPaused)
			{
				_systems.Execute ();
			}
		}
		protected void OnDestroy () 
		{
			DestroyPoolObserver();
		}

        private Bounds GetBounds()
        {
            var size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            return new Bounds(Vector3.zero, new Vector3(size.x * 2, size.y * 2));
        }

		private void SetupPools ()
		{
			Debug.Log ("SetupPools()");
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
			Entity gameEntity = _pool.CreateEntity();
			gameEntity.AddGame(0);
			gameEntity.AddBounds(bounds);
			gameEntity.AddScore(0,0);

			Entity entityWhite = _pool.CreateEntity ();
			entityWhite.AddPosition (new Vector3 (25, 0, 0));
			entityWhite.AddVelocity (Vector3.zero);
			entityWhite.HasInput (true);
			entityWhite.AddResource ("Prefabs/PaddleWhite");

			Entity entityBlack = _pool.CreateEntity ();
			entityBlack.AddPosition (new Vector3 (-25, 0, 0));
			entityBlack.AddVelocity (Vector3.zero);
			entityBlack.AddAI (entityWhite, 1, 0.5f);
			entityBlack.AddResource ("Prefabs/PaddleBlack");


		}

		private void SetupSystems ()
		{

#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
			_systems = new DebugSystems();
#else
			_systems = new EntitasSystems();
#endif
			
			_systems.Add (_pool.CreateSystem<StartNextRoundSystem> ());
			_systems.Add (_pool.CreateSystem<VelocitySystem> ());
			_systems.Add (_pool.CreateSystem<ViewSystem> ());

			_systems.Add (_pool.CreateSystem<AddResourceSystem> ());
			_systems.Add (_pool.CreateSystem<RemoveResourceSystem> ());

			_systems.Add (_pool.CreateSystem<InputSystem> ());
			_systems.Add (_pool.CreateSystem<AISystem> ());
			_systems.Add (_pool.CreateSystem<GoalSystem> ());
			_systems.Add (_pool.CreateSystem<DestroySystem> ());

			//	Not physics based - as an example
			_systems.Add (_pool.CreateSystem<BoundsBounceSystem> ());

			//	Physics based - as an example.
			_systems.Add (_pool.CreateSystem<CollisionSystem> ());

			_systems.Initialize();
			_systems.ActivateReactiveSystems();

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


		//TODO: Restart is not complete. It doesn't truely represent a fresh start yet - srivello
		public void Restart ()
		{
			Debug.Log ("Restart()");
			DestroyPoolObserver();

			_systems.DeactivateReactiveSystems();
			_pool.Reset();

			GameController.Destroy();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}


		//TODO: Pause is not working - srivello
        public void TogglePause ()
		{
			_isPaused = !_isPaused;	
			Debug.Log ("TogglePause() _isPaused: " + _isPaused);	

		}

	}


}
