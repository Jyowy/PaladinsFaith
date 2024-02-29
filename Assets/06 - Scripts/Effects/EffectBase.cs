using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Effects
{
    public abstract class EffectBase
    {
        public TargetDetector targetDetector = new TargetDetector();
        public GameObject prefab = null;

        public virtual void Apply(GameObject caster)
        {
            Vector3 center = caster.transform.position;
            List<GameObject> targets = targetDetector.GetTargets(caster, center, null);
            foreach (GameObject target in targets)
            {
                ApplyToTarget(caster, target);
            }
        }

        public virtual void ApplyOnImpacted(GameObject caster, GameObject impactedTarget, Vector3 impactPoint)
        {
            List<GameObject> targets = targetDetector.GetTargets(caster, impactPoint, impactedTarget);
            foreach (GameObject target in targets)
            {
                ApplyToTarget(caster, target);
            }
        }

        protected virtual void ApplyToTarget(GameObject caster, GameObject target) { }
    }
}