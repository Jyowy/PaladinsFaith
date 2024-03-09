using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Math
{
    public struct Line
    {
        private enum LineType
        {
            X,
            Y,
            Z,
            Other
        }

        public Vector3 referencePoint;
        public Vector3 direction;

        public float mxy;
        public float nxy;
        public float mxz;
        public float nxz;

        public float myx;
        public float nyx;
        public float myz;
        public float nyz;

        public float mzx;
        public float nzx;
        public float mzy;
        public float nzy;

        private LineType lineType;

        public Line(LineSegment line)
        {
            referencePoint = line.start;
            direction = (line.end - line.start).normalized;

            Vector3 p0 = line.start;
            Vector3 p1 = line.end;

            float dx = p1.x - p0.x;
            float dy = p1.y - p0.y;
            float dz = p1.z - p0.z;

            myx = 0f;
            nyx = 0f;
            myz = 0f;
            nyz = 0f;

            mzx = 0f;
            nzx = 0f;
            mzy = 0f;
            nzy = 0f;

            lineType = LineType.Other;

            mxy = dx != 0f ? dy / dx : 0f;
            nxy = p0.y - p0.x * mxy;
            mxz = dx != 0f ? dz / dx : 0f;
            nxz = p0.z - p0.x * mxz;

            myx = dy != 0f ? dx / dy : 0f;
            nyx = p0.x - p0.y * myx;
            myz = dy != 0f ? dz / dy : 0f;
            nyz = p0.z - p0.y * myz;

            mzx = dz != 0f ? dx / dz : 0f;
            nzx = p0.x - p0.z * mzx;
            mzy = dz != 0f ? dy / dz : 0f;
            nzy = p0.y - p0.z * mzy;
        }

        public readonly Vector3 GetPointAtX(float x)
        {
            Vector3 point = referencePoint;
            if (direction.x != 0f)
            {
                float y = mxy * x + nxy;
                float z = mxz * x + nxz;
                point = new Vector3(x, y, z);
            }
            return point;
        }

        public readonly Vector3 GetPointAtY(float y)
        {
            Vector3 point = referencePoint;
            if (direction.y != 0f)
            {
                float x = myx * y + nyx;
                float z = myz * y + nyz;
                point = new Vector3(x, y, z);
            }
            return point;
        }

        public readonly Vector3 GetPointAtZ(float z)
        {
            Vector3 point = referencePoint;
            if (direction.z != 0f)
            {
                float x = mzx * z + nzx;
                float y = mzy * z + nzy;
                point = new Vector3(x, y, z);
            }
            return point;
        }
    }
}