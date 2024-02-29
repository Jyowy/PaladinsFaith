using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PaladinsFaith.Math
{
    [System.Serializable]
    public struct Line
    {
        public Vector3 start;
        public Vector3 end;
    }

    [System.Serializable]
    public struct Square
    {
        public Vector3 center;
        public Vector2 size;
        public Vector3 up;
    }

    [System.Serializable]
    public struct Cube
    {
        public Vector3 center;
        public Vector3 size;
        public Vector3 halfSize;
        public Vector3 forward;
        public Vector3 up;

        public Cube(Bounds bounds, Vector3 forward, Vector3 up)
            : this(bounds.center, bounds.size, forward, up)
        {
        }

        public Cube(Vector3 origin, Vector3 size, Vector3 forward, Vector3 up)
        {
            center = origin;
            this.size = size;
            halfSize = size * 0.5f;
            this.forward = forward;
            this.up = up;
        }

        public bool ContainsNormalizedPoint(Vector3 normalizedPoint)
        {
            return Mathf.Abs(normalizedPoint.x) - halfSize.x <= 0f
                && Mathf.Abs(normalizedPoint.y) - halfSize.y <= 0f
                && Mathf.Abs(normalizedPoint.z) - halfSize.z <= 0f;
        }
    }

    public static class IntersectionCalculator
    {
        public static bool DoesContainPoint(Cube cube, Vector3 point)
        {
            Vector3 offset = cube.center;
            //Quaternion rotation = Quaternion.FromToRotation(cube.forward, Vector3.forward);
            //Quaternion rotation = Quaternion.FromToRotation(cube.forward, Vector3.forward);
            Quaternion rotation = Quaternion.Inverse(Quaternion.LookRotation(cube.forward, cube.up));
            Vector3 movedPoint = point - offset;
            Vector3 normalizedPoint = rotation * movedPoint;
            Debug.Log($"Point: {point} => moved {movedPoint}, normalized {normalizedPoint} (rotation {rotation.eulerAngles})");
            return cube.ContainsNormalizedPoint(normalizedPoint);
        }

        public static bool IntersectLineWithSquare(Line line, Square square, out Vector3 intersection)
        {
            bool interesct = false;
            intersection = Vector3.zero;

            // TODO


            return interesct;
        }
    }
}