using PaladinsFaith.Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Characters
{
    public class CharacterMoveByVelocity : CharacterMoveRigidbody
    {
        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private float dashDistance = 2f;
        [SerializeField]
        private float dashDuration = 0.5f;

        [SerializeField]
        private float stopTime = 0.25f;

        private bool isStopping = false;
        private float stopTimer = 0f;

        private Vector3 dashDirection = Vector3.zero;
        private float dashTimer = 0f;
        private float dashSpeed = 0f;

        public override void PlanarMove(Vector3 worldDirection, float speedFactor)
        {
            if (glueToFloor)
            {
                GlueToFloor();
            }

            if (MoveType == CharacterMoveType.Dashing)
            {
                UpdateDash();
                return;
            }

            bool noInput = worldDirection.magnitude == 0f;
            if (noInput)
            {
                if (!IsStopped)
                {
                    if (!isStopping)
                    {
                        isStopping = true;
                        stopTimer = stopTime;
                    }
                    else if (stopTimer > 0f)
                    {
                        stopTimer -= Time.deltaTime;
                        if (stopTimer <= 0f)
                        {
                            isStopping = false;
                            Stop();
                        }
                    }
                }
                
                return;
            }

            if (IsStopped)
            {
                MoveStarted();
            }

            isStopping = false;
            float moveModeFactor = GetMoveModeFactor();
            float newSpeed = speed * moveModeFactor * speedFactor; 
            Vector3 planarVelocity = worldDirection * newSpeed;
            Vector3 gravity = rigidbody.velocity.OnlyY();
            Vector3 newVelocity = planarVelocity + gravity;
            rigidbody.velocity = newVelocity;

            OnMoved();
        }

        protected override void StartDash(Vector3 direction, float speedFactor)
        {
            dashDirection = GetDashDirection(direction);
            dashTimer = dashDuration;
            dashSpeed = GetDashSpeed() * speedFactor;

            Debug.Log($"Dash started: dir {direction}, speed {dashSpeed}, duration {dashDuration}");

            base.StartDash(direction, speedFactor);

            ActivatePhysics();
            UpdateDash();
        }

        private Vector3 GetDashDirection(Vector3 moveDirection)
        {
            Vector3 dashDirection = moveDirection.normalized;
            if (dashDirection.magnitude == 0f)
            {
                dashDirection = -rigidbody.transform.forward;
            }
            return dashDirection;
        }

        private void UpdateDash()
        {
            dashTimer = Mathf.Max(dashTimer - Time.fixedDeltaTime, 0f);
            Vector3 velocity = dashDirection * dashSpeed;
            rigidbody.velocity = velocity;

            if (dashTimer <= 0f)
            {
                DashCompleted();
            }
        }

        private float GetDashSpeed()
        {
            if (dashDuration <= 0f)
            {
                return 0f;
            }

            return dashDistance / dashDuration;
        }
    }
}
