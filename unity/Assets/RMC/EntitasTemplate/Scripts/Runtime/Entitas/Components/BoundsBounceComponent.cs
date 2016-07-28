using Entitas;

namespace RMC.EntitasTemplate.Entitas.Components
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class BoundsBounceComponent : IComponent
	{
		// ------------------ Serialized fields and properties

        //negative required, "-1" means bounce at same speed as you entered. Default.
		public float bounceAmount = -1;		
	}
}