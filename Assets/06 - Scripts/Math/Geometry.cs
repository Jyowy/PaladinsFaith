using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Math
{
    public static class Geometry
    {
        public static Vector3 GetNormal(Vector3 up, Vector3 right)
        {
            Vector3 normal = Vector3.Cross(right, up);
            return normal;
        }

        public static bool ArePerpendicular(Vector3 v1, Vector3 v2)
        {
            float dot = Vector3.Dot(v1, v2);
            return dot == 0f;
        }

        public static float AproximateAngleFromDot(Vector3 vector1, Vector3 vector2)
        {
            float dot = Vector3.Dot(vector1, vector2);
            float normalizedDot = Mathf.InverseLerp(1f, -1f, dot);
            float aproximatedAngle = normalizedDot * 360f;
            return aproximatedAngle;
        }
    }
}