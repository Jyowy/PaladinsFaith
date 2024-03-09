using PaladinsFaith.Effects;
using PaladinsFaith.Math;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Combat
{
    public class Weapon : MonoBehaviour, AttackDeliverer
    {
        [SerializeField]
        private float baseDamage = 10f;

        [SerializeField]
        private Impact impactPrefab = null;

        public UnityEvent OnBlocked = null;

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
            bool collision = IntersectionCalculator.IntersectColliders(weaponCollider, other, out Vector3 impactPoint);
            if (!collision)
            {
                impactPoint = (transform.position + other.transform.position) * 0.5f;
            }
            return impactPoint;
        }

        private Attack CreateAttack(AttackData attackData, Vector3 impactPoint)
        {
            Attack attack = new Attack(wielder, impactPoint, attackData);
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

            AttackResult result = receiver.ReceiveAttack(attack);

            if (result == AttackResult.Defended)
            {
                OnBlocked?.Invoke();
            }
            else if (result != AttackResult.Invalid)
            {
                receiversDamaged.Add(receiver);
                if (impactPrefab != null)
                {
                    Impact.CreateAttackImpact(impactPrefab, attack);
                }
            }
        }

        public void CancelAttack()
        {
            currentAttackData = null;
            gameObject.SetActive(false);
        }
    }
}
