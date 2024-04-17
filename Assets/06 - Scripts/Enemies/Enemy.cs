using PaladinsFaith.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat;
using PaladinsFaith.Player;
using PaladinsFaith.Combat.AlteredStates;

namespace PaladinsFaith.Enemies
{
    public class Enemy : CombatantCharacter
    {
        [SerializeField]
        private PlayerDetector detector = null;
        [SerializeField]
        private PlayerDetector range = null;

        private GameObject player = null;
        private bool playerInRange = false;

        protected override void Awake()
        {
            base.Awake();
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

        protected override void Update()
        {
            base.Update();
            if (player != null)
            {
                if (playerInRange)
                {
                    AttackPlayer();
                }
                else if (!combatModule.IsAttacking)
                {
                    MoveToPlayer();
                }
            }
        }

        private void MoveToPlayer()
        {
            if (!CanMove())
            {
                return;
            }

            Vector3 position = player.transform.position;
            moveModule.MoveTo(position);
        }

        protected override void OnDead()
        {
            base.OnDead();
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
                CombatMove combatMove = CombatMove.LightAttack;
                combatModule.TryToAttack(combatMove);
            }
        }

        protected override void KnockDownStarted()
        {
            base.KnockDownStarted();
            Debug.Log($"Enemy '{name}' KnockDownStarted");
        }

        protected override void KnockDownFinished()
        {
            base.KnockDownFinished();
            Debug.Log($"Enemy '{name}' KnockDownFinished");
        }

        protected override void StunStarted()
        {
            moveModule.Stop();
        }
    }
}