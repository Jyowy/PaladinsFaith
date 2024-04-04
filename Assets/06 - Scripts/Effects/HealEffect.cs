using PaladinsFaith.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Effects
{
    [System.Serializable]
    public class HealEffect : EffectBase
    {
        [SerializeField]
        private float amount = 10f;

        protected override void ApplyToTarget(GameObject owner, GameObject target, float multiplier)
        {
            HealCombatantCharacter(target, multiplier);
        }

        private void HealCombatantCharacter(GameObject healingTarget, float multiplier)
        {
            Debug.Log($"Trying to apply heal to '{healingTarget.name}'");
            if (!healingTarget.TryGetComponent(out CombatantCharacter combatant))
            {
                Debug.LogError($"Is not a combatan character!");
                return;
            }

            Debug.Log($"Healed '{combatant.name}': {amount} hp");
            combatant.Heal(amount * multiplier);
        }
    }
}