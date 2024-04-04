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

        private float holdingTime = 0f;

        public float GetBaseDamage() => baseDamage;

        public void SetWielder(GameObject wielder)
        {
            this.wielder = wielder;
        }

        public void SetAttackData(AttackData attackData)
        {
            holdingTime = 0f;
            currentAttackData = attackData;
        }

        public void HoldingAttack(float holdingTime)
        {
            this.holdingTime = holdingTime;
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
                Attack attack = CreateAttack(attackData, impactPoint, holdingTime);
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
                impactPoint = weaponCollider.ClosestPoint(other.transform.position);
            }
            return impactPoint;
        }

        private Attack CreateAttack(AttackData attackData, Vector3 impactPoint, float holdingTime)
        {
            Attack attack = new Attack(wielder, impactPoint, attackData, holdingTime);
            return attack;
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

        private bool CanDamage(AttackReceiver damageReceiver)
        {
            bool canDamage = damageReceiver != null
                && !receiversDamaged.Contains(damageReceiver);
            return canDamage;
        }

        public void CancelAttack()
        {
            currentAttackData = null;
            gameObject.SetActive(false);
        }
    }
}
