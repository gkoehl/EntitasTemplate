using Entitas;
using Entitas.CodeGenerator;

namespace RMC.Common.Entitas.Components.Destroy
{
    //Since this class has no properties this attribute is required for it to be generated - srivello
    //e.g. entity.willDestroy()
    [CustomPrefix("will")]
    public class DestroyComponent : IComponent
    {
    }
}
