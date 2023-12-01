using FirstSlice.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Effects
{
    [System.Serializable]
    public class HealEffect : EffectBase
    {
        [SerializeField]
        private float amount = 10f;

        public override void Apply(GameObject target)
        {
            Debug.Log($"Trying to apply heal to '{target.name}'");
            if (!target.TryGetComponent(out CombatantCharacter combatant))
            {
                return;
            }

            Debug.Log($"Healed '{combatant.name}': {amount} hp");
            combatant.Heal(amount);
        }
    }
}
