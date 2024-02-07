using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Characters
{
    public class CombatantCharacter : Character, AttackReceiver
    {
        [SerializeField]
        protected HealthBar healthBar = null;
        [SerializeField]
        protected CombatModule combatModule = null;

        public virtual void ReceiveAttack(Attack attack)
        {
            ReceiveDamage(attack.damage);
        }

        public virtual void ReceiveDamage(float damage)
        {
            healthBar.InflictDamage(damage);
        }

        public virtual void Heal(float heal)
        {
            healthBar.Heal(heal);
        }

        public virtual void Attack()
        {
            combatModule.Attack();
        }
    }
}
