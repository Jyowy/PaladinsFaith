using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith
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

        public bool IsDefending { get; protected set; } = false;
        public bool IsAttacking { get; protected set; } = false;

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
            OnDefenseStarted?.Invoke();
        }

        public virtual void StopDefending()
        {
            if (!IsDefending)
            {
                return;
            }

            IsDefending = false;
            OnDefenseFinished?.Invoke();
        }
    }
}
