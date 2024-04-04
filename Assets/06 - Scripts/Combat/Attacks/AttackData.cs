using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using PaladinsFaith.Effects;
using Sirenix.OdinInspector;

namespace PaladinsFaith.Combat
{
    [CreateAssetMenu()]
    public class AttackData : SerializedScriptableObject
    {
        public string attackName = "";
        public string description = "";
        public PlayableAsset animation = null;
        public EffectSet effectsOnImpact = null;
        public bool canBeBlocked = true;
        public bool canBeCharged = false;

        [BoxGroup("Charge", VisibleIf = nameof(canBeCharged))]
        public float minChargeTime = 1f;
        [BoxGroup("Charge")]
        public float maxChargeTime = 3f;
        [BoxGroup("Charge")]
        public AnimationCurve chargeMultiplierEvolution = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Button]
        public float GetChargeMultiplier(float time)
        {
            Debug.Log($"GetChargeMultiplier");
            if (time < minChargeTime)
            {
                Debug.Log($"ZERO");
                return 0f;
            }

            time = Mathf.Clamp(time, minChargeTime, maxChargeTime);
            float progress = Mathf.InverseLerp(minChargeTime, maxChargeTime, time);
            float chargeMultiplier = chargeMultiplierEvolution.Evaluate(progress);
            Debug.Log($"Charge multiplier => {chargeMultiplier}");
            return chargeMultiplier;
        }
    }
}