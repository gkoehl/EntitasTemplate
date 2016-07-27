using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using System;

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
		private Group _group;
		private Group _gameBoundsGroup;
		private Group _gameScoreGroup;
		private Pool _pool;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_pool = pool;
			_group = _pool.GetGroup(Matcher.AllOf(Matcher.Goal, Matcher.Position, Matcher.Velocity));
			_gameBoundsGroup = _pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds));
			_gameScoreGroup = _pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Score));

		}

		public void Execute() 
		{
			foreach (var e in _group.GetEntities()) 
			{
				Vector3 nextVelocity = e.velocity.velocity;
				float bounceAmount = e.boundsBounce.bounceAmount;
				Bounds bounds = _gameBoundsGroup.GetSingleEntity().bounds.bounds;

				if (e.position.position.x < bounds.min.x) 
				{
					//white
					ChangeScore(e.goal.pointsPerGoal, 0);
					e.willDestroy = true;
					_pool.CreateEntity().willStartNextRound = true;
				} 
				else if (e.position.position.x > bounds.max.x)
				{
					//black
					ChangeScore(0, e.goal.pointsPerGoal);
					e.willDestroy = true;
					_pool.CreateEntity().willStartNextRound = true;
				}
			}
			
		}

        private void ChangeScore(int whiteScoreDelta, int blackScoreDelta)
        {
			
            var whiteScore = _gameScoreGroup.GetSingleEntity().score.whiteScore + whiteScoreDelta;
			var blackScore = _gameScoreGroup.GetSingleEntity().score.blackScore + blackScoreDelta;
			_gameScoreGroup.GetSingleEntity().ReplaceScore (whiteScore, blackScore);

        }
    }
}