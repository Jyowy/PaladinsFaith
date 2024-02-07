using PaladinsFaith.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Player
{
    public class CharacterMoveByForce : CharacterMoveRigidbody
    {
        [SerializeField]
        private float force = 1f;

        public override void PlanarMove(Vector3 worldDirection)
        {
            float moveModeFactor = GetMoveModeFactor();
            float newForce = force * moveModeFactor;
            Vector3 directedForce = worldDirection * newForce;
            rigidbody.AddForce(directedForce);

            OnMoved();
        }
    }
}
