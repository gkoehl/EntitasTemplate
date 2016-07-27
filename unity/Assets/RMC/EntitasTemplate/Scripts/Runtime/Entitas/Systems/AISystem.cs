using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;
using RMC.Common.Entitas.Components.Render;
using RMC.Common.Entitas.Components.Transform;

namespace RMC.Common.Entitas.Systems
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class AISystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _aiGroup;


		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_aiGroup = pool.GetGroup(Matcher.AllOf(Matcher.AI, Matcher.Position, Matcher.Velocity));
			
			Group ballCreatedGroup = pool.GetGroup(Matcher.AllOf(Matcher.Goal, Matcher.BoundsBounce).NoneOf (Matcher.Destroy));
			ballCreatedGroup.OnEntityAdded += OnBallEntityCreated;

			Group ballDestroyGroup = pool.GetGroup(Matcher.AllOf(Matcher.Goal, Matcher.Destroy));
			ballDestroyGroup.OnEntityAdded += OnBallEntityDestroyed;
			
	

		}

		//Whenever a new ball is created, follow it
		protected virtual void OnBallEntityCreated(Group collection, Entity ballEntity, int index, IComponent component)
		{
			//Debug.Log ("created" + ballEntity);
			foreach (var e in _aiGroup.GetEntities()) 
			{
				e.ReplaceAI (ballEntity, e.aI.deadZoneY, e.aI.velocityY);
			}
		}

		//whenever a ball is destroyed, stop following it.
		protected virtual void OnBallEntityDestroyed(Group collection, Entity ballEntity, int index, IComponent component)
		{
			//Debug.Log ("destroy" + ballEntity);
			foreach (var e in _aiGroup.GetEntities()) 
			{
				e.ReplaceAI (null, e.aI.deadZoneY, e.aI.velocityY);
				e.ReplaceVelocity ( new Vector3 (0,0,0));
			}
		}

		public void Execute() 
		{

			foreach (var e in _aiGroup.GetEntities()) 
			{
				Vector3 nextVelocity = Vector3.zero;
		
				Entity targetEntity = e.aI.targetEntity;
				if (targetEntity != null)
				{

					PositionComponent targetPositionComponent = (PositionComponent)targetEntity.GetComponent (ComponentIds.Position);

					Vector3 targetPosition = targetPositionComponent.position;
					if (targetPosition.y > e.position.position.y + e.aI.deadZoneY) 
					{
						nextVelocity = new Vector3 (0, e.aI.velocityY, 0);
					} 
					else if (targetPosition.y < e.position.position.y - e.aI.deadZoneY) 
					{
						nextVelocity = new Vector3 (0, -e.aI.velocityY, 0);
					}

					e.ReplaceVelocity(nextVelocity);
				}

			}
		}


	}
}