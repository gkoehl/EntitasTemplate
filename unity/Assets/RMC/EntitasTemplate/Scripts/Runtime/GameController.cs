using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity.VisualDebugging;
using RMC.Common.Entitas.Systems;


namespace RMC.EntitasTemplate.Entitas
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class GameController : MonoBehaviour 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Systems _systems;
		private Pool _pool;


		// ------------------ Methods

		protected void Start () {

			SetupPools ();
			SetupSystems ();
			SetupEntities ();

		}

		protected void Update () 
		{
			_systems.Execute ();
		}


		private void SetupPools ()
		{
			_pool = new Pool (ComponentIds.TotalComponents, 0, new PoolMetaData ("Pool", ComponentIds.componentNames, ComponentIds.componentTypes));

			//	Optional debugging (Its helpful.)
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
			var poolObserver = new PoolObserver (_pool);
			Object.DontDestroyOnLoad (poolObserver.entitiesContainer);
#endif

		}

		private void SetupEntities ()
		{


			var entityWhite = _pool.CreateEntity ();
			entityWhite.AddPosition (25, 0, 0);
			entityWhite.AddVelocity (0, 0, 0);
			entityWhite.AddInput (0);
			entityWhite.AddResource ("Prefabs/PaddleWhite");

			var entityBlack = _pool.CreateEntity ();
			entityBlack.AddPosition (-25, 0, 0);
			entityBlack.AddVelocity (0,0,0);
			entityBlack.AddAI (entityWhite, 1, 0.5f);
			entityBlack.AddResource ("Prefabs/PaddleBlack");
		}

		private void SetupSystems ()
		{

#if (UNITY_EDITOR)
			_systems = new DebugSystems();
#else
			_systems = new Systems();
#endif

			_systems = new Systems ();
			_systems.Add (_pool.CreateSystem<VelocitySystem> ());
			_systems.Add (_pool.CreateSystem<ViewSystem> ());
			_systems.Add (_pool.CreateSystem<AddResourceSystem> ());
			_systems.Add (_pool.CreateSystem<InputSystem> ());
			_systems.Add (_pool.CreateSystem<AISystem> ());
			//_systems.Add (_pool.CreateSystem<RemoveResourceSystem> ());
		}

	}
}
