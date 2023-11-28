using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class Destructible : MonoBehaviour, DamageReceiver
    {
        [SerializeField]
        private bool oneHit = true;
        [SerializeField, HideIf("oneHit")]
        private HealthBar healthBar = new HealthBar();

        [SerializeField]
        private Shield shield = null;

        [ShowInInspector, ReadOnly]
        private bool destroyed = false;

        private void Awake()
        {
            healthBar.Initialize(Destroy);
        }

        [Button]
        public void ReceiveDamage(DamageDealer damageDealer, float damage)
        {
            if (destroyed
                || (shield != null && shield.HasBlocked(damageDealer)))
            {
                return;
            }

            if (oneHit)
            {
                Destroy();
            }
            else
            {
                healthBar.ReceiveDamage(damageDealer, damage);
            }
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
