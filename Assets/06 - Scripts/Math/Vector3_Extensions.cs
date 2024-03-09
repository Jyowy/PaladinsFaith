using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public static class Vector3_Extensions
    {
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            Vector3 newVector = vector;
            newVector.y = y;
            return newVector;
        }

        public static Vector3 OnlyY(this Vector3 vector)
        {
            Vector3 newVector = Vector3.zero;
            newVector.y = vector.y;
            return newVector;
        }

        public static Vector3 NormalizedWithoutY(this Vector3 vector)
        {
            return vector.WithY(0f).normalized;
        }

        public static Vector3 Abs(this Vector3 vector)
        {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }
    }
}