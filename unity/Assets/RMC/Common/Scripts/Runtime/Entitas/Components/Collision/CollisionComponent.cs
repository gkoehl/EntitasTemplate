using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Components.Collision
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class CollisionComponent : IComponent
	{
		public enum CollisionType
		{
			TriggerEnter,
			TriggerStay,
			TriggerExit

		}
		// ------------------ Serialized fields and properties
		public GameObject gameObject;
		public Collider collider;
		public CollisionType collisionType;


	}
}