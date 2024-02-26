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

        public UnityEvent OnDefenseStarted = null;
        public UnityEvent OnDefenseFinished = null;
        public UnityEvent<AttackData> OnAttackTriggered = null;
        public UnityEvent OnAttackCancelled = null;
        public UnityEvent OnAttackFinished = null;

        private ContinuousResource stamina = null;

        public bool IsDefending { get; protected set; } = false;
        public bool IsAttacking { get; protected set; } = false;

        public void SetStamina(ContinuousResource stamina)
        {
            this.stamina = stamina;
        }

        public virtual void Attack() { }

        public virtual void NextAttackAvailable() { }

        public virtual void AttackFinished()
        {
            IsAttacking = false;
        }

        public virtual void CancelAttack() { }

        public virtual void StartDefending()
        {
            if (IsDefending
                || IsAttacking)
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
    }
}
