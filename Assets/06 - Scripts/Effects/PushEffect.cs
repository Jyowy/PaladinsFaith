using PaladinsFaith.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Effects
{
    [System.Serializable]
    public class PushEffect : EffectBase
    {
        [SerializeField]
        private float strength = 10f;
        [SerializeField]
        private float duration = 1f;

        protected override void ApplyToTarget(GameObject caster, GameObject target)
        {
            Debug.Log($"Trying to apply push to '{target.name}'");
            if (!target.TryGetComponent(out CombatantCharacter combatant))
            {
                return;
            }

            Debug.Log($"Push '{combatant.name}': {strength}");
            Vector3 direction = target.transform.position - caster.transform.position;
            Vector3 pushDirection = direction.NormalizedWithoutY();
            combatant.Push(pushDirection, strength, duration);
        }
    }
}