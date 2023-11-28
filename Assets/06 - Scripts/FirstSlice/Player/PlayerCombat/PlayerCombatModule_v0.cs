using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class PlayerCombatModule_v0 : PlayerCombatModule
    {
        [SerializeField]
        private int maxSteps = 3;

        [ShowInInspector, ReadOnly]
        private bool defending = false;
        [ShowInInspector, ReadOnly]
        private bool attacking = false;
        [ShowInInspector, ReadOnly]
        private bool canAttack = true;

        [ShowInInspector, ReadOnly]
        private int currentAttackIndex = -1;
        [ShowInInspector, ReadOnly]
        private bool canTriggerNextAttack = false;
        [ShowInInspector, ReadOnly]
        private bool nextAttackRequested = false;

        public override void StartDefending()
        {
            bool wasDefending = defending;
            defending = true;

            if (wasDefending
                || attacking)
            {
                return;
            }

            OnDefenseStarted?.Invoke();
        }

        public override void StopDefending()
        {
            if (!defending)
            {
                return;
            }

            defending = false;
            OnDefenseFinished?.Invoke();
        }

        public override void Attack()
        {
            if (!canAttack)
            {
                return;
            }

            if (attacking)
            {
                nextAttackRequested = true;
                if (!canTriggerNextAttack)
                {
                    return;
                }
            }

            TriggerNextAttack();
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
            attacking = true;
            canTriggerNextAttack = false;
            nextAttackRequested = false;
            OnAttackTriggered?.Invoke(attackIndex);
        }

        public override void NextAttackAvailable()
        {
            canTriggerNextAttack = true;
            if (nextAttackRequested)
            {
                TriggerNextAttack();
            }
        }

        public override void CancelAttack()
        {
            if (!attacking)
            {
                return;
            }

            ResetAttacks();
            OnAttackCancelled?.Invoke();
        }

        private void ResetAttacks()
        {
            attacking = false;
            canAttack = true;
            currentAttackIndex = -1;
        }

        public override void AttackFinished()
        {
            if (!attacking)
            {
                return;
            }

            ResetAttacks();
            OnAttackFinished?.Invoke();
        }
    }
}
