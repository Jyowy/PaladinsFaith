using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Math
{
    public struct Plane
    {
        private enum PlaneType
        {
            X,
            Y,
            Z,
            Other
        }

        public Vector3 point;
        public Vector3 right;
        public Vector3 up;
        public Vector3 normal;

        private readonly float a;
        private readonly float b;
        private readonly float c;
        private readonly float d;

        public Plane(PrismFace face)
        {
            point = face.center;
            right = face.right;
            up = face.up;
            normal = face.normal;

            Vector3 v01 = up;
            Vector3 v02 = right;

            Vector3 p0 = point;
            Vector3 cross = Vector3.Cross(v01, v02);
            a = cross.x;
            b = cross.y;
            c = cross.z;
            d = a * p0.x + b * p0.y + c * p0.z;
        }

        public readonly bool TryToInteresect(LineSegment lineSegment, out Vector3 point)
        {
            point = Vector3.zero;
            Line line = new Line(lineSegment);

            bool lineAndPlaneAreParallel = Geometry.ArePerpendicular(line.direction, normal);
            bool doIntersect = false;

            if (!lineAndPlaneAreParallel)
            {
                point = GetIntersection(line);
                doIntersect = lineSegment.DoesContainLinearPoint(point);
                if (!doIntersect)
                {
                    point = Vector3.zero;
                }
            }

            return doIntersect;
        }

        private readonly Vector3 GetIntersection(Line line)
        {
            Vector3 p0 = line.referencePoint;
            Vector3 v0 = line.direction;

            float denominator = (a * v0.x + b * v0.y + c * v0.z);
            float t = denominator != 0f
                ? (d - (a * p0.x + b * p0.y + c * p0.z)) / denominator
                : 0f;
            float x = p0.x + t * v0.x;
            float y = p0.y + t * v0.y;
            float z = p0.z + t * v0.z;

            Vector3 point = new Vector3(x, y, z);
            return point;
        }
    }
}