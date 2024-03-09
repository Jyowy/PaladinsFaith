using PaladinsFaith.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Combat
{
    public abstract class CombatModule : MonoBehaviour
    {
        [SerializeField]
        protected List<AttackData> attacks = new List<AttackData>();
        [SerializeField]
        private float brokenDefenseRecovery = 2f;

        [SerializeField]
        private float staminaByAttack = 10f;

        public UnityEvent OnDefenseStarted = null;
        public UnityEvent OnDefenseFinished = null;
        public UnityEvent<AttackData> OnAttackTriggered = null;
        public UnityEvent OnAttackCancelled = null;
        public UnityEvent OnAttackFinished = null;

        private ContinuousResource stamina = null;

        public bool IsDefending { get; protected set; } = false;
        public bool IsAttacking { get; protected set; } = false;

        protected bool defenseBroken = false;
        protected bool attacksCancelled = false;

        public void SetStamina(ContinuousResource stamina)
        {
            this.stamina = stamina;
        }

        public void CancelAttacks(float duration)
        {
            attacksCancelled = true;
            Timers.StartGameTimer(gameObject, "AttackCancelled", duration, CancelAttacksFinished);
        }

        private void CancelAttacksFinished()
        {
            attacksCancelled = false;
        }

        public void TryToAttack()
        {
            if (!CanAttack())
            {
                return;
            }

            Attack();
        }

        protected virtual bool CanAttack()
        {
            bool can = !IsAttacking
                && !attacksCancelled;
            return can;
        }

        protected virtual void Attack() { }

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
            stamina.Deplete(staminaByAttack);
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
    }
}
