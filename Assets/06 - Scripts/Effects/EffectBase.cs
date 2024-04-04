using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Effects
{
    public abstract class EffectBase
    {
        public TargetDetector targetDetector = new TargetDetector();
        public GameObject prefab = null;

        public virtual void Apply(GameObject caster, float multiplier)
        {
            Vector3 center = caster.transform.position;
            List<GameObject> targets = targetDetector.GetTargets(caster, center, null);
            foreach (GameObject target in targets)
            {
                ApplyToTarget(caster, target, multiplier);
            }
        }

        public virtual void ApplyOnImpacted(GameObject caster, GameObject impactedTarget, Vector3 impactPoint, float multiplier)
        {
            List<GameObject> targets = targetDetector.GetTargets(caster, impactPoint, impactedTarget);
            foreach (GameObject target in targets)
            {
                ApplyToTarget(caster, target, multiplier);
            }
        }

        protected virtual void ApplyToTarget(GameObject caster, GameObject target, float multiplier) { }
    }
}