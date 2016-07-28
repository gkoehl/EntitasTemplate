using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using System;
using RMC.EntitasTemplate.Entitas.Controllers;
using System.Collections;
using RMC.EntitasTemplate.Entitas.Controllers.Singleton;

namespace RMC.Common.Entitas.Systems.GameState
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class GoalSystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _goalGroup;
        private Entity _gameEntity;
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_pool = pool;
            _goalGroup = _pool.GetGroup(Matcher.AllOf(Matcher.Goal, Matcher.Position, Matcher.Velocity));

            //By design: Systems created before Entities, so wait :)
            _pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds, Matcher.Score)).OnEntityAdded += OnGameEntityAdded;

        }

        private void OnGameEntityAdded (Group group, Entity entity, int index, IComponent component)
        {
            //TODO: I expect this to be called on game start and game restart, but not every StartNextRound, why - srivello
            //Debug.Log("added _gameEntity: " + _gameEntity);
            _gameEntity = group.GetSingleEntity();
        }

		public void Execute() 
		{
			foreach (var e in _goalGroup.GetEntities()) 
			{
				Bounds bounds = _gameEntity.bounds.bounds;

				if (e.position.position.x < bounds.min.x) 
				{
					//white
					ChangeScore(e.goal.pointsPerGoal, 0);
					e.willDestroy = true;
                    GameController.Instance.StartCoroutine(StartNextRound_Coroutine(1));
				} 
				else if (e.position.position.x > bounds.max.x)
				{
					//black
					ChangeScore(0, e.goal.pointsPerGoal);
					e.willDestroy = true;
                    GameController.Instance.StartCoroutine(StartNextRound_Coroutine(1));
					
				}
			}
		}

        private IEnumerator StartNextRound_Coroutine (float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            _pool.CreateEntity().willStartNextRound = true;
        }

        private void ChangeScore(int whiteScoreDelta, int blackScoreDelta)
        {
            var whiteScore = _gameEntity.score.whiteScore + whiteScoreDelta;
            var blackScore = _gameEntity.score.blackScore + blackScoreDelta;
            _gameEntity.ReplaceScore (whiteScore, blackScore);

        }
    }
}