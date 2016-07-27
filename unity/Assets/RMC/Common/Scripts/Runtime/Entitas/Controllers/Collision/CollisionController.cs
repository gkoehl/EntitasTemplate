﻿using UnityEngine;
using Entitas;
using RMC.Common.Entitas.Components.Collision;

namespace RMC.Common.Entitas.Controllers.Collision
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class CollisionController : MonoBehaviour 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
		private Pool _pool;

		// ------------------ Methods
		protected void Start()
        {
			_pool = Pools.pool;
        }

        protected void OnTriggerEnter(Collider collider)
        {
			//Debug.Log ("OnTriggerEnter() collider: " + collider);
			_pool.CreateEntity().AddCollision (gameObject, collider, CollisionComponent.CollisionType.TriggerEnter);
        }

		protected void OnTriggerStay(Collider collider)
        {
			_pool.CreateEntity().AddCollision (gameObject, collider, CollisionComponent.CollisionType.TriggerStay);
        }

		protected void OnTriggerExit(Collider collider)
        {
			_pool.CreateEntity().AddCollision (gameObject, collider, CollisionComponent.CollisionType.TriggerExit);
        }
	}
}