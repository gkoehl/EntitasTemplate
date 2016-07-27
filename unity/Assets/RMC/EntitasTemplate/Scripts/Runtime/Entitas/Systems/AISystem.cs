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
				Vector3 nextVelocity = Vector3.zero;
				PositionComponent targetPositionComponent = (PositionComponent)e.aI.targetEntity.GetComponent (ComponentIds.Position);
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