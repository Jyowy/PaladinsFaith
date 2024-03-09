using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Player
{
    [RequireComponent(typeof(Collider))]
    public class PlayerDetector : MonoBehaviour
    {
        public UnityEvent<GameObject> OnPlayerFound = null;
        public UnityEvent OnPlayerLost = null;

        private Player player = null;

        private void OnTriggerEnter(Collider other)
        {
            PlayerFound(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerLost();
        }

        private void PlayerDead()
        {
            PlayerLost();
        }

        private void PlayerFound(GameObject playerGameObject)
        {
            player = playerGameObject.GetComponent<Player>();
            player.OnDeath?.AddListener(PlayerDead);
            OnPlayerFound?.Invoke(playerGameObject);
        }

        private void PlayerLost()
        {
            player = null;
            OnPlayerLost?.Invoke();
        }
    }
}
