using Entitas;

namespace RMC.EntitasTemplate.Entitas.Components
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class PaddleComponent : IComponent
	{
        public enum PaddleType
        {
            White,
            Black
        }
		// ------------------ Serialized fields and properties
		public PaddleType paddleType;

	}
}