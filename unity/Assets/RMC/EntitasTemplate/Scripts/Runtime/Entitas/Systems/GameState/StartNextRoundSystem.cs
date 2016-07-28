﻿using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using System;
using System.Collections.Generic;
using RMC.EntitasTemplate.Entitas;

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
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_pool = pool;
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

                //Friction added in the y only
                entityBall.AddVelocity (new Vector3 (30f, 30f, 0), GameConstants.BallFriction);
				entityBall.AddResource ("Prefabs/Ball");
				entityBall.AddGoal(1);
				entityBall.AddBoundsBounce(-1);

				e.willDestroy = true;
			}
		}

    }
}