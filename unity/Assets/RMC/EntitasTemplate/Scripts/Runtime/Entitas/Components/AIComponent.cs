using Entitas;

namespace RMC.EntitasTemplate.Entitas.Components
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class AIComponent : IComponent
	{
		// ------------------ Serialized fields and properties
		public Entity targetEntity;
		public float deadZoneY = 1;
		public float velocityY = 0.5f;

	}
}