using PaladinsFaith.Math;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat
{
    public class Shield : MonoBehaviour
    {
        [SerializeField]
        private float blockableAngle = 120f;
        [SerializeField]
        private AttackData chargeAttack = null;
        [SerializeField]
        private float chargeImpactCooldown = 0.25f;

        [ShowInInspector, ReadOnly]
        private GameObject wielder = null;
        [ShowInInspector, ReadOnly]
        private readonly List<AttackDeliverer> dealersBlocked = new List<AttackDeliverer>();

        private Collider shieldCollider = null;
        [ShowInInspector, ReadOnly]
        private bool isCharging = false;

        private float holdingTime = 0f;

        private readonly Dictionary<AttackReceiver, float> chargedTargets = new Dictionary<AttackReceiver, float>();

        private void Awake()
        {
            shieldCollider = GetComponent<Collider>();
        }

        public void SetCharging(bool charging)
        {
            if (isCharging == charging)
            {
                return;
            }

            chargedTargets.Clear();
            this.isCharging = charging;
        }

        public void SetWielder(GameObject wielder)
        {
            this.wielder = wielder;
        }

        private void OnDisable()
        {
            dealersBlocked.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AttackDeliverer damageDealer))
            {
                BlockDamageDealer(damageDealer);
            }
            else if (other.TryGetComponent(out AttackReceiver attackReceiver)
                && isCharging)
            {
                Charge(attackReceiver, other);
            }
        }

        public bool CanBlock(Attack attack)
        {
            Vector3 attackerPosition = attack.attacker.transform.position;
            Vector3 wielderPosition = wielder.transform.position;

            Vector3 shieldForward = transform.forward;
            Vector3 wielderToAttacker = attackerPosition - wielderPosition;

            float angle = Geometry.AproximateAngleFromDot(shieldForward, wielderToAttacker);

            bool canBlock = angle <= blockableAngle;

            Debug.Log($"CanBlock? {(canBlock ? "Yes" : "No")}, angle {angle} of blockable angle {blockableAngle}");

            return canBlock;
        }

        private void BlockDamageDealer(AttackDeliverer damageDealer)
        {
            if (dealersBlocked.Contains(damageDealer))
            {
                return;
            }

            Debug.Log($"Blocked damage dealer {damageDealer}");
            dealersBlocked.Add(damageDealer);
        }

        public bool HasBlocked(AttackDeliverer damageDealer)
        {
            return dealersBlocked.Contains(damageDealer);
        }

        private void Charge(AttackReceiver attackReceiver, Collider other)
        {
            if (!CanCharge(attackReceiver))
            {
                return;
            }

            float now = Time.time;
            chargedTargets[attackReceiver] = now;
            if (!IntersectionCalculator.IntersectColliders(shieldCollider, other, out Vector3 impactPoint))
            {
                impactPoint = shieldCollider.ClosestPoint(other.transform.position);
            }
            Attack attack = new Attack(wielder, impactPoint, chargeAttack, holdingTime);
            attackReceiver.ReceiveAttack(attack);
        }

        private bool CanCharge(AttackReceiver attackReceiver)
        {
            if (!chargedTargets.TryGetValue(attackReceiver, out float lastTime))
            {
                lastTime = 0f;
            }
            float now = Time.time;
            bool can = (now - lastTime) > chargeImpactCooldown;
            return can;
        }
    }
}
