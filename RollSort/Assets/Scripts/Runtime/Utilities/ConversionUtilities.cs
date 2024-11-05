using UnityEngine;

namespace RollSort.Runtime.Utilities
{
    /// <summary>
    ///     Utility class providing methods for extracting angles in degrees from direction vectors.
    /// </summary>
    /// <remarks>
    ///     The class includes methods to derive the angle in degrees from a direction vector in 3D space.
    /// </remarks>
    public static class ConversionUtilities
    {
        /// <summary>
        ///     Get angle in degrees from a direction vector
        /// </summary>
        /// ///
        /// <param name="vector">The direction vector.</param>
        /// <returns>The angle in degrees from the provided direction vector.</returns>
        public static float VectorToAngle(Vector3 vector)
        {
            float radians = Mathf.Atan2(vector.y, vector.x);
            float degrees = radians * Mathf.Rad2Deg;

            return degrees;
        }

        /// <summary>
        ///     Get direction vector from an angle in degrees
        /// </summary>
        /// ///
        /// <param name="angle">The angle.</param>
        /// <returns>The vector from the provided angle in degrees.</returns>
        public static Vector3 AngleToVector(float angle)
        {
            return new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        }
    }
}