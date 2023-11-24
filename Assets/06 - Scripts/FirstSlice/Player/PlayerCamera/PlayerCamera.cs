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
        private float horizontalRotationSpeedFactor = 0.25f;
        [SerializeField]
        private float verticalRotationSpeedFactor = 0.25f;

        [SerializeField]
        private float rotationSpeedFactor = 0.25f;
        [SerializeField]
        private float zoomSpeedFactor = 0.1f;

        [SerializeField]
        private float height = 5f;

        [SerializeField]
        private float distance = 20f;

        [SerializeField]
        private bool invertedHorizontal = false;
        [SerializeField]
        private bool invertedVertical = false;
        [SerializeField]
        private float maxVerticalAngle = 60f;
        [SerializeField]
        private float minVerticalAngle = -10f;

        [SerializeField]
        private Transform target = null;

        [SerializeField]
        private Transform colliderToTarget = null;
        [SerializeField]
        private float colliderToTargetWidth = 5f;

        [ShowInInspector]
        private float verticalAngle = 22.5f;
        [ShowInInspector]
        private float horizontalAngle = 0f;

        private float rotation = 0f;
        private float currentDistance = 0f;

        private Vector3 startDirection = Vector3.zero;

        private void Awake()
        {
            currentDistance = defaultDistance;
            startDirection = -target.forward;
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
            Quaternion offsetRotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0f);
            Vector3 offsetDirection = offsetRotation * startDirection;
            Vector3 offsetPosition = offsetDirection * distance;
            return offsetPosition;
        }

        private Vector3 GetOffsetRelativeToTarget_FixedHeight()
        {
            Vector3 offsetPosition = Vector3.zero;
            offsetPosition.y = height;

            float planarDistance = Mathf.Sqrt(currentDistance * currentDistance - height * height);
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
            if (invertedHorizontal)
            {
                angle = -angle;
            }

            angle *= horizontalRotationSpeedFactor;
            rotation = Mathf.Repeat(rotation + angle, 360f);

            horizontalAngle = Mathf.Repeat(horizontalAngle + angle, 360f);
        }

        public void Zoom(float zoom)
        {
            if (invertedVertical)
            {
                zoom = -zoom;
            }

            float angle = -zoom * verticalRotationSpeedFactor;

            zoom *= zoomSpeedFactor;
            currentDistance = Mathf.Clamp(currentDistance + zoom, minDistance, maxDistance);

            verticalAngle = Mathf.Clamp(verticalAngle + angle, minVerticalAngle, maxVerticalAngle);
            //verticalAngle = Mathf.Repeat(verticalAngle + angle, 360f);
        }
    }
}
