using Entitas;
using UnityEngine;

namespace RMC.EntitasTemplate.Entias.Systems
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class MoveSystem : IExecuteSystem, ISetPool 
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
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Move, Matcher.Position));

			Debug.Log ("MoveSystem.SetPool(), group.count : " + _group.count);
		}

		public void Execute() 
		{
			foreach (var e in _group.GetEntities()) 
			{
				var move = e.move;
				var pos = e.position;
				e.ReplacePosition(pos.x, pos.y + move.y, pos.z);
			}
		}


	}
}