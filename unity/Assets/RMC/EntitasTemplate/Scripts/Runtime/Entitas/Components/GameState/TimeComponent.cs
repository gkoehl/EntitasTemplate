using Entitas;
using Entitas.CodeGenerator;

namespace RMC.EntitasTemplate.Entitas.Components.GameState
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	[SingleEntity]
	public class TimeComponent : IComponent
	{
		// ------------------ Serialized fields and properties
        public float timeSinceGameStartUnpaused = 0;
        public float timeSinceGameStartTotal = 0;
        public bool isPaused = false;

	}
}