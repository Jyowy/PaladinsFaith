using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Player
{
    public class PlayerMoveRigidbodyByForce : PlayerMoveModule
    {
        [SerializeField]
        private float walkForce = 1f;
        [SerializeField]
        private float runForce = 2f;

        [SerializeField]
        private new Rigidbody rigidbody = null;

        public override void PlanarMove(Vector3 worldDirection)
        {
            float forceFactor = GetForceFactor();
            Vector3 force = worldDirection * forceFactor;
            rigidbody.AddForce(force);
        }

        private float GetForceFactor()
        {
            return MoveMode == PlayerMoveMode.Walking ? walkForce : runForce;
        }
    }
}
