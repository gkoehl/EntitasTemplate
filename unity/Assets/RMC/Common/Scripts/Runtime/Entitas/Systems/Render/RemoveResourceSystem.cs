using System.Collections.Generic;
using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components.Render;


namespace RMC.Common.Entitas.Systems.Render
{
		
    /// <summary>
    /// Destroys the Unity gameObject related to an Entity when the Entity is destroyed
    /// </summary>
	public class RemoveResourceSystem : IMultiReactiveSystem, ISetPool, IEnsureComponents 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
		public TriggerOnEvent[] triggers 
        { 
            get { return new [] {
					Matcher.Resource.OnEntityRemoved() //,
					//Matcher.AllOf(Matcher.Resource).OnEntityAdded()
				}; } 
        }

		public IMatcher ensureComponents { get { return Matcher.View; } }

		// ------------------ Non-serialized fields

		// ------------------ Methods

	    public void SetPool(Pool pool) {
			pool.GetGroup(Matcher.View).OnEntityRemoved += onEntityRemoved;
	    }

	
	    public void Execute(List<Entity> entities) 
		{
	        foreach (var e in entities) 
			{
	            e.RemoveView();
	        }
	    }


		private void onEntityRemoved(Group group, Entity entity, int index, IComponent component) 
		{
			var viewComponent = (ViewComponent)component;
			Object.Destroy(viewComponent.gameObject);
		}
	}
}

