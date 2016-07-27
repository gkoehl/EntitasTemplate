//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Entitas {
    public partial class Entity {
        public RMC.Common.Entitas.Components.ResourceComponent resource { get { return (RMC.Common.Entitas.Components.ResourceComponent)GetComponent(ComponentIds.Resource); } }

        public bool hasResource { get { return HasComponent(ComponentIds.Resource); } }

        public Entity AddResource(string newName) {
            var component = CreateComponent<RMC.Common.Entitas.Components.ResourceComponent>(ComponentIds.Resource);
            component.name = newName;
            return AddComponent(ComponentIds.Resource, component);
        }

        public Entity ReplaceResource(string newName) {
            var component = CreateComponent<RMC.Common.Entitas.Components.ResourceComponent>(ComponentIds.Resource);
            component.name = newName;
            ReplaceComponent(ComponentIds.Resource, component);
            return this;
        }

        public Entity RemoveResource() {
            return RemoveComponent(ComponentIds.Resource);
        }
    }

    public partial class Matcher {
        static IMatcher _matcherResource;

        public static IMatcher Resource {
            get {
                if (_matcherResource == null) {
                    var matcher = (Matcher)Matcher.AllOf(ComponentIds.Resource);
                    matcher.componentNames = ComponentIds.componentNames;
                    _matcherResource = matcher;
                }

                return _matcherResource;
            }
        }
    }
}
