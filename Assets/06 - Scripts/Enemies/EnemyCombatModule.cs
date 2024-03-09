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

        protected override void Attack()
        {
            IsAttacking = true;
            weapon.SetAttackData(attackData);
            weapon.OnBlocked?.AddListener(AttackBlocked);
            OnAttackTriggered?.Invoke(attackData);
        }

        private void AttackBlocked()
        {
            enemy.SimplePushBack();
            IsAttacking = false;
        }

        public override void AttackFinished()
        {
            IsAttacking = false;
        }

        protected override void AttackCancelled()
        {
            weapon.CancelAttack();
        }
    }
}
