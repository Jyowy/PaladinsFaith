using PaladinsFaith.Math;
using PaladinsFaith.Player;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace PaladinsFaith
{
    public class Shield : MonoBehaviour
    {
        [SerializeField]
        private float blockableAngle = 120f;

        [ShowInInspector, ReadOnly]
        private GameObject wielder = null;
        [ShowInInspector, ReadOnly]
        private readonly List<AttackDeliverer> dealersBlocked = new List<AttackDeliverer>();

        public void SetWielder(GameObject wielder)
        {
            this.wielder = wielder;
        }

        private void OnTriggerEnter(Collider other)
        {
            AttackDeliverer damageDealer = other.GetComponent<AttackDeliverer>();
            if (damageDealer != null)
            {
                BlockDamageDealer(damageDealer);
            }
        }

        private void OnDisable()
        {
            dealersBlocked.Clear();
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
    }
}
