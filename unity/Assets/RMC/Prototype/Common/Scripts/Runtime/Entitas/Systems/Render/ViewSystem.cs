using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems
{
	
	public class ViewSystem : IReactiveSystem, IEnsureComponents 
	{

		public TriggerOnEvent trigger { get { return Matcher.AllOf(Matcher.View, Matcher.Position).OnEntityAdded(); } }

		public IMatcher ensureComponents { get { return Matcher.View; } }

	    public void Execute(List<Entity> entities) 
		{
			//Debug.Log ("pos ex");
	        foreach (var e in entities) 
			{
	            var pos = e.position;
	            e.view.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
	        }
	    }
	}
}

