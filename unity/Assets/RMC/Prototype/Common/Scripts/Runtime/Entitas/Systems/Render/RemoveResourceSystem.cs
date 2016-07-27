using System.Collections.Generic;
using Entitas;
using UnityEngine;


namespace RMC.Common.Entitas.Systems
{
		
	public class RemoveResourceSystem : IMultiReactiveSystem, ISetPool, IEnsureComponents 
		{
		public TriggerOnEvent[] triggers {
			get {
				throw new System.NotImplementedException ();
			}
		}

		#region IEnsureComponents implementation

		public IMatcher ensureComponents {
			get {
				throw new System.NotImplementedException ();
			}
		}

		#endregion

		#region ISetPool implementation

		public void SetPool (Pool pool)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		public void Execute (List<Entity> entities)
		{
			throw new System.NotImplementedException ();
		}

	//    public TriggerOnEvent[] triggers { get { return new [] {
	//            CoreMatcher.Resource.OnEntityRemoved(),
	//            Matcher.AllOf(CoreMatcher.Resource, CoreMatcher.Destroy).OnEntityAdded()
	//        }; } }
	//
	//    public IMatcher ensureComponents { get { return CoreMatcher.View; } }
	//
	//    public void SetPool(Pool pool) {
	//        pool.GetGroup(CoreMatcher.View).OnEntityRemoved += onEntityRemoved;
	//    }
	//
	//    void onEntityRemoved(Group group, Entity entity, int index, IComponent component) {
	//        var viewComponent = (ViewComponent)component;
	//        Object.Destroy(viewComponent.gameObject);
	//    }
	//
	//    public void Execute(List<Entity> entities) {
	//        foreach (var e in entities) {
	//            e.RemoveView();
	//        }
	//    }
	}
}

