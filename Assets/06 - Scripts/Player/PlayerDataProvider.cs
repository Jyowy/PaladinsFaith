using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Player
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

        public PlayerData UpdateAndGetPlayerData(Player player)
        {
            UpdateData(player);
            return playerData;
        }

        private void UpdateData(Player player)
        {
            Vector3 velocity = playerRigidbody.velocity;
            playerData.direction = velocity.normalized;
            playerData.speed = velocity.magnitude;
            playerData.isMoving = playerData.speed > 0f;
        }
    }
}
