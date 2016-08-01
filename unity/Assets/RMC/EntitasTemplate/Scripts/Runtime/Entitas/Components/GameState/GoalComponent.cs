using Entitas;
using Entitas.CodeGenerator;

namespace RMC.EntitasTemplate.Entitas.Components.GameState
{
	/// <summary>
	/// Stores reward for goals
	/// </summary>
    [SingleEntity]
	public class GoalComponent : IComponent
	{
		// ------------------ Serialized fields and properties
		public int pointsPerGoal = 1;

	}
}