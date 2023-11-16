using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Player
{
    public class PlayerMoveRigidbodyByVelocity : PlayerMoveModule
    {
        [SerializeField]
        private float walkVelocity = 1f;
        [SerializeField]
        private float runVelocity = 2f;

        [SerializeField]
        private Rigidbody rigibody = null;

        public override void PlanarMove(Vector2 worldDirection)
        {
            if (worldDirection.magnitude == 0f)
            {
                return;
            }

            float velocityFactor = GetVelocityFactor();
            Vector3 planarDirection = Vector3.zero;
            planarDirection.x = worldDirection.x;
            planarDirection.z = worldDirection.y;

            Vector3 planarVelocity = planarDirection * velocityFactor;
            Vector3 currentVelocity = rigibody.velocity;

            bool tooDifferent = AreDirectionsTooDifferent(planarDirection, currentVelocity);

            if (tooDifferent)
            {
                rigibody.velocity = planarVelocity;
            }
            else
            {
                float newSpeed = planarVelocity.magnitude;
                float currentSpeed = currentVelocity.magnitude;
                float resultSpeed = Mathf.Max(newSpeed, currentSpeed);

                Vector3 newVelocity = planarDirection * resultSpeed;
                rigibody.velocity = newVelocity;
            }
        }

        private float GetVelocityFactor()
        {
            return MoveMode == PlayerMoveMode.Walking ? walkVelocity : runVelocity;
        }

        [ShowInInspector]
        public float tooDifferentThresshold = 1f;

        private bool AreDirectionsTooDifferent(Vector3 dir1, Vector3 dir2)
        {
            float dotProduct = Vector2.Dot(dir1.normalized, dir2.normalized);
            bool tooDifferent = dotProduct < tooDifferentThresshold;
            return tooDifferent;
        }
    }
}
