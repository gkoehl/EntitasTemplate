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
        float _deltaTime;
        Vector3 _friction;
            
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
            //Debug.Log ("VelocitySystem.Execute(), _group.count : " + _group.count);

			foreach (var e in _group.GetEntities()) 
			{
				Vector3 velocity = e.velocity.velocity;
				Vector3 position = e.position.position;

                //TickComponent is optional
                if (e.hasTick)
                {
                    _deltaTime = e.tick.deltaTime;
                }
                else
                {
                    //default
                    _deltaTime = 1;
                }
                  

                //FrictionComponent is optional
                _friction = Vector3.zero;
                if (e.hasFriction)
                {
                    _friction = e.friction.friction;
                }
                else
                {
                    //default
                    _friction = Vector3.zero;
                }

                e.ReplacePosition(new Vector3 (
                    (position.x + velocity.x * _deltaTime) * (1- _friction.x), 
                    (position.y + velocity.y * _deltaTime) * (1- _friction.y), 
                    (position.z + velocity.z * _deltaTime) * (1- _friction.z)
                ));

			}
		}


	}
}