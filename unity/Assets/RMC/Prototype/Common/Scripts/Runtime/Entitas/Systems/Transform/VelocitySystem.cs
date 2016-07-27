using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class VelocitySystem : IExecuteSystem, ISetPool 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
		public float x;
		public float y;
		public float z;

		// ------------------ Non-serialized fields
		private Group _group;

		// ------------------ Methods

		// Implement ISetPool to get the pool used when calling
		// pool.CreateSystem<MoveSystem>();
		public void SetPool(Pool pool) 
		{
			// Get the group of entities that have a Move and Position component
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Velocity, Matcher.Position));

			Debug.Log ("Velocity.SetPool(), group.count : " + _group.count);
		}

		public void Execute() 
		{
			//Debug.Log ("MoveSystem.SetPool(), group.count : " + _group.count);

			foreach (var e in _group.GetEntities()) 
			{
				var velocity = e.velocity;
				var pos = e.position;
				e.ReplacePosition(pos.x + velocity.x, pos.y + velocity.y, pos.z + velocity.z);
			}
		}


	}
}