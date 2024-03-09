using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Math
{
    public class GeometryTest : MonoBehaviour
    {
        [SerializeField]
        private Vector3 vector1 = Vector3.forward;
        [SerializeField]
        private Vector3 vector2 = Vector3.forward;

        [Button]
        public float TestAproximateAngle()
        {
            float aproximatedAngle = Geometry.AproximateAngleFromDot(vector1, vector2);
            return aproximatedAngle;
        }
    }
}