using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Characters;
using PaladinsFaith.Spells;
using UnityEngine.Events;

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

        public UnityEvent OnDeath = null;

        protected override void Awake()
        {
            base.Awake();
            healthBar.Initialize(OnDead);
            combatModule.SetStamina(stamina);
            stamina.Fill();
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

        public virtual AttackResult ReceiveAttack(Attack attack)
        {
            AttackResult result = AttackResult.Success;

            if (combatModule.CanBlock(attack))
            {
                combatModule.Block(attack);
                result = AttackResult.Defended;
            }
            else
            {
                attack.effectsOnImpact.ApplyOnImpact(attack.attacker, gameObject, attack.impactPoint);
            }

            return result;
        }

        public virtual void ReceiveDamage(float damage)
        {
            healthBar.ReceiveDamage(damage);
        }

        public virtual void Heal(float heal)
        {
            healthBar.Heal(heal);
        }

        protected virtual void OnDead()
        {
            OnDeath?.Invoke();
        }

        public virtual void Attack()
        {
            combatModule.TryToAttack();
        }

        protected virtual void OnPushed()
        {
            combatModule.CancelAttack();
        }
    }
}
