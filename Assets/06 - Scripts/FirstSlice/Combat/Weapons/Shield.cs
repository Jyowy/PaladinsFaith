using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class Shield : MonoBehaviour
    {
        [SerializeField]
        private DamageReceiver damageReceiver = null;

        [ShowInInspector, ReadOnly]
        private readonly List<DamageDealer> dealersBlocked = new List<DamageDealer>();

        private void OnTriggerEnter(Collider other)
        {
            DamageDealer damageDealer = other.GetComponent<DamageDealer>();
            if (damageDealer != null)
            {
                BlockDamageDealer(damageDealer);
            }
        }

        private void OnDisable()
        {
            dealersBlocked.Clear();
        }

        private void BlockDamageDealer(DamageDealer damageDealer)
        {
            if (dealersBlocked.Contains(damageDealer))
            {
                return;
            }

            Debug.Log($"Blocked damage dealer {damageDealer}");
            dealersBlocked.Add(damageDealer);
        }

        public bool HasBlocked(DamageDealer damageDealer)
        {
            return dealersBlocked.Contains(damageDealer);
        }
    }
}
