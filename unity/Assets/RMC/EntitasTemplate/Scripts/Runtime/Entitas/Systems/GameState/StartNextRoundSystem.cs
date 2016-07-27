using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using System;
using System.Collections.Generic;

namespace RMC.Common.Entitas.Systems.GameState
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class StartNextRoundSystem : IReactiveSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _startNextRoundGroup;
		private Group _aiGroup;
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_pool = pool;
			_startNextRoundGroup = _pool.GetGroup(Matcher.AllOf(Matcher.StartNextRound));
			_aiGroup = _pool.GetGroup(Matcher.AllOf(Matcher.AI));
		}

		public TriggerOnEvent trigger
        {
            get { return Matcher.StartNextRound.OnEntityAdded(); }
        }

        public void Execute(List<Entity> entities)
        {
			foreach (var e in entities)
			{
				Entity entityBall = _pool.CreateEntity ();
				entityBall.AddPosition (new Vector3 (0,0,0));
				entityBall.AddVelocity (new Vector3 (1f, 1f, 0));
				entityBall.AddResource ("Prefabs/Ball");
				entityBall.AddGoal(1);
				entityBall.AddBoundsBounce(-1);

				e.willDestroy = true;
			}
		}

    }
}