using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Characters
{
    public abstract class CharacterMoveRigidbody : CharacterMoveModule
    {
        [SerializeField]
        protected new Rigidbody rigidbody = null;

        protected override void RotateToForward()
        {
            Vector3 direction = rigidbody.velocity.normalized;
            Vector3 normalizedDirecition = direction.WithY(0f).normalized;
            Quaternion rotation = Quaternion.LookRotation(normalizedDirecition, Vector3.up);
            rigidbody.rotation = rotation;
        }

        public override void Stop()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            OnStopped?.Invoke();
        }
    }
}
