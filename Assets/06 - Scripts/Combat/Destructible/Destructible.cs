using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public class Destructible : MonoBehaviour, AttackReceiver, DamageReceiver
    {
        [SerializeField]
        private HealthBar healthBar = new HealthBar();

        [ShowInInspector, ReadOnly]
        private bool destroyed = false;

        private void Awake()
        {
            healthBar.Initialize(Destroy);
        }

        [Button]
        public AttackResult ReceiveAttack(Attack attack)
        {
            AttackResult attackResult = AttackResult.Success;
            if (destroyed)
            {
                attackResult = AttackResult.Invalid;
            }
            else
            {
                attack.effectsOnImpact.ApplyOnImpact(attack.attacker, gameObject, attack.impactPoint, attack.multiplier);
            }

            return attackResult;
        }

        public void ReceiveDamage(float damage)
        {
            if (destroyed)
            {
                return;
            }

            healthBar.ReceiveDamage(damage);
        }

        public void Destroy()
        {
            if (destroyed)
            {
                return;
            }

            destroyed = true;
            Destroy(gameObject);
        }
    }
}
