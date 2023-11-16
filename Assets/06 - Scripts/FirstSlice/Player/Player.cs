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

        private void Update()
        {
            ProcessData();
        }

        private void ProcessData()
        {
            PlayerInputData playerInputData = inputDataProvider.GetPlayerInputData();
            PlayerData playerData = dataProvider.GetPlayerData();

            UpdateMoveMode(playerInputData.run);
            UpdateMove(playerInputData.movement);
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
            moveModule.PlanarMove(move);
        }
    }
}