using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Characters
{
    public abstract class CharacterMoveRigidbody : CharacterMoveModule
    {
        [SerializeField]
        protected new Rigidbody rigidbody = null;
        [SerializeField]
        private bool onlyApplyPhysicsWhenMoving = false;

        [SerializeField]
        protected bool glueToFloor = false;
        [SerializeField, FoldoutGroup("Floor Detection", VisibleIf = "glueToFloor")]
        private LayerMask layerMask = 0;
        [SerializeField, FoldoutGroup("Floor Detection")]
        private float minDistanceToGlue = 0.0f;
        [SerializeField, FoldoutGroup("Floor Detection")]
        private float floorDetectionHeightOffset = 0.5f;
        [SerializeField, FoldoutGroup("Floor Detection")]
        private float glueFactor = 0.5f;

        [ShowInInspector, ReadOnly]
        protected bool IsStopped { get; private set; } = false;

        protected override void RotateToForward()
        {
            Vector3 direction = rigidbody.velocity.normalized;
            Vector3 normalizedDirecition = direction.WithY(0f).normalized;
            Quaternion rotation = Quaternion.LookRotation(normalizedDirecition, Vector3.up);
            rigidbody.rotation = rotation;
        }

        protected void MoveStarted()
        {
            IsStopped = false;
            if (onlyApplyPhysicsWhenMoving)
            {
                ActivatePhysics();
            }
        }

        public override void Stop()
        {
            IsStopped = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            OnStopped?.Invoke();

            if (onlyApplyPhysicsWhenMoving)
            {
                DeactivatePhysics();
            }
        }

        public bool ArePhysicsEnabled() => rigidbody.isKinematic;

        public void ActivatePhysics()
        {
            rigidbody.isKinematic = false;
        }

        public void DeactivatePhysics()
        {
            rigidbody.isKinematic = true;
        }

        protected void GlueToFloor()
        {
            float floorDistance = GetFloorDistance();
            if (floorDistance > minDistanceToGlue)
            {
                rigidbody.position += Vector3.down * (floorDistance * glueFactor);
            }
        }

        private float GetFloorDistance(Vector3 offset = new Vector3())
        {
            Vector3 origin = rigidbody.position + offset + Vector3.up * floorDetectionHeightOffset;
            Vector3 direction = Vector3.down;
            float maxDistance = 100f;

            bool hit = Physics.Raycast(origin, direction, out var hitInfo, maxDistance, layerMask);
            float distance = hit ? hitInfo.distance - floorDetectionHeightOffset : 0f;
            return distance;
        }

        protected override void PushStarted(Vector3 direction, float power)
        {
            ActivatePhysics();
        }

        protected override void PushFinished()
        {
            base.PushFinished();
            if (IsStopped)
            {
                DeactivatePhysics();
            }
        }
    }
}
