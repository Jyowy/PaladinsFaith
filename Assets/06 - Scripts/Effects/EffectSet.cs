using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Effects
{
    public class EffectSet
    {
        public List<EffectBase> effects = new List<EffectBase>();

        public void Apply(GameObject caster)
        {
            Apply(caster, 1f);
        }

        public void Apply(GameObject caster, float multiplier)
        {
            foreach (EffectBase effect in effects)
            {
                effect.Apply(caster, multiplier);
            }
        }

        public void ApplyOnImpact(GameObject caster, GameObject impactedTarget, Vector3 impactPoint)
        {
            ApplyOnImpact(caster, impactedTarget, impactPoint, 1f);
        }

        public void ApplyOnImpact(GameObject caster, GameObject impactedTarget, Vector3 impactPoint, float multiplier)
        {
            foreach (EffectBase effect in effects)
            {
                effect.ApplyOnImpacted(caster, impactedTarget, impactPoint, multiplier);
            }
        }
    }
}
