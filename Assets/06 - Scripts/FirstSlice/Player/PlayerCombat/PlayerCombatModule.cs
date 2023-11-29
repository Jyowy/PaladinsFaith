using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice
{
    public abstract class PlayerCombatModule : MonoBehaviour
    {
        [SerializeField]
        protected Player.Player player;

        [SerializeField]
        protected List<AttackData> attacks = null;

        [SerializeField]
        protected Weapon weapon = null;
        [SerializeField]
        protected Shield shield = null;

        public UnityEvent OnDefenseStarted = null;
        public UnityEvent OnDefenseFinished = null;
        public UnityEvent<AttackData> OnAttackTriggered = null;
        public UnityEvent OnAttackCancelled = null;
        public UnityEvent OnAttackFinished = null;

        [ShowInInspector, ReadOnly]
        protected int currentAttackIndex = -1;


        public AttackData GetCurrentAttackData() => currentAttackIndex >= 0
            ? attacks[currentAttackIndex]
            : null;

        [Button]
        public abstract void StartDefending();

        [Button]
        public abstract void StopDefending();

        public abstract bool IsDefending();

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
