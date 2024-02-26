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
        public void ReceiveAttack(Attack attack)
        {
            if (destroyed)
            {
                return;
            }

            attack.effectsOnImpact.ApplyOnImpact(attack.attacker, gameObject);
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
