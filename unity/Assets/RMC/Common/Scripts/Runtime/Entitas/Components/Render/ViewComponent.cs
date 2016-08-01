using Entitas;
using UnityEngine;

namespace RMC.Common.Entitas.Components.Render
{
	/// <summary>
	/// Stores the gameObject and similar info to render the Entity 
	/// </summary>
	public class ViewComponent : IComponent
	{
		// ------------------ Serialized fields and properties
		public GameObject gameObject;
        public Bounds bounds;

	}
}