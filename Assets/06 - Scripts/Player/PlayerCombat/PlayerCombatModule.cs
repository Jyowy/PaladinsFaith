using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Player
{
    public class PlayerCombatModule : CombatModule
    {
        [SerializeField]
        protected Player player = null;

        [SerializeField]
        protected Weapon weapon = null;
        [SerializeField]
        protected Shield shield = null;

        [SerializeField]
        private int maxSteps = 3;

        [ShowInInspector, ReadOnly]
        private bool canAttack = true;

        [ShowInInspector, ReadOnly]
        protected int currentAttackIndex = -1;
        [ShowInInspector, ReadOnly]
        private bool canTriggerNextAttack = false;
        [ShowInInspector, ReadOnly]
        private bool nextAttackRequested = false;

        private void Start()
        {
            weapon.SetWielder(player.gameObject);
            shield.SetWielder(player.gameObject);
        }

        protected override void Attack()
        {
            if (IsDefending)
            {
                StopDefending();
            }

            if (IsAttacking)
            {
                nextAttackRequested = true;
                if (!canTriggerNextAttack)
                {
                    return;
                }
            }

            TriggerNextAttack();
        }

        protected override bool CanAttack()
        {
            bool can = !attacksCancelled && canAttack;
            return can;
        }

        [Button]
        private void TriggerNextAttack()
        {
            int nextAttack = Mathf.Min(currentAttackIndex + 1, maxSteps - 1);
            TriggerAttack(nextAttack);
        }

        private void TriggerAttack(int attackIndex)
        {
            canAttack = attackIndex < maxSteps - 1;

            currentAttackIndex = attackIndex;
            IsAttacking = true;
            canTriggerNextAttack = false;
            nextAttackRequested = false;

            AttackData attackData = GetAttackData(attackIndex);
            weapon.SetAttackData(attackData);
            OnAttackTriggered?.Invoke(attackData);
        }

        private AttackData GetAttackData(int attackIndex)
        {
            AttackData attack = attacks[attackIndex];
            return attack;
        }

        public override void NextAttackAvailable()
        {
            canTriggerNextAttack = true;
            if (nextAttackRequested)
            {
                TriggerNextAttack();
            }
        }

        protected override void AttackCancelled()
        {
            ResetAttacks();
            OnAttackCancelled?.Invoke();
        }

        private void ResetAttacks()
        {
            IsAttacking = false;
            canAttack = true;
            currentAttackIndex = -1;
        }

        public override void AttackFinished()
        {
            if (!IsAttacking)
            {
                return;
            }

            ResetAttacks();
            OnAttackFinished?.Invoke();
        }

        public override bool CanBlock(Attack attack)
        {
            bool can = base.CanBlock(attack)
                && shield.CanBlock(attack);
            return can;
        }
    }
}