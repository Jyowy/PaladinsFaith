using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaladinsFaith.Math
{
    [System.Serializable]
    public struct PrismFace
    {
        public Vector3 center;
        public Vector2 size;
        public Vector2 halfSize;
        public Vector3 up;
        public Vector3 right;
        public Vector3 normal;

        public Vector3[] corners;

        public PrismFace(Vector3 leftTop, Vector3 rightTop, Vector3 rightBottom, Vector3 leftBottom)
        {
            corners = new Vector3[] { leftTop, rightTop, rightBottom, leftBottom };

            center = (leftTop + rightTop + rightBottom + leftBottom) / 4f;

            float sizeX = Vector3.Distance(leftTop, rightTop);
            float sizeY = Vector3.Distance(rightTop, rightBottom);
            size = new Vector2(sizeX, sizeY);
            halfSize = size * 0.5f;

            up = (rightTop - rightBottom).normalized;
            right = (rightTop - leftTop).normalized;
            normal = Geometry.GetNormal(up, right);
        }

        public PrismFace(Vector3 center, Vector2 size, Vector3 up, Vector3 right)
        {
            this.center = center;
            this.size = size;
            this.halfSize = size * 0.5f;
            this.up = up;
            this.right = right;
            normal = Geometry.GetNormal(up, right);
            corners = null;
            corners = GetCorners();
        }

        private readonly Vector3[] GetCorners()
        {
            Vector3[] corners = new Vector3[4];
            corners[0] = center + right * halfSize.x + up * halfSize.y;
            corners[1] = center - right * halfSize.x + up * halfSize.y;
            corners[2] = center - right * halfSize.x - up * halfSize.y;
            corners[3] = center + right * halfSize.x - up * halfSize.y;
            return corners;
        }

        public readonly bool DoesContainPlanarPoint(Vector3 point)
        {
            Vector3 centerToPoint = point - center;
            float distance = centerToPoint.magnitude;
            float angle = Vector3.SignedAngle(centerToPoint, up, normal);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(rad) * distance;
            float y = Mathf.Cos(rad) * distance;

            /*
            Gizmos.color = Color.red;
            Gizmos.DrawLine(center, point);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(center, center + right * x);
            Gizmos.DrawLine(center + up * y, center + right * x + up * y);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(center, center + up * y);
            Gizmos.DrawLine(center + right * x, center + right * x + up * y);
            //*/
            return Mathf.Abs(x) <= halfSize.x && Mathf.Abs(y) <= halfSize.y;

            //*
            //*/

            /*
            float xmin = corners.Min(corner => corner.x);
            float xmax = corners.Max(corner => corner.x);
            float ymin = corners.Min(corner => corner.y);
            float ymax = corners.Max(corner => corner.y);
            float zmin = corners.Min(corner => corner.z);
            float zmax = corners.Max(corner => corner.z);
            return xmin <= point.x && point.x <= xmax
                && ymin <= point.y && point.y <= ymax
                && zmin <= point.z && point.z <= zmax;
            //*/
        }

        public readonly bool TryToIntersect(LineSegment lineSegment, out Vector3 intersection)
        {
            Plane plane = new Plane(this);
            bool doIntersect = plane.TryToInteresect(lineSegment, out intersection);
            if (doIntersect)
            {
                doIntersect = this.DoesContainPlanarPoint(intersection);
                if (!doIntersect)
                {
                    intersection = Vector3.zero;
                }
            }
            return doIntersect;
        }
    }
}