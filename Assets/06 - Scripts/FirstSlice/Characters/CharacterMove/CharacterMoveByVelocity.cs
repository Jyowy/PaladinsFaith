using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Characters
{
    public class CharacterMoveByVelocity : CharacterMoveRigidbody
    {
        [SerializeField]
        private float velocity = 1f;

        public override void PlanarMove(Vector3 worldDirection)
        {
            Vector3 gravity = rigidbody.velocity.OnlyY();

            if (worldDirection.magnitude == 0f)
            {
                rigidbody.velocity = gravity;
                rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            float moveModeFactor = GetMoveModeFactor();
            float newVelocity = velocity * moveModeFactor;
            Vector3 planarVelocity = worldDirection * newVelocity;
            Vector3 directedVelocity = planarVelocity;
            rigidbody.velocity = directedVelocity + gravity;

            OnMoved();
        }
    }
}
