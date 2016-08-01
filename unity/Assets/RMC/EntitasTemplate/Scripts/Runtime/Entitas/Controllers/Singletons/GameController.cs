using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;
using RMC.Common.Entitas.Systems.Render;
using RMC.Common.Entitas.Systems.Transform;
using RMC.Common.Entitas.Systems;

// This is required because the entitas class path is similar to my namespaces. This prevents collision - srivello
using Feature = Entitas.Systems;
//
using RMC.Common.Entitas.Systems.Destroy;
using RMC.Common.Singleton;
using UnityEngine.SceneManagement;
using RMC.EntitasTemplate.Entitas.Components;
using RMC.Common.Entitas.Controllers.Singleton;
using System.Collections;
using RMC.Common.Entitas.Utilities;
using EntitasSystems = Entitas.Systems;
using RMC.EntitasTemplate.Entitas.Systems;

namespace RMC.EntitasTemplate.Entitas.Controllers.Singleton
{
	/// <summary>
	/// The main entry point for the game. Creates the Entitas setup
	/// </summary>
    public class GameController : SingletonMonobehavior<GameController> 
	{
		// ------------------ Constants and statics
        private const float PaddleOffsetToEdgeX = 3;

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
        private EntitasSystems _unpausableUpdateSystems;
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


		}

		protected void Update () 
		{
            _unpausableUpdateSystems.Execute();
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

            //  Create game with data. This is non-visual.
			_gameEntity = _pool.CreateEntity();
			_gameEntity.AddScore(0);
            _gameEntity.AddAudioSettings(false);

            //  Create human player on the right
            Entity playerEntity            = _pool.CreateEntity ();
            playerEntity.AddResource       ("Prefabs/Player");
            playerEntity.AddVelocity       (Vector3.zero);
            playerEntity.AddPosition       (Vector3.zero);
            playerEntity.WillAcceptInput   (true);
            playerEntity.AddTick           (Time.deltaTime);


		}

		private void SetupSystems ()
		{

			//a feature is a group of systems
            _unpausableUpdateSystems = new Feature ();
            _unpausableUpdateSystems.Add (_pool.CreateSystem<VelocitySystem> ());
            _unpausableUpdateSystems.Add (_pool.CreateSystem<ViewSystem> ());

            _unpausableUpdateSystems.Add (_pool.CreateSystem<AddResourceSystem> ());
            _unpausableUpdateSystems.Add (_pool.CreateSystem<RemoveResourceSystem> ());

            _unpausableUpdateSystems.Add (_pool.CreateSystem<AcceptInputSystem> ());
            _unpausableUpdateSystems.Add (_pool.CreateSystem<DestroySystem> ());

			_unpausableUpdateSystems.Initialize();
			_unpausableUpdateSystems.ActivateReactiveSystems();

            // I'm not sure this is useful, but I saw something similar in Entitas presentation slides - srivello
            _pool.SetEntitas
            (
                null,
                _unpausableUpdateSystems,
                null
            );
            //Debug.Log("pausableUpdateSystems: " + Pools.pool.entitas.unpausableUpdateSystems);


		}


		//ADVICE ON RESTARTING: https://github.com/sschmid/Entitas-CSharp/issues/82
		public void Restart ()
		{
            
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
