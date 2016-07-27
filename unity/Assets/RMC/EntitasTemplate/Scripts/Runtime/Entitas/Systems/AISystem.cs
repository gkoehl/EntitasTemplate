using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components;

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
		private Group _group;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_group = pool.GetGroup(Matcher.AllOf(Matcher.AI, Matcher.Position, Matcher.Velocity));

		}

		public void Execute() 
		{

			foreach (var e in _group.GetEntities()) 
			{
				Vector3 velocity = Vector3.zero;
				PositionComponent targetPosition = (PositionComponent)e.aI.targetEntity.GetComponent (ComponentIds.Position);
				if (targetPosition.y > e.position.y + e.aI.deadZoneY) 
				{
					velocity = new Vector3 (0, e.aI.velocityY, 0);
				} 
				else if (targetPosition.y < e.position.y - e.aI.deadZoneY) 
				{
					velocity = new Vector3 (0, -e.aI.velocityY, 0);
				}

				e.ReplaceVelocity(velocity.x, velocity.y, velocity.z);
			}
		}


	}
}