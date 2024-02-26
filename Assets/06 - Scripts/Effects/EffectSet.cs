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
            foreach (EffectBase effect in effects)
            {
                effect.Apply(caster);
            }
        }

        public void ApplyOnImpact(GameObject caster, GameObject impactedTarget)
        {
            foreach (EffectBase effect in effects)
            {
                effect.ApplyOnImpacted(caster, impactedTarget);
            }
        }
    }
}
