using UnityEngine;
using System.Collections;
using Entitas;
using Entitas.Unity.VisualDebugging;
using RMC.EntitasTemplate.Entias.Systems;


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
			SetupEntities ();
			SetupSystems ();
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
			var e = _pool.CreateEntity ();
			e.AddPosition (1, 2, 3);
			e.AddMove (10, 10, 10);
			Debug.Log ("Entity Position.y : " + e.position.y);
		}

		private void SetupSystems ()
		{
			_systems = new Systems ().Add (_pool.CreateSystem<MoveSystem> ());
		}

	}
}
