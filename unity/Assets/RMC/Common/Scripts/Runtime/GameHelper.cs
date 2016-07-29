using UnityEngine;

namespace RMC.Common.Entitas.Helpers
{
    /// <summary>
    /// Replace me with description.
    /// </summary>
    public class GameHelper
    {
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties

        // ------------------ Non-serialized fields

        // ------------------ Methods

        /// <summary>
        /// Gives the 3D coordinates for the edge of the camera at a z of 0
        /// </summary>
        public static Bounds GetOrthographicBounds(Camera camera)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds
            (
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0)
            );
            return bounds;
        }


    }


}
