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
        private Rigidbody rigibody = null;

        public override void PlanarMove(Vector2 worldDirection)
        {
            float forceFactor = GetForceFactor();
            Vector2 planeMoveScaled = forceFactor * worldDirection;
            Vector3 force = Vector3.zero;
            force.x = planeMoveScaled.x;
            force.z = planeMoveScaled.y;
            rigibody.AddForce(force);
        }

        private float GetForceFactor()
        {
            return MoveMode == PlayerMoveMode.Walking ? walkForce : runForce;
        }
    }
}
