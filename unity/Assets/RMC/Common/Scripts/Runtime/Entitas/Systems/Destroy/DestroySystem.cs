using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Systems.Destroy
{
    public class DestroySystem : IReactiveSystem, ISetPool
    {
        private Pool _pool;

        public void Execute(List<Entity> entities)
        {
            foreach (var e in entities)
                _pool.DestroyEntity(e);
        }

        public TriggerOnEvent trigger
        {
            get { return Matcher.Destroy.OnEntityAdded(); }
        }

        public void SetPool(Pool pool)
        {
            _pool = pool;
        }
    }
}
