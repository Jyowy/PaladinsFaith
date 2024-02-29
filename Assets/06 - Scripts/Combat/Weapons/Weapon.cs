using PaladinsFaith.Effects;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat
{
    public class Weapon : MonoBehaviour, AttackDeliverer
    {
        [SerializeField]
        private float baseDamage = 10f;

        private readonly List<AttackReceiver> receiversDamaged = new List<AttackReceiver>();

        private GameObject wielder = null;
        private AttackData currentAttackData = null;
        private Collider weaponCollider = null;

        public float GetBaseDamage() => baseDamage;

        public void SetWielder(GameObject wielder)
        {
            this.wielder = wielder;
        }

        public void SetAttackData(AttackData attackData)
        {
            currentAttackData = attackData;
        }

        private void Awake()
        {
            weaponCollider = GetComponent<Collider>();
        }

        private void OnDisable()
        {
            receiversDamaged.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"OnTriggerEnter");

            if (other.TryGetComponent(out AttackReceiver receiver)
                && other.gameObject.layer != gameObject.layer)
            {
                AttackData attackData = GetCurrentAttackData();
                Vector3 impactPoint = GetImpactPointWith(other);
                Attack attack = CreateAttack(attackData, impactPoint);
                Attack(receiver, attack);
            }
        }

        private AttackData GetCurrentAttackData()
        {
            return currentAttackData;
        }

        private Vector3 GetImpactPointWith(Collider other)
        {
            Vector3 impactPoint = Vector3.zero;
            // TODO
            return impactPoint;
        }

        private Attack CreateAttack(AttackData attackData, Vector3 impactPoint)
        {
            EffectSet effectsOnImpact = attackData.effectsOnImpact;
            Attack attack = new Attack(wielder, effectsOnImpact, impactPoint);
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
