using PaladinsFaith.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public struct Attack
    {
        public GameObject attacker;
        public EffectSet effectsOnImpact;
        public Vector3 impactPoint;

        public Attack(GameObject attacker, EffectSet effectsOnImpact, Vector3 impactPoint)
        {
            this.attacker = attacker;
            this.effectsOnImpact = effectsOnImpact;
            this.impactPoint = impactPoint;
        }
    }
}
