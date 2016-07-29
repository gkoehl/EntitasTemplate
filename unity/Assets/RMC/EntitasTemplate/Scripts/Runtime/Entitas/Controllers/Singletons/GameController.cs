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
using RMC.Common.Entitas.Controllers.Singleton;
using System.Collections;

namespace RMC.EntitasTemplate.Entitas.Controllers.Singleton
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
    public class GameController : SingletonMonobehavior<GameController> 
	{
		// ------------------ Constants and statics
        private const float PaddleOffsetToEdgeX = 3;

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Feature _pausableUpdateSystems;
		private Feature _unpausableUpdateSystems;
        private Feature _pausableFixedUpdateSystems;
		private Pool _pool;
		private PoolObserver _poolObserver;
		private Entity _gameEntity;


		// ------------------ Methods

		override protected void Awake () 
		{
			base.Awake();
            GameController.OnDestroying += GameController_OnDestroying;
            AudioController.Instantiate();
            InputController.Instantiate();
			Debug.Log ("GC.Awake()");

			Application.targetFrameRate = 30;

			SetupPools ();
			SetupPoolObserver();

			//order matters
			//1 - Systems that depend on entities will internally listen for entity creation before reacting - nice!
			SetupSystems ();

			//2
			SetupEntities ();

			//place a ball in the middle of the screen w/ velocity
			_pool.CreateEntity().willStartNextRound = true;

		}

		protected void Update () 
		{
			if (!_gameEntity.time.isPaused)
			{
                _pausableUpdateSystems.Execute ();
			}
            _unpausableUpdateSystems.Execute();
		}

        protected void LateUpdate () 
        {
            if (!_gameEntity.time.isPaused)
            {
                _pausableFixedUpdateSystems.Execute ();
            }
        }

        //Called during GameController.Destroy();
        private void GameController_OnDestroying (GameController instance) 
        {
            Debug.Log ("OnGameControllerDestroying()");
            GameController.OnDestroying -= GameController_OnDestroying;

            if (AudioController.IsInstantiated())
            {
                AudioController.Destroy();
            }
            if (InputController.IsInstantiated())
            {
                InputController.Destroy();
            }
            _pausableUpdateSystems.DeactivateReactiveSystems();
            _unpausableUpdateSystems.DeactivateReactiveSystems ();

            Pools.pool.Reset ();
            DestroyPoolObserver();

            Group _scoreGroup = Pools.pool.GetGroup(Matcher.AllOf (Matcher.Game, Matcher.Score));
            Debug.LogWarning ("DESTROY should have zero and : " + _scoreGroup.count);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private static Bounds GetOrthographicBounds(Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        private static Bounds GetBounds(Camera camera)
        {
            //TODO: Why is this needed? Without it the lower bound is offscreen
            float offsetY = 2;

            var size = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            return new Bounds(Vector3.zero, new Vector3(size.x * 2, size.y * 2 - offsetY));
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

            var bounds = GetOrthographicBounds(Camera.main);
            //Debug.Log(bounds.min.y + " and " + bounds.max.y);


			_gameEntity = _pool.CreateEntity();
			_gameEntity.AddGame(0);
			_gameEntity.AddBounds(bounds);
			_gameEntity.AddScore(0,0);
			_gameEntity.AddTime (0, 0, false);

            //on Right
			Entity entityWhite = _pool.CreateEntity ();
            entityWhite.AddPaddle(PaddleComponent.PaddleType.White);
            entityWhite.AddResource ("Prefabs/PaddleWhite");
            entityWhite.AddVelocity (Vector3.zero);
            entityWhite.WillAcceptInput (true);

            //on left
			Entity entityBlack = _pool.CreateEntity ();
            entityBlack.AddPaddle(PaddleComponent.PaddleType.Black);
            entityBlack.AddResource ("Prefabs/PaddleBlack");
            entityBlack.AddVelocity (Vector3.zero);
            entityBlack.AddAI(entityWhite, 1, 25f);



            //Tick the systems once so the 'View' is added by the AddResourceSystem()
            _pausableUpdateSystems.Execute();
            entityWhite.AddPosition (new Vector3 (bounds.max.x - entityWhite.view.bounds.size.x/2 - PaddleOffsetToEdgeX, 0, 0));
            entityBlack.AddPosition (new Vector3 (bounds.min.x + entityBlack.view.bounds.size.x/2 + PaddleOffsetToEdgeX, 0, 0));
			

			Group _scoreGroup = Pools.pool.GetGroup(Matcher.AllOf (Matcher.Game, Matcher.Score));
			Debug.LogWarning ("START should have zero and : " + _scoreGroup.count);


		}

		private void SetupSystems ()
		{

			//a feature is a group of systems
			_pausableUpdateSystems = new Feature ();
			
			_pausableUpdateSystems.Add (_pool.CreateSystem<StartNextRoundSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<VelocitySystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<ViewSystem> ());

			_pausableUpdateSystems.Add (_pool.CreateSystem<AddResourceSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<RemoveResourceSystem> ());

            _pausableUpdateSystems.Add (_pool.CreateSystem<AcceptInputSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<AISystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<GoalSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<DestroySystem> ());

			//	Not physics based - as an example
			_pausableUpdateSystems.Add (_pool.CreateSystem<BoundsBounceSystem> ());
            _pausableUpdateSystems.Add (_pool.CreateSystem<BoundsConstrainSystem> ());
			_pausableUpdateSystems.Initialize();
			_pausableUpdateSystems.ActivateReactiveSystems();


            _pausableFixedUpdateSystems = new Feature ();
            //  Physics based - as an example.
            _pausableFixedUpdateSystems.Add (_pool.CreateSystem<CollisionSystem> ());
            _pausableFixedUpdateSystems.Initialize();
            _pausableFixedUpdateSystems.ActivateReactiveSystems();



			//for demo only, an example of an unpausable system
			_unpausableUpdateSystems = new Feature ();
			_unpausableUpdateSystems.Add (_pool.CreateSystem<TimeSystem> ());
			_unpausableUpdateSystems.Initialize();
			_unpausableUpdateSystems.ActivateReactiveSystems();



		}


		public void TogglePause ()
		{
            _pool.CreateEntity().AddAudio(GameConstants.Audio_ButtonClickSuccess, 0.5f);
            SetPause(!_gameEntity.time.isPaused);

			//Keep
			//Debug.Log ("TogglePause() isPaused: " + _gameEntity.time.isPaused);	

		}

        private void SetPause (bool isPaused)
        {
            _gameEntity.ReplaceTime
            (
                _gameEntity.time.timeSinceGameStartUnpaused, 
                _gameEntity.time.timeSinceGameStartTotal, 
                isPaused
            );
        }



		//ADVICE ON RESTARTING: https://github.com/sschmid/Entitas-CSharp/issues/82
		//TODO: Restart is not complete. It doesn't truely represent a fresh start yet - srivello
		public void Restart ()
		{
            
            SetPause(true);
            StartCoroutine(Restart_Coroutine());
			
		}

        //Add small pause so we hear the click sound
        private IEnumerator Restart_Coroutine ()
        {
            _pool.CreateEntity().AddAudio(GameConstants.Audio_ButtonClickSuccess, 0.5f);
            yield return new WaitForSeconds(0.25f);
            GameController.Destroy();
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
