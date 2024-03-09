using PaladinsFaith.Combat;
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
        public bool canBeBlocked;

        public Attack(GameObject attacker, Vector3 impactPoint, AttackData attackData)
        {
            this.attacker = attacker;
            this.impactPoint = impactPoint;
            this.effectsOnImpact = attackData.effectsOnImpact;
            this.canBeBlocked = attackData.canBeBlocked;
        }
    }
}
