using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Characters;
using PaladinsFaith.Spells;

namespace PaladinsFaith.Combat
{
    public class CombatantCharacter : Character, AttackReceiver, DamageReceiver
    {
        [SerializeField]
        protected HealthBar healthBar = null;
        [SerializeField]
        protected ContinuousResource mana = new ContinuousResource(100f);
        [SerializeField]
        protected CombatModule combatModule = null;
        [SerializeField]
        protected SpellModule spellModule = null;

        protected override void Awake()
        {
            base.Awake();
            combatModule.SetStamina(stamina);
            if (spellModule != null )
            {
                spellModule.SetMana(mana);
            }

            moveModule.OnPushed?.AddListener(OnPushed);
        }

        protected override void Update()
        {
            base.Update();
            float dt = Time.deltaTime;
            mana.Update(dt);
        }

        public virtual void ReceiveAttack(Attack attack)
        {
            attack.effectsOnImpact.ApplyOnImpact(attack.attacker, gameObject);
        }

        public virtual void ReceiveDamage(float damage)
        {
            healthBar.ReceiveDamage(damage);
        }

        public virtual void Heal(float heal)
        {
            healthBar.Heal(heal);
        }

        public virtual void Attack()
        {
            combatModule.Attack();
        }

        protected virtual void OnPushed()
        {
            combatModule.CancelAttack();
        }
    }
}
