using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody cameraRigidbody = null;

        [SerializeField]
        private float defaultDistance = 7.5f;
        [SerializeField]
        private float minDistance = 3f;
        [SerializeField]
        private float maxDistance = 10f;

        [SerializeField]
        private float rotationSpeedFactor = 0.25f;
        [SerializeField]
        private float zoomSpeedFactor = 0.1f;

        [SerializeField]
        private float height = 5f;

        [SerializeField]
        private Transform target = null;

        [SerializeField]
        private Transform colliderToTarget = null;
        [SerializeField]
        private float colliderToTargetWidth = 5f;

        private float rotation = 0f;
        private float distance = 0f;

        private void Awake()
        {
            distance = defaultDistance;
        }

        public Vector3 GetVectorRelativeToView(Vector3 vector)
        {
            Transform cameraTransform = cameraRigidbody.transform;
            Vector3 relativeVector = cameraTransform.right * vector.x
                + cameraTransform.up * vector.y
                + cameraTransform.forward  * vector.z;
            return relativeVector;
        }

        private void FixedUpdate()
        {
            UpdatePosition();
        }

        [Button]
        private void UpdatePosition()
        {
            Vector3 desiredPosition = GetDesiredPosition();
            MoveTo(desiredPosition);
            UpdateColliderToTarget();
        }

        private Vector3 GetDesiredPosition()
        {
            Vector3 targetPosition = GetTargetPosition();
            Vector3 offset = GetOffsetRelativeToTarget();
            Vector3 desiredPosition = targetPosition + offset;
            return desiredPosition;
        }

        private Vector3 GetTargetPosition()
        {
            Vector3 targetPosition = target.position;
            return targetPosition;
        }

        private Vector3 GetOffsetRelativeToTarget()
        {
            Vector3 offsetPosition = Vector3.zero;
            offsetPosition.y = height;

            float planarDistance = Mathf.Sqrt(distance * distance - height * height);
            Vector3 direction = -Vector3.forward;
            direction = Quaternion.Euler(0f, rotation, 0f) * direction;

            offsetPosition += direction * planarDistance;

            return offsetPosition;
        }

        private void MoveTo(Vector3 position)
        {
            cameraRigidbody.Move(position, Quaternion.identity);
        }

        private void UpdateColliderToTarget()
        {
            Vector3 cameraPosition = cameraRigidbody.transform.position;
            Vector3 targetPosition = target.position;

            Vector3 cameraToTargetVector = targetPosition - cameraPosition;
            Vector3 position = cameraPosition + cameraToTargetVector * 0.5f;
            float distance = cameraToTargetVector.magnitude;
            Vector3 scale = new Vector3(colliderToTargetWidth, distance, colliderToTargetWidth);

            Quaternion rotation = cameraRigidbody.transform.rotation;
            Vector3 euler = rotation.eulerAngles;
            euler.x += 90f;
            rotation.eulerAngles = euler;

            colliderToTarget.localScale = scale;
            colliderToTarget.SetPositionAndRotation(position, rotation);
        }

        public void Rotate(float angle)
        {
            angle *= rotationSpeedFactor;
            rotation = Mathf.Repeat(rotation + angle, 360f);
        }

        public void Zoom(float zoom)
        {
            zoom *= zoomSpeedFactor;
            distance = Mathf.Clamp(distance + zoom, minDistance, maxDistance);
        }
    }
}
