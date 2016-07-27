using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems.Render
{
	
	public class ViewSystem : IReactiveSystem, IEnsureComponents 
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
		public TriggerOnEvent trigger { get { return Matcher.AllOf(Matcher.View, Matcher.Position).OnEntityAdded(); } }

		public IMatcher ensureComponents { get { return Matcher.View; } }

		// ------------------ Non-serialized fields

		// ------------------ Methods
	    public void Execute(List<Entity> entities) 
		{
			//Debug.Log ("pos ex");
	        foreach (var e in entities) 
			{
	            e.view.gameObject.transform.position = e.position.position;
	        }
	    }
	}
}

