using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Player
{
    public class PlayerDataProvider : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody playerRigidbody = null;

        [ShowInInspector, ReadOnly]
        private PlayerData playerData = null;

        private void Awake()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            playerData = new PlayerData();
        }

        public PlayerData GetPlayerData()
        {
            ProcessData();

            return playerData;
        }

        private void ProcessData()
        {
            Vector3 velocity = playerRigidbody.velocity;
            playerData.direction = velocity.normalized;
            playerData.speed = velocity.magnitude;
            playerData.isMoving = false;
        }
    }
}
