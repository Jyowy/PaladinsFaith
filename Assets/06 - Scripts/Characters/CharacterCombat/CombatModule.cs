using PaladinsFaith.Characters;
using PaladinsFaith.Combat.Combos;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Combat
{
    public enum CombatMove
    {
        LightAttack,
        HeavyAttack
    }

    public abstract class CombatModule : MonoBehaviour
    {
        [SerializeField]
        protected ComboList comboList = null;
        [SerializeField]
        private float comboAvailableTime = 2f;
        [SerializeField]
        private float brokenDefenseRecovery = 2f;

        [SerializeField]
        private float staminaByAttack = 10f;

        public UnityEvent OnDefenseStarted = null;
        public UnityEvent OnDefenseFinished = null;
        public UnityEvent<AttackData> OnAttackTriggered = null;
        public UnityEvent OnAttackCancelled = null;
        public UnityEvent OnAttackFinished = null;
        public UnityEvent OnReleaseHoldingAttack = null;

        private ContinuousResource stamina = null;

        [ShowInInspector, ReadOnly]
        public bool IsDefending { get; protected set; } = false;
        [ShowInInspector, ReadOnly]
        public bool IsAttacking { get; protected set; } = false;
        [ShowInInspector, ReadOnly]
        public bool IsHoldingAttack { get; protected set; } = false;

        [ShowInInspector, ReadOnly]
        protected CharacterMoveType currentMoveType = CharacterMoveType.Walking;
        [ShowInInspector, ReadOnly]
        protected bool defenseBroken = false;
        [ShowInInspector, ReadOnly]
        protected bool attacksDisabled = false;

        private ComboManager comboManager = null;

        protected virtual void Awake()
        {
            comboManager = new ComboManager(comboList);
        }

        public void SetStamina(ContinuousResource stamina)
        {
            this.stamina = stamina;
        }

        public void SetMoveType(CharacterMoveType moveType)
        {
            Debug.Log($"CombatModule.SetMoveType: {moveType}");
            currentMoveType = moveType;
        }

        public void DisableAttacks()
        {
            attacksDisabled = true;
            Timers.StopTimer(gameObject, "AttackCancelled");
        }

        public void DisableAttacksForSeconds(float duration)
        {
            DisableAttacks();
            Timers.StartGameTimer(gameObject, "AttackCancelled", duration, EnableAttacks);
        }

        public void EnableAttacks()
        {
            Timers.StopTimer(gameObject, "AttackCancelled");
            attacksDisabled = false;
        }

        public void TryToAttack(CombatMove comboElement)
        {
            if (!CanAttack())
            {
                return;
            }

            Attack(comboElement);
        }

        protected virtual bool CanAttack()
        {
            bool can = !IsAttacking
                && !attacksDisabled;
            return can;
        }

        protected virtual void Attack(CombatMove comboElement) { }

        protected AttackData AddMoveToCombo(CombatMove comboElement)
        {
            AttackData attack = comboManager.AddCombo(comboElement);
            Timers.StartGameTimer(gameObject, "ComboAvailable", comboAvailableTime, BreakCombo);
            return attack;
        }

        protected void BreakCombo()
        {
            comboManager.BreakCombo();
        }

        protected void StartHoldingAttack()
        {
            IsHoldingAttack = true;
        }

        public virtual void HoldingAttack(CombatMove combatMove, float duration) { }

        public void StopHoldingAttack()
        {
            ReleaseHoldingAttack();
        }

        protected void ReleaseHoldingAttack()
        {
            Debug.Log($"Release Holding Attack");
            IsHoldingAttack = false;
            OnReleaseHoldingAttack?.Invoke();
        }

        public virtual void NextAttackAvailable() { }

        public virtual void AttackFinished()
        {
            IsAttacking = false;
        }

        public void CancelAttack()
        {
            if (!IsAttacking)
            {
                return;
            }

            AttackFinished();
            OnAttackCancelled?.Invoke();
            AttackCancelled();
        }

        protected virtual void AttackCancelled() { }

        public virtual void StartDefending()
        {
            if (IsDefending
                || IsAttacking
                || defenseBroken)
            {
                return;
            }

            IsDefending = true;
            stamina.AddAutomaticRegainBlocker();
            OnDefenseStarted?.Invoke();
        }

        public virtual void StopDefending()
        {
            if (!IsDefending)
            {
                return;
            }

            IsDefending = false;
            stamina.RemoveAutomaticRegainBlocker();
            OnDefenseFinished?.Invoke();
        }

        public virtual bool CanBlock(Attack attack)
        {
            bool can = IsDefending && attack.canBeBlocked;
            return can;
        }

        public void Block(Attack attack)
        {
            stamina.Consume(staminaByAttack);
            if (stamina.IsEmpty())
            {
                BreakDefense();
            }
        }

        public virtual void BreakDefense()
        {
            if (!IsDefending)
            {
                return;
            }

            stamina.AddAutomaticRegainBlocker();
            StopDefending();

            defenseBroken = true;
            Timers.StartGameTimer(gameObject, "DefenseBroken", brokenDefenseRecovery, DefenseRecovered);
        }

        private void DefenseRecovered()
        {
            defenseBroken = false;
            stamina.RemoveAutomaticRegainBlocker();
        }

        public void Stop()
        {
            DisableAttacks();
            StopDefending();
        }
    }
}
