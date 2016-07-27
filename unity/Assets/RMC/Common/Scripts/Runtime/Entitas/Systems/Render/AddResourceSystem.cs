using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;


namespace RMC.Common.Entitas.Systems
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
		private readonly Transform _viewContainer = new GameObject("Views").transform;

		// ------------------ Methods

		public void Execute(List<Entity> entities) 
		{
			foreach (var e in entities) 
			{
	            var res = Resources.Load<GameObject>(e.resource.name);
				Debug.Log ("added");
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

