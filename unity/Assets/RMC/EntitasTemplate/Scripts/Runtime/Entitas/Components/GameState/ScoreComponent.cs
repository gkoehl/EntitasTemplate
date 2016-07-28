using Entitas;
using Entitas.CodeGenerator;

namespace RMC.EntitasTemplate.Entitas.Components.GameState
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	[SingleEntity]
	public class ScoreComponent : IComponent
	{
		// ------------------ Serialized fields and properties
		public int whiteScore;
        public int blackScore;

	}
}