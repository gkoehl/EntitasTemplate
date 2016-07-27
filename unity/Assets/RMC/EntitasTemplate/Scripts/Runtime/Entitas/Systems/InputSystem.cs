using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class InputSystem : IExecuteSystem, ISetPool 
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
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Input, Matcher.Position, Matcher.Velocity));

		}

		public void Execute() 
		{
			//Debug.Log ("MoveSystem.SetPool(), group.count : " + _group.count);

			foreach (var e in _group.GetEntities()) 
			{
				Vector3 nextVelocity = new Vector3 (0, Input.GetAxis ("Vertical"), 0);
				e.ReplaceVelocity(nextVelocity);
			}
		}


	}
}