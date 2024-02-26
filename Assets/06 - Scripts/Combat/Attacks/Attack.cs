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

        public Attack(GameObject attacker, EffectSet effectsOnImpact)
        {
            this.attacker = attacker;
            this.effectsOnImpact = effectsOnImpact;
        }
    }
}
