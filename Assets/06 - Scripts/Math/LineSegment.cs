using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Math
{
    [System.Serializable]
    public struct LineSegment
    {
        public Vector3 start;
        public Vector3 end;
        public Vector3 direction;
        public float distance;

        public LineSegment(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            Vector3 vector = end - start;
            direction = vector.normalized;
            distance = vector.magnitude;
        }

        public readonly bool DoesContainLinearPoint(Vector3 point)
        {
            float dist1 = Vector3.Distance(start, point);
            float dist2 = Vector3.Distance(end, point);
            return dist1 <= distance && dist2 <= distance;
        }
    }
}