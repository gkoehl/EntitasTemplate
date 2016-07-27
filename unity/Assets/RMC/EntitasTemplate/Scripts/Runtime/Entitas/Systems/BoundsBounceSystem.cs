﻿using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using System;

namespace RMC.Common.Entitas.Systems
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class BoundsBounceSystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _group;
		private Group _gameBounds;
		private Group _gameScore;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_group = pool.GetGroup(Matcher.AllOf(Matcher.BoundsBounce, Matcher.Position, Matcher.Velocity));
			_gameBounds = pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds));
			_gameScore = pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Score));

		}

		public void Execute() 
		{
			foreach (var e in _group.GetEntities()) 
			{
				Vector3 nextVelocity = e.velocity.velocity;
				float bounceAmount = e.boundsBounce.bounceAmount;
				Bounds bounds = _gameBounds.GetSingleEntity().bounds.bounds;

					

				if (e.position.position.y < bounds.min.y) 
				{
					nextVelocity = new Vector3 (nextVelocity.x, nextVelocity.y * bounceAmount, nextVelocity.z);

					ChangeScore();
				} 
				else if (e.position.position.y > bounds.max.y)
				{
					nextVelocity = new Vector3 (nextVelocity.x, nextVelocity.y * bounceAmount, nextVelocity.z);
					ChangeScore();
				}

				e.ReplaceVelocity(nextVelocity);
			}
		}

        private void ChangeScore()
        {
			
            var whiteScore = _gameScore.GetSingleEntity().score.whiteScore + 1;
			var blackScore = _gameScore.GetSingleEntity().score.blackScore - 1;
			_gameScore.GetSingleEntity().ReplaceScore (whiteScore, blackScore);

			//Debug.Log ("is2 : " + _gameScore.GetSingleEntity().score.whiteScore);
        }
    }
}