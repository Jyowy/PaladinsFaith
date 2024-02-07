using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Player
{
    public abstract class PlayerCameraModule : MonoBehaviour
    {
        [SerializeField]
        private GameObject cameraRoot = null;

        public abstract void RotateCamera(Vector2 rotation);

        public Transform GetCameraTransform() => cameraRoot.transform;
    }
}
