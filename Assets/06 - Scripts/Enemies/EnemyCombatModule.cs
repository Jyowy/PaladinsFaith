using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Enemies
{
    public class EnemyCombatModule : CombatModule
    {
        [SerializeField]
        private Enemy enemy = null;

        [SerializeField]
        private AttackData attackData = null;

        [SerializeField]
        private Weapon weapon = null;

        private void Start()
        {
            weapon.SetWielder(enemy.gameObject);
        }

        public override void Attack()
        {
            if (IsAttacking)
            {
                return;
            }

            IsAttacking = true;
            weapon.SetAttackData(attackData);
            OnAttackTriggered?.Invoke(attackData);
        }

        public override void AttackFinished()
        {
            IsAttacking = false;
        }
    }
}
