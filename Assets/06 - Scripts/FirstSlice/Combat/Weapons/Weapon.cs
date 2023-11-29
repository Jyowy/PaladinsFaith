using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class Weapon : MonoBehaviour, AttackDeliverer
    {
        [SerializeField]
        private float baseDamage = 10f;

        private readonly List<AttackReceiver> receiversDamaged = new List<AttackReceiver>();

        private GameObject wielder = null;
        private AttackData currentAttackData = null;

        public float GetBaseDamage() => baseDamage;

        public void SetWielder(GameObject wielder)
        {
            this.wielder = wielder;
        }

        public void SetAttackData(AttackData attackData)
        {
            currentAttackData = attackData;
        }

        private void OnDisable()
        {
            receiversDamaged.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            AttackReceiver receiver = other.GetComponent<AttackReceiver>();
            if (receiver != null
                && currentAttackData != null)
            {
                Attack attack = GetAttack();
                Attack(receiver, attack);
            }
        }

        private Attack GetAttack()
        {
            float damage = baseDamage * currentAttackData.damageMultiplier;
            Attack attack = new Attack(wielder, damage);
            return attack;
        }

        private bool CanDamage(AttackReceiver damageReceiver)
        {
            bool canDamage = damageReceiver != null
                && !receiversDamaged.Contains(damageReceiver);
            return canDamage;
        }

        public void Attack(AttackReceiver receiver, Attack attack)
        {
            if (!CanDamage(receiver))
            {
                return;
            }

            receiver.ReceiveAttack(attack);
            receiversDamaged.Add(receiver);
        }
    }
}
