using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat.AlteredStates;

namespace PaladinsFaith.Effects
{
    [System.Serializable]
    public class AlteredStateEffect : EffectBase
    {
        [SerializeField]
        private AlteredState alteredState = new AlteredState();

        protected override void ApplyToTarget(GameObject caster, GameObject target, float multiplier)
        {
            Debug.Log($"Trying to apply altered state '{alteredState.type}' for {alteredState.duration} s to '{target.name}'");
            if (!target.TryGetComponent(out AlteredStateReceiver receiver))
            {
                return;
            }

            receiver.ReceiveAlteredState(alteredState);
        }
    }
}