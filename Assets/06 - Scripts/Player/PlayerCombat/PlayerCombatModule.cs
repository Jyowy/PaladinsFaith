using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat;
using PaladinsFaith.Combat.Combos;

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
        protected AttackData currentAttack = null;
        [ShowInInspector, ReadOnly]
        private CombatMove currentComboMove = CombatMove.LightAttack;
        [ShowInInspector, ReadOnly]
        private bool CanContinueAttacking = false;
        [ShowInInspector, ReadOnly]
        private bool attackRequested = false;
        [ShowInInspector, ReadOnly]
        private CombatMove combatMoveRequested = CombatMove.LightAttack;

        private void Start()
        {
            weapon.SetWielder(player.gameObject);
            shield.SetWielder(player.gameObject);
        }

        protected override void Attack(CombatMove combatMove)
        {
            if (IsDefending)
            {
                StopDefending();
            }

            if (IsAttacking)
            {
                RequestAttack(combatMove);
                if (!CanContinueAttacking)
                {
                    return;
                }
            }

            TriggerComboAttack(combatMove);
        }

        private void RequestAttack(CombatMove combatMove)
        {
            Debug.Log($"Request Attack {combatMove}");
            attackRequested = true;
            combatMoveRequested = combatMove;
        }

        private void ClearRequestedAttack()
        {
            attackRequested = false;
        }

        private void TriggerComboAttack(CombatMove combatMove)
        {
            ClearRequestedAttack();
            currentComboMove = combatMove;
            AttackData attack = AddMoveToCombo(combatMove);
            Debug.Log($"Trigger Combo Attack {combatMove} => {attack.attackName}");
            TriggerAttack(attack);
        }

        protected override bool CanAttack()
        {
            bool can = !attacksDisabled && canAttack;
            return can;
        }

        private void TriggerAttack(AttackData attackData)
        {
            currentAttack = attackData;
            IsAttacking = true;
            CanContinueAttacking = false;
            IsHoldingAttack = attackData.canBeCharged;
            weapon.SetAttackData(attackData);
            OnAttackTriggered?.Invoke(attackData);
        }

        public override void HoldingAttack(CombatMove combatMove, float holdingTime)
        {
            if (!IsHoldingAttack
                || currentComboMove != combatMove)
            {
                return;
            }

            Debug.Log($"HoldingAttack {holdingTime}");
            weapon.HoldingAttack(holdingTime);
            if (holdingTime >= currentAttack.maxChargeTime)
            {
                ReleaseHoldingAttack();
            }
        }

        public override void NextAttackAvailable()
        {
            CanContinueAttacking = true;
            if (attackRequested)
            {
                TriggerRequestedAttack();
            }
        }

        private void TriggerRequestedAttack()
        {
            TriggerComboAttack(combatMoveRequested);
        }

        protected override void AttackCancelled()
        {
            ResetAttacks();
            OnAttackCancelled?.Invoke();
        }

        private void ResetAttacks()
        {
            IsAttacking = false;
            IsHoldingAttack = false;
            currentAttack = null;
            BreakCombo();
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

        protected void Update()
        {
            UpdateShieldCharging();
        }

        private void UpdateShieldCharging()
        {
            bool isCharging = IsCharging();
            shield.SetCharging(isCharging);
        }

        [Button]
        private bool IsCharging()
        {
            bool fastMove = currentMoveType == Characters.CharacterMoveType.Running
                || currentMoveType == Characters.CharacterMoveType.Dashing;
            return IsDefending
                && fastMove;
        }
    }
}