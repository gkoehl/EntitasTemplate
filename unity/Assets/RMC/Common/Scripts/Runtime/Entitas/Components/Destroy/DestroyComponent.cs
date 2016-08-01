using Entitas;
using Entitas.CodeGenerator;

namespace RMC.Common.Entitas.Components.Destroy
{
    /// <summary>
    /// Sent like an event: Will destroy the Entity
    /// </summary>
    //Since this class has no properties this attribute is required for it to be generated - srivello
    //e.g. entity.WillDestroy()
    [CustomPrefix("Will")]
    public class DestroyComponent : IComponent
    {
    }
}
