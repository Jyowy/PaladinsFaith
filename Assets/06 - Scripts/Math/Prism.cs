using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PaladinsFaith.Math
{
    [System.Serializable]
    public struct Prism
    {
        public enum Corners
        {
            TopRightFront,
            TopRightBack,
            TopLeftFront,
            TopLeftBack,

            BottomRightFront,
            BottomRightBack,
            BottomLeftFront,
            BottomLeftBack,
        }

        public enum Edges
        {
            TopFront,
            BottomFront,
            LeftFront,
            RightFront,

            TopBack,
            BottomBack,
            LeftBack,
            RightBack,

            RightTop,
            RightBottom,
            LeftTop,
            LeftBottom,
        }

        public enum Faces
        {
            Top,
            Bottom,
            Left,
            Right,
            Front,
            Back
        }

        public Vector3 center;
        public Vector3 size;
        public Vector3 halfSize;
        public Vector3 forward;
        public Vector3 right;
        public Vector3 up;

        public Vector3[] corners;
        public LineSegment[] edges;
        public PrismFace[] faces;

        /*
        public Prism(Collider collider)
            : this(collider.bounds, collider.transform.up, collider.transform.forward, collider.transform.right)
        {
        }
        //*/

        public Prism(Collider collider)
        {
            Transform transform = collider.transform;
            Vector3 centerOffset = Vector3.zero;
            Vector3 scale = transform.lossyScale;
            Vector3 size = Vector3.zero;
            if (collider is BoxCollider box)
            {
                centerOffset = box.center;
                size = box.size;
            }
            else if (collider is SphereCollider sphere)
            {
                centerOffset = sphere.center;
                size = Vector3.one * sphere.radius * 2f;
            }
            else if (collider is CapsuleCollider capsule)
            {
                centerOffset = capsule.center;
                float capsuleDiameter = capsule.radius * 2f;
                size = new Vector3(capsuleDiameter, capsule.height, capsuleDiameter);
            }

            this.center = transform.position
                + transform.right * centerOffset.x * scale.x
                + transform.up * centerOffset.y * scale.y
                + transform.forward * centerOffset.z * scale.z;
            this.size = Vector3.Scale(size, scale);
            halfSize = this.size * 0.5f;
            forward = transform.forward;
            right = transform.right;
            up = transform.up;

            corners = null;
            edges = null;
            faces = null;

            corners = GetCorners();
            edges = GetEdges();
            faces = GetFaces();
        }

        public Prism(Bounds bounds, Vector3 up, Vector3 forward, Vector3 right)
            : this(bounds.center, bounds.size, up, forward, right)
        {
        }

        public Prism(Vector3 origin, Vector3 size, Vector3 up, Vector3 forward, Vector3 right)
        {
            center = origin;
            this.size = size;
            halfSize = size * 0.5f;
            this.forward = forward;
            this.right = right;
            this.up = up;

            corners = null;
            edges = null;
            faces = null;

            corners = GetCorners();
            edges = GetEdges();
            faces = GetFaces();
        }

        private readonly Vector3[] GetCorners()
        {
            Vector3[] corners = new Vector3[8];
            corners[(int)Corners.TopRightFront] = center + right * halfSize.x + up * halfSize.y - forward * halfSize.z;
            corners[(int)Corners.TopRightBack] = center + right * halfSize.x + up * halfSize.y + forward * halfSize.z;
            corners[(int)Corners.TopLeftFront] = center - right * halfSize.x + up * halfSize.y - forward * halfSize.z;
            corners[(int)Corners.TopLeftBack] = center - right * halfSize.x + up * halfSize.y + forward * halfSize.z;
            corners[(int)Corners.BottomRightFront] = center + right * halfSize.x - up * halfSize.y - forward * halfSize.z;
            corners[(int)Corners.BottomRightBack] = center + right * halfSize.x - up * halfSize.y + forward * halfSize.z;
            corners[(int)Corners.BottomLeftFront] = center - right * halfSize.x - up * halfSize.y - forward * halfSize.z;
            corners[(int)Corners.BottomLeftBack] = center - right * halfSize.x - up * halfSize.y + forward * halfSize.z;
            return corners;
        }

        private readonly LineSegment[] GetEdges()
        {
            LineSegment[] edges = new LineSegment[12];
            edges[(int)Edges.TopFront] = new LineSegment(corners[(int)Corners.TopRightFront], corners[(int)Corners.TopLeftFront]);
            edges[(int)Edges.BottomFront] = new LineSegment(corners[(int)Corners.BottomRightFront], corners[(int)Corners.BottomLeftFront]);
            edges[(int)Edges.LeftFront] = new LineSegment(corners[(int)Corners.TopLeftFront], corners[(int)Corners.BottomLeftFront]);
            edges[(int)Edges.RightFront] = new LineSegment(corners[(int)Corners.TopRightFront], corners[(int)Corners.BottomRightFront]);

            edges[(int)Edges.TopBack] = new LineSegment(corners[(int)Corners.TopRightBack], corners[(int)Corners.TopLeftBack]);
            edges[(int)Edges.BottomBack] = new LineSegment(corners[(int)Corners.BottomRightBack], corners[(int)Corners.BottomLeftBack]);
            edges[(int)Edges.LeftBack] = new LineSegment(corners[(int)Corners.TopLeftBack], corners[(int)Corners.BottomLeftBack]);
            edges[(int)Edges.RightBack] = new LineSegment(corners[(int)Corners.TopRightBack], corners[(int)Corners.BottomRightBack]);

            edges[(int)Edges.RightTop] = new LineSegment(corners[(int)Corners.TopRightFront], corners[(int)Corners.TopRightBack]);
            edges[(int)Edges.RightBottom] = new LineSegment(corners[(int)Corners.BottomRightFront], corners[(int)Corners.BottomRightBack]);
            edges[(int)Edges.LeftTop] = new LineSegment(corners[(int)Corners.TopLeftFront], corners[(int)Corners.TopLeftBack]);
            edges[(int)Edges.LeftBottom] = new LineSegment(corners[(int)Corners.BottomLeftFront], corners[(int)Corners.BottomLeftBack]);
            return edges;
        }

        private readonly PrismFace[] GetFaces()
        {
            PrismFace[] faces = new PrismFace[6];
            faces[(int)Faces.Top] = new PrismFace(
                corners[(int)Corners.TopLeftFront],
                corners[(int)Corners.TopRightFront],
                corners[(int)Corners.TopRightBack],
                corners[(int)Corners.TopLeftBack]
            );
            faces[(int)Faces.Bottom] = new PrismFace(
                corners[(int)Corners.BottomRightFront],
                corners[(int)Corners.BottomLeftFront],
                corners[(int)Corners.BottomLeftBack],
                corners[(int)Corners.BottomRightBack]
            );
            faces[(int)Faces.Right] = new PrismFace(
                corners[(int)Corners.TopRightBack],
                corners[(int)Corners.TopRightFront],
                corners[(int)Corners.BottomRightFront],
                corners[(int)Corners.BottomRightBack]
            );
            faces[(int)Faces.Left] = new PrismFace(
                corners[(int)Corners.TopLeftFront],
                corners[(int)Corners.TopLeftBack],
                corners[(int)Corners.BottomLeftBack],
                corners[(int)Corners.BottomLeftFront]
            );
            faces[(int)Faces.Front] = new PrismFace(
                corners[(int)Corners.TopRightFront],
                corners[(int)Corners.TopLeftFront],
                corners[(int)Corners.BottomLeftFront],
                corners[(int)Corners.BottomRightFront]
            );
            faces[(int)Faces.Back] = new PrismFace(
                corners[(int)Corners.TopLeftBack],
                corners[(int)Corners.TopRightBack],
                corners[(int)Corners.BottomRightBack],
                corners[(int)Corners.BottomLeftBack]
            );
            return faces;
        }

        public readonly bool ContainsPoint(Vector3 point)
        {
            Vector3 offset = center;
            Quaternion rotation = Quaternion.Inverse(Quaternion.LookRotation(forward, up));
            Vector3 movedPoint = point - offset;
            Vector3 normalizedPoint = rotation * movedPoint;
            return ContainsNormalizedPoint(normalizedPoint);
        }

        public readonly bool ContainsNormalizedPoint(Vector3 normalizedPoint)
        {
            return Mathf.Abs(normalizedPoint.x) - halfSize.x <= 0f
                && Mathf.Abs(normalizedPoint.y) - halfSize.y <= 0f
                && Mathf.Abs(normalizedPoint.z) - halfSize.z <= 0f;
        }

        public readonly bool TryToIntersect(Prism other, out Vector3 intersectionMiddle)
        {
            bool doIntersect = false;
            intersectionMiddle = Vector3.zero;

            Vector3[] thisCorners = corners;
            Vector3[] otherCorners = other.corners;

            List<Vector3> intersectionPoints = new List<Vector3>();

            foreach (Vector3 corner in thisCorners)
            {
                bool contained = other.ContainsPoint(corner);
                if (contained)
                {
                    intersectionPoints.Add(corner);
                }
            }

            foreach (Vector3 corner in otherCorners)
            {
                bool contained = this.ContainsPoint(corner);
                if (contained)
                {
                    intersectionPoints.Add(corner);
                }
            }

            LineSegment[] thisEdges = edges;
            LineSegment[] otherEdges = other.edges;
            PrismFace[] thisFaces = faces;
            PrismFace[] otherFaces = other.faces;

            foreach (LineSegment segment in thisEdges)
            {
                foreach (PrismFace face in otherFaces)
                {
                    if (face.TryToIntersect(segment, out Vector3 intersectionPoint))
                    {
                        intersectionPoints.Add(intersectionPoint);
                    }
                }
            }

            foreach (LineSegment segment in otherEdges)
            {
                foreach (PrismFace face in thisFaces)
                {
                    if (face.TryToIntersect(segment, out Vector3 intersectionPoint))
                    {
                        intersectionPoints.Add(intersectionPoint);
                    }
                }
            }

            if (intersectionPoints.Count > 0)
            {
                doIntersect = true;
                float xmin = intersectionPoints.Min(point => point.x);
                float xmax = intersectionPoints.Max(point => point.x);
                float ymin = intersectionPoints.Min(point => point.y);
                float ymax = intersectionPoints.Max(point => point.y);
                float zmin = intersectionPoints.Min(point => point.z);
                float zmax = intersectionPoints.Max(point => point.z);

                /*
                Gizmos.color = Color.yellow;
                foreach (Vector3 point in intersectionPoints)
                {
                    Gizmos.DrawSphere(point, 0.1f);
                }
                //*/

                float x = (xmax + xmin) * 0.5f;
                float y = (ymax + ymin) * 0.5f;
                float z = (zmax + zmin) * 0.5f;
                intersectionMiddle = new Vector3(x, y, z);
            }

            return doIntersect;
        }
    }
}