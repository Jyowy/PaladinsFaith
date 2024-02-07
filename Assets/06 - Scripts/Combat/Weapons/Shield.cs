using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public class Shield : MonoBehaviour
    {
        // TODO

        [SerializeField]
        private AttackReceiver attackReceiver = null;

        [ShowInInspector, ReadOnly]
        private readonly List<AttackDeliverer> dealersBlocked = new List<AttackDeliverer>();

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
