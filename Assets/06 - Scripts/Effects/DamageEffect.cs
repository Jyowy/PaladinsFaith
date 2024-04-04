using PaladinsFaith.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Effects
{
    [System.Serializable]
    public class DamageEffect : EffectBase
    {
        [SerializeField]
        private float amount = 10f;

        protected override void ApplyToTarget(GameObject owner, GameObject target, float multiplier)
        {
            Debug.Log($"Trying to apply damage to '{target.name}'");
            if (!target.TryGetComponent(out DamageReceiver damageReceiver))
            {
                return;
            }

            damageReceiver.ReceiveDamage(amount * multiplier);
        }
    }
}