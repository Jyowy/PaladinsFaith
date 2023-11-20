using log4net.Util;
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
        private new Rigidbody rigidbody = null;

        public override void PlanarMove(Vector3 worldDirection)
        {
            if (worldDirection.magnitude == 0f)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            float velocityFactor = GetVelocityFactor();
            Vector3 planarVelocity = worldDirection * velocityFactor;
            Vector3 currentVelocity = rigidbody.velocity;

            bool tooDifferent = AreDirectionsTooDifferent(worldDirection, currentVelocity);

            if (tooDifferent)
            {
                rigidbody.velocity = planarVelocity;
            }
            else
            {
                float newSpeed = planarVelocity.magnitude;
                float currentSpeed = currentVelocity.magnitude;
                float resultSpeed = Mathf.Max(newSpeed, currentSpeed);

                Vector3 newVelocity = worldDirection * resultSpeed;
                rigidbody.velocity = newVelocity;
            }

            OnMoved();
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

        private void OnMoved()
        {
            RotateToForward();
        }

        private void RotateToForward()
        {
            Vector3 direction = rigidbody.velocity.normalized;
            Vector3 normalizedDirecition = direction.WithY(0f).normalized;
            Quaternion rotation = Quaternion.LookRotation(normalizedDirecition, Vector3.up);
            rigidbody.rotation = rotation;

            //Debug.Log($"Rotate To Forward {rotation}, direction {direction}");
        }
    }
}
