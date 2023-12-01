using FirstSlice.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Enemies
{
    public class Enemy : CombatantCharacter
    {
        [SerializeField]
        private PlayerDetector detector = null;
        [SerializeField]
        private PlayerDetector range = null;

        private GameObject player = null;
        private bool playerInRange = false;

        private void Awake()
        {
            healthBar.Initialize(OnDead);

            detector.OnPlayerFound?.AddListener(PlayerFound);
            detector.OnPlayerLost?.AddListener(PlayerLost);

            range.OnPlayerFound?.AddListener(PlayerInRange);
            range.OnPlayerLost?.AddListener(PlayerOutOfRange);
        }

        private void OnDestroy()
        {
            if (detector != null)
            {
                detector.OnPlayerFound?.RemoveListener(PlayerFound);
                detector.OnPlayerLost?.RemoveListener(PlayerLost);
            }

            if (range != null)
            {
                range.OnPlayerFound?.RemoveListener(PlayerInRange);
                range.OnPlayerLost?.RemoveListener(PlayerOutOfRange);
            }
        }

        private void PlayerFound(GameObject player)
        {
            this.player = player;
        }

        private void PlayerLost()
        {
            player = null;
            moveModule.Stop();
        }

        private void Update()
        {
            if (player != null)
            {
                if (playerInRange)
                {
                    AttackPlayer();
                }
                else
                {
                    MoveToPlayer();
                }
            }
        }

        private void MoveToPlayer()
        {
            Vector3 position = player.transform.position;
            moveModule.MoveTo(position);
        }

        private void OnDead()
        {
            Destroy(gameObject);
        }

        private void PlayerInRange(GameObject _)
        {
            playerInRange = true;
            moveModule.Stop();
        }

        private void PlayerOutOfRange()
        {
            playerInRange = false;
        }

        private void AttackPlayer()
        {
            if (!combatModule.IsAttacking)
            {
                moveModule.LookAt(player.transform.position);
                combatModule.Attack();
            }
        }
    }
}
