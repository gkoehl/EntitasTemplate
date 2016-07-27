using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems.Transform
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class VelocitySystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Group _group;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<VelocitySystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and position component
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Velocity, Matcher.Position));

		}

		public void Execute() 
		{
			//Debug.Log ("MoveSystem.SetPool(), group.count : " + _group.count);

			foreach (var e in _group.GetEntities()) 
			{
				Vector3 velocity = e.velocity.velocity;
				Vector3 position = e.position.position;
				e.ReplacePosition(new Vector3 (position.x + velocity.x, position.y + velocity.y, position.z + velocity.z));

				// if (e.positionition.y > 1.2 && e.hasResource)
				// {
				// 	e.RemoveResource();
				// }
			}
		}


	}
}