using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using RMC.Common.Entitas.Components.Transform;

namespace RMC.Common.Entitas.Systems.Render
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class AddResourceSystem : IReactiveSystem 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
		public TriggerOnEvent trigger 
		{ 
			get 
			{ 
				return Matcher.Resource.OnEntityAdded(); 
			} 
		}


		// ------------------ Non-serialized fields
		private readonly UnityEngine.Transform _viewContainer = new GameObject("Views").transform;

		// ------------------ Methods

		public void Execute(List<Entity> entities) 
		{
			foreach (var e in entities) 
			{
	            var res = Resources.Load<GameObject>(e.resource.name);
	            GameObject gameObject = null;
	            try {
	                gameObject = UnityEngine.Object.Instantiate(res);
	                
	            } catch (Exception) {
	                Debug.Log("Cannot instantiate " + res);
	            }

	            if (gameObject != null) 
				{
	                gameObject.transform.parent = _viewContainer;
	                e.AddView(gameObject);
	            }
	        }
	   }
	}
}

