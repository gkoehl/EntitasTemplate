//Uncomment to remove debugging functionality
//#define ENTITAS_DISABLE_VISUAL_DEBUGGING
using UnityEngine;

namespace RMC.EntitasTemplate.Entitas
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class GameConstants
	{
		// ------------------ Constants and statics
        public static readonly Vector3 BallFriction = new Vector3 (0, 0.03f, 0);
        public static readonly float PaddleFriction = 0.25f;

        //
        public const string Audio_ButtonClickSuccess = "Audio/SoundEffects/ButtonClickSuccess";
        public const string Audio_Collision = "Audio/SoundEffects/Collision";
        public const string Audio_GoalFailure = "Audio/SoundEffects/GoalFailure";
        public const string Audio_GoalSuccess = "Audio/SoundEffects/GoalSuccess";

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields

		// ------------------ Methods

	}
}