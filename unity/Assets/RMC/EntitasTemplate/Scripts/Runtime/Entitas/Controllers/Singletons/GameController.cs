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
using RMC.Common.Entitas.Helpers;
using EntitasSystems = Entitas.Systems;

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
        private EntitasSystems _pausableUpdateSystems;
        private EntitasSystems _unpausableUpdateSystems;
        private EntitasSystems _pausableFixedUpdateSystems;
		private Pool _pool;
		private PoolObserver _poolObserver;
		private Entity _gameEntity;


		// ------------------ Methods

		override protected void Awake () 
		{
			base.Awake();
            Debug.Log ("GameController.Awake()");


            GameController.OnDestroying += GameController_OnDestroying;
            AudioController.Instantiate();
            InputController.Instantiate();

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
            Debug.Log ("GameController.Destroy()");

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

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

            var bounds = GameHelper.GetOrthographicBounds(Camera.main);
            //Debug.Log(bounds.min.y + " and " + bounds.max.y);


            //  Create game with data. This is non-visual.
			_gameEntity = _pool.CreateEntity();
			_gameEntity.AddGame(0);
			_gameEntity.AddBounds(bounds);
			_gameEntity.AddScore(0,0);
			_gameEntity.AddTime (0, 0, false);
            _gameEntity.AddAudioSettings(false);

            //  Create human player on the right
            Entity whitePaddleEntity            = _pool.CreateEntity ();
            whitePaddleEntity.AddPaddle         (PaddleComponent.PaddleType.White);
            whitePaddleEntity.AddResource       ("Prefabs/PaddleWhite");
            whitePaddleEntity.AddVelocity       (Vector3.zero);
            whitePaddleEntity.WillAcceptInput   (true);
            whitePaddleEntity.AddTick           (Time.deltaTime);

            //  Create computer player on the left
            Entity blackPaddleEntity        = _pool.CreateEntity ();
            blackPaddleEntity.AddPaddle     (PaddleComponent.PaddleType.Black);
            blackPaddleEntity.AddResource   ("Prefabs/PaddleBlack");
            blackPaddleEntity.AddVelocity   (Vector3.zero);
            blackPaddleEntity.AddAI         (whitePaddleEntity, 1, 25f);
            blackPaddleEntity.AddTick       (Time.deltaTime);


            //Tick the systems once so the 'View' is added by the AddResourceSystem()
            _pausableUpdateSystems.Execute();
            whitePaddleEntity.AddPosition (new Vector3 (bounds.max.x - whitePaddleEntity.view.bounds.size.x/2 - PaddleOffsetToEdgeX, 0, 0));
            blackPaddleEntity.AddPosition (new Vector3 (bounds.min.x + blackPaddleEntity.view.bounds.size.x/2 + PaddleOffsetToEdgeX, 0, 0));
			

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

			//	'Collision' as NOT physics based - as an example
			_pausableUpdateSystems.Add (_pool.CreateSystem<BoundsBounceSystem> ());
            _pausableUpdateSystems.Add (_pool.CreateSystem<BoundsConstrainSystem> ());
			_pausableUpdateSystems.Initialize();
			_pausableUpdateSystems.ActivateReactiveSystems();


            _pausableFixedUpdateSystems = new Feature ();
            //  'Collision as Physics based - as an example.
            _pausableFixedUpdateSystems.Add (_pool.CreateSystem<CollisionSystem> ());
            _pausableFixedUpdateSystems.Initialize();
            _pausableFixedUpdateSystems.ActivateReactiveSystems();


			//for demo only, an example of an unpausable system
			_unpausableUpdateSystems = new Feature ();
			_unpausableUpdateSystems.Add (_pool.CreateSystem<TimeSystem> ());
			_unpausableUpdateSystems.Initialize();
			_unpausableUpdateSystems.ActivateReactiveSystems();

            // This is custom and optional. I use it to store the systems in case I need them again. 
            // This is the only place I put a component directly on a _pool. It is supported.
            // I'm not sure this is useful, but I saw something similar in Entitas presentation slides - srivello
            _pool.SetEntitas
            (
                _pausableUpdateSystems,
                _unpausableUpdateSystems,
                _pausableUpdateSystems
            );
            //Debug.Log("pausableUpdateSystems: " + Pools.pool.entitas.pausableUpdateSystems);


		}


		public void TogglePause ()
		{
            _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, 0.5f);
            SetPause(!_gameEntity.time.isPaused);

			//Keep
			//Debug.Log ("TogglePause() isPaused: " + _gameEntity.time.isPaused);	

		}

        public void ToggleMute ()
        {

            bool isMuted = _gameEntity.audioSettings.isMuted;

            if (isMuted)
            {
                //unmute first
                SetMute(!isMuted);
                _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, 0.5f);

            }
            else
            {
                //play sound first
                _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, 0.5f);
                SetMute(!isMuted);
            }

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

        private void SetMute (bool isMute)
        {
            _gameEntity.ReplaceAudioSettings
            (
                isMute
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
            _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, 0.5f);
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
