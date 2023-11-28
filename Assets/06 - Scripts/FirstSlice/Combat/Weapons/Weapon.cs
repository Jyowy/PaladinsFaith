using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class Weapon : MonoBehaviour, DamageDealer
    {
        [SerializeField]
        private float baseDamage = 10f;

        private readonly List<DamageReceiver> receiversDamaged = new List<DamageReceiver>();

        private void OnDisable()
        {
            receiversDamaged.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
            if (damageReceiver != null)
            {
                float damage = GetDamage();
                Damage(damageReceiver, damage);
            }
        }

        private bool CanDamage(DamageReceiver damageReceiver)
        {
            bool canDamage = damageReceiver != null
                && !receiversDamaged.Contains(damageReceiver);
            return canDamage;
        }

        public void Damage(DamageReceiver damageReceiver, float damage)
        {
            if (!CanDamage(damageReceiver))
            {
                return;
            }

            damageReceiver.ReceiveDamage(this, damage);
            receiversDamaged.Add(damageReceiver);
        }

        private float GetDamage()
        {
            float damage = baseDamage;
            return damage;
        }
    }
}
