using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstSlice.PlayerInput;

namespace FirstSlice.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputDataProvider inputDataProvider = null;
        [SerializeField]
        private PlayerDataProvider dataProvider = null;
        [SerializeField]
        private PlayerMoveModule moveModule = null;
        [SerializeField]
        private new PlayerCamera camera = null;

        private void FixedUpdate()
        {
            ProcessPhysicsData();
        }

        private void ProcessPhysicsData()
        {
            PlayerInputData playerInputData = inputDataProvider.GetPlayerInputData();
            PlayerData playerData = dataProvider.GetPlayerData();

            UpdateMoveMode(playerInputData.run);
            UpdateMove(playerInputData.movement);

            UpdateCameraRotation(playerInputData.cameraRotation);
        }

        private void UpdateMoveMode(bool run)
        {
            PlayerMoveModule.PlayerMoveMode moveMode = run
                ? PlayerMoveModule.PlayerMoveMode.Running
                : PlayerMoveModule.PlayerMoveMode.Walking;
            moveModule.SetMoveMode(moveMode);
        }

        private void UpdateMove(Vector2 move)
        {
            Vector3 worldMove = GetWorldDirectionFrom2DInput(move);
            moveModule.PlanarMove(worldMove);
        }

        private Vector3 GetWorldDirectionFrom2DInput(Vector2 input2D)
        {
            Vector3 planarDirection = new Vector3(input2D.x, 0f, input2D.y);
            Vector3 worldDirection = camera.GetVectorRelativeToView(planarDirection);
            Vector3 normalizedWorldDirection = worldDirection.NormalizedWithoutY();

            //Transform cameraTransform = camera.GetCameraTransform();
            //Vector3 right = cameraTransform.right.WithY(0f).normalized;
            //Vector3 forward = cameraTransform.forward.WithY(0f).normalized;
            //Vector3 worldDirection = right * input2D.x
            //    + forward * input2D.y;

            return normalizedWorldDirection;
        }

        private void UpdateCameraRotation(Vector2 cameraRotation)
        {
            float angle = cameraRotation.x;
            camera.Rotate(angle);

            float zoom = -cameraRotation.y;
            camera.Zoom(zoom);
        }
    }
}