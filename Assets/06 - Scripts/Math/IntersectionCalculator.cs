using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Math
{
    public static class IntersectionCalculator
    {
        public static bool DoesContainPoint(Prism cube, Vector3 point)
        {
            return cube.ContainsPoint(point);
        }

        public static bool IntersectLineWithSquare(LineSegment line, PrismFace quad, out Vector3 intersection)
        {
            return quad.TryToIntersect(line, out intersection);
        }

        public static bool IntersectColliders(Collider c1, Collider c2, out Vector3 intersection)
        {
            Prism p1 = new Prism(c1);
            Prism p2 = new Prism(c2);
            return p1.TryToIntersect(p2, out intersection);
        }

        public static bool IntersectPrisms(Prism p1, Prism p2, out Vector3 intersection)
        {
            return p1.TryToIntersect(p2, out intersection);
        }
    }
}