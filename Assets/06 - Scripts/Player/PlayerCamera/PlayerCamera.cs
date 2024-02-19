using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PaladinsFaith.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform target = null;

        [SerializeField]
        private Transform cameraTransform = null;
        [SerializeField]
        private Rigidbody cameraRigidbody = null;

        [SerializeField]
        private float minDistance = 2.5f;
        [SerializeField]
        private float maxDistance = 25f;

        [SerializeField]
        private float horizontalSpeed = 1.25f;
        [SerializeField]
        private float verticalSpeed = 1.25f;

        [SerializeField]
        private LayerMask scenaryLayers = 0;
        [SerializeField]
        private float distanceFromScenary = 0.25f;
        [SerializeField]
        private float radiusToCheckScenary = 0.5f;

        [SerializeField]
        private bool invertedHorizontal = false;
        [SerializeField]
        private bool invertedVertical = false;
        [SerializeField]
        private float maxVerticalAngle = 60f;
        [SerializeField]
        private float minVerticalAngle = -10f;

        [SerializeField]
        private Transform colliderToTarget = null;
        [SerializeField]
        private float colliderToTargetWidth = 5f;

        [ShowInInspector]
        private float verticalAngle = 22.5f;
        [ShowInInspector]
        private float horizontalAngle = 0f;

        [SerializeField]
        private float moveFactor = 0.1f;

        private Vector3 startDirection = Vector3.zero;

        private void Awake()
        {
            startDirection = -target.forward;
        }

        private void Start()
        {
            ForcePosition();
        }

        private void ForcePosition()
        {
            Vector3 desiredPosition = GetDesiredPosition();
            cameraTransform.position = desiredPosition;
        }

        public Vector3 GetVectorRelativeToView(Vector3 vector)
        {
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
            Vector3 offsetPosition = offsetDirection * maxDistance;
            return offsetPosition;
        }

        private void MoveTo(Vector3 position)
        {
            Vector3 bestPosition = GetBestPosition(position);
            Vector3 halfwayPosition = cameraTransform.position * (1f - moveFactor) + bestPosition * moveFactor;
            cameraRigidbody.Move(halfwayPosition, Quaternion.identity);
        }

        private Vector3 GetBestPosition(Vector3 desiredPosition)
        {
            Vector3 targetPosition = GetTargetPosition();
            Vector3 vector = desiredPosition - targetPosition;
            Vector3 direction = vector.normalized;

            float distance = GetDistanceToScenary(targetPosition, desiredPosition);
            Vector3 bestPosition = targetPosition + direction * distance;

            return bestPosition;
        }

        private float GetDistanceToScenary(Vector3 targetPosition, Vector3 desiredPosition)
        {
            float distance = maxDistance;

            Vector3 direction = (desiredPosition - targetPosition).normalized;
            Ray ray = new Ray(targetPosition, direction);
            if (Physics.SphereCast(ray, radiusToCheckScenary, out RaycastHit info, maxDistance, scenaryLayers))
            {
                distance = info.distance - distanceFromScenary;
            }

            return distance;
        }

        private void UpdateColliderToTarget()
        {
            Vector3 cameraPosition = cameraTransform.position;
            Vector3 targetPosition = target.position;

            Vector3 cameraToTargetVector = targetPosition - cameraPosition;
            Vector3 position = cameraPosition + cameraToTargetVector * 0.5f;
            float distance = cameraToTargetVector.magnitude;
            Vector3 scale = new Vector3(colliderToTargetWidth, distance, colliderToTargetWidth);
            colliderToTarget.localScale = scale;

            float verticalAngle = 90f;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles += new Vector3(verticalAngle, 0f, 0f);
            colliderToTarget.SetPositionAndRotation(position, rotation);
        }

        public void HorizontalRotation(float angle)
        {
            if (invertedHorizontal)
            {
                angle = -angle;
            }

            angle *= horizontalSpeed;

            horizontalAngle = Mathf.Repeat(horizontalAngle + angle, 360f);
        }

        public void VerticalRotation(float angle)
        {
            if (invertedVertical)
            {
                angle = -angle;
            }

            angle *= verticalSpeed;

            verticalAngle = Mathf.Clamp(verticalAngle + angle, minVerticalAngle, maxVerticalAngle);
        }
    }
}