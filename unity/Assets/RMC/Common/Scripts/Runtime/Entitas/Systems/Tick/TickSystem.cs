using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems.Tick
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class TickSystem : IExecuteSystem, ISetPool 
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
			_group = pool.GetGroup(Matcher.AllOf(Matcher.Tick));

		}

		public void Execute() 
		{
			foreach (var e in _group.GetEntities()) 
			{
                e.ReplaceTick(Time.deltaTime);
			}
		}


	}
}