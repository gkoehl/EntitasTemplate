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
        public static readonly float PaddleFriction = 0.5f;
        public static readonly float BallInitialVelocityMinX = 30;
        public static readonly float BallInitialVelocityMaxX = 50;
        public static readonly float BallInitialVelocityMinY = 0;
        public static readonly float BallInitialVelocityMaxY = 20;

        //
        public const string Audio_ButtonClickSuccess = "Audio/SoundEffects/ButtonClickSuccess";
        public const string Audio_Collision = "Audio/SoundEffects/Collision";
        public const string Audio_GoalFailure = "Audio/SoundEffects/GoalFailure";
        public const string Audio_GoalSuccess = "Audio/SoundEffects/GoalSuccess";

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields

		// ------------------ Methods

        /// <summary>
        /// Return random starting velocity. Called for each StartNextRound
        /// </summary>
        public static Vector3 GetBallInitialVelocity ()
        {
            return new Vector3
            (
                    GetRandomRangePosNeg (BallInitialVelocityMinX, BallInitialVelocityMaxX),
                    GetRandomRangePosNeg (BallInitialVelocityMinY, BallInitialVelocityMaxY),
                0
            );

        }


        /// <summary>
        /// Gets a random within range with equal odds of being positive or negative
        /// </summary>
        public static float GetRandomRangePosNeg (float positiveMin, float positiveMax)
        {
            //start with only positive
            float randomResult = Random.Range(Mathf.Abs (positiveMin), Mathf.Abs(positiveMax));
            if (Random.value > 0.5)
            {
                randomResult *= -1;   
            }
            return randomResult;
           
        }

	}
}