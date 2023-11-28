using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.PlayerInput
{
    public class PlayerInputData
    {
        public Vector2 movement = Vector2.zero;
        public Vector2 cameraRotation = Vector2.zero;
        public bool runMode = false;

        public bool defenseActive = false;
        public bool AttackRequested { get; set; } = false;

        private float attackRequestedTime = 0f;
        private readonly float attackBufferTime = 1.5f;

        public void Attack()
        {
            AttackRequested = true;
            attackRequestedTime = Time.time;
        }

        public void CheckAttackState()
        {
            if (AttackRequested)
            {
                float ellapsedTime = Time.time - attackRequestedTime;
                if (ellapsedTime > attackBufferTime)
                {
                    ConsumeAttack();
                }
            }
        }

        public void ConsumeAttack()
        {
            AttackRequested = false;
            attackRequestedTime = 0f;
        }
    }
}