using FirstSlice.Effects;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace FirstSlice.Spells
{
    public class SpellEffects
    {
        public List<EffectBase> effects = null;
        public GameObject prefab = null;

        public void Apply(GameObject target)
        {
            foreach (EffectBase effect in effects)
            {
                effect.Apply(target);
            }
        }
    }
}
