using System.Collections.Generic;
using System.Linq;
using Entitas;
using RMC.Common.Entitas.Components.Collision;
using UnityEngine;

namespace RMC.EntitasTemplate.Entitas.Systems.Collision
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class CollisionSystem : IReactiveSystem, ISetPool
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
		public TriggerOnEvent trigger 
		{ 
			get 
			{ 
				return Matcher.Collision.OnEntityAdded(); 
			} 
		}


		// ------------------ Non-serialized fields
		private Group _group;
		private float _paddleFriction = 0.8f;

		// ------------------ Methods
		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Position, Matcher.Velocity));

		}
		public void Execute(List<Entity> entities) 
		{
			foreach (var collisionEntity in entities) 
			{
				if (collisionEntity.collision.collisionType == CollisionComponent.CollisionType.TriggerEnter)
				{
					//Find entities from the unity data
					var ballEntity = _group.GetEntities().FirstOrDefault(e2 => e2.view.gameObject == collisionEntity.collision.gameObject);
					var paddleEntity = _group.GetEntities().FirstOrDefault(e2 => e2.view.gameObject == collisionEntity.collision.collider.gameObject);
					
					//Debug.Log (collisionEntity.collision.collider.gameObject);
					
					//Move the ball and include some of the paddle's y velocity to 'steer' the ball
					Vector3 nextBallVelocity = ballEntity.velocity.velocity;
					Vector3 paddleVelocity = paddleEntity.velocity.velocity;
					ballEntity.ReplaceVelocity ( new Vector3 (-nextBallVelocity.x, nextBallVelocity.y + paddleVelocity.y * _paddleFriction, nextBallVelocity.z) );
					
				}
				collisionEntity.willDestroy = true;
	        }

	   }
	}
}

