using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice
{
    public abstract class PlayerCombatModule : MonoBehaviour
    {
        public UnityEvent OnDefenseStarted = null;
        public UnityEvent OnDefenseFinished = null;
        public UnityEvent<int> OnAttackTriggered = null;
        public UnityEvent OnAttackCancelled = null;
        public UnityEvent OnAttackFinished = null;

        [Button]
        public abstract void StartDefending();

        [Button]
        public abstract void StopDefending();

        [Button]
        public abstract void Attack();

        [Button]
        public abstract void NextAttackAvailable();

        [Button]
        public abstract void CancelAttack();

        [Button]
        public abstract void AttackFinished();
    }
}
