using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Player
{
    public class PlayerCameraWithRigidbody : PlayerCameraModule
    {
        [SerializeField]
        private new Rigidbody rigidbody = null;

        public override void RotateCamera(Vector2 rotation)
        {
            // TODO
        }
    }
}
