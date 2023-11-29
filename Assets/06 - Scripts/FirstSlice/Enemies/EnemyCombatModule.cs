using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice
{
    public class EnemyCombatModule : MonoBehaviour
    {
        [SerializeField]
        private Enemy enemy = null;

        [SerializeField]
        private AttackData attackData = null;

        [SerializeField]
        private Weapon weapon = null;

        public UnityEvent<AttackData> OnAttackTriggered = null;

        public bool IsAttacking { get; private set; } = false;

        private void Start()
        {
            weapon.SetWielder(enemy.gameObject);
        }

        public void Attack()
        {
            if (IsAttacking)
            {
                return;
            }

            IsAttacking = true;
            weapon.SetAttackData(attackData);
            OnAttackTriggered?.Invoke(attackData);
        }

        public void AttackFinished()
        {
            IsAttacking = false;
        }
    }
}
