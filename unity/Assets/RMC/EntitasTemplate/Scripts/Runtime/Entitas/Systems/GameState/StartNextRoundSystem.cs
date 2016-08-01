using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using System;
using System.Collections.Generic;
using RMC.EntitasTemplate.Entitas;
using RMC.EntitasTemplate.Entitas.Controllers.Singleton;
using System.Collections;

namespace RMC.EntitasTemplate.Entitas.Systems.GameState
{
	/// <summary>
	/// Called at game start and after every goal
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

                entityBall.AddFriction (GameConstants.BallFriction);
				entityBall.AddResource ("Prefabs/Ball");
				entityBall.AddGoal(1);
				entityBall.AddBoundsBounce(-1);
                entityBall.AddTick       (Time.deltaTime);

				e.willDestroy = true;

                GameController.Instance.StartCoroutine(StartNextRound_Coroutine(entityBall, 0.5f));
			}
		}

        /// <summary>
        /// Add a delay AFTER creating the ball visuall and BEFORE moving it. A courtesy to player.
        /// </summary>
        private IEnumerator StartNextRound_Coroutine (Entity entityBall, float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
             entityBall.AddVelocity (GameConstants.GetBallInitialVelocity());
        }

    }
}