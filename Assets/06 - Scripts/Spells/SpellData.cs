using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaladinsFaith.Effects;

namespace PaladinsFaith.Spells
{
    public enum SpellCastType
    {
        Instant,
        Hold
    }

    [CreateAssetMenu]
    public class SpellData : SerializedScriptableObject
    {
        public string spellName = "";
        [TextArea]
        public string description = "";

        public float manaCost = 10f;
        public float timeToCast = 2f;
        public SpellCastType castType = SpellCastType.Instant;
        [ShowIf(nameof(castType), SpellCastType.Hold)]
        public float spellDuration = 2f;

        public EffectSet EffectsOnStartCasting = null;
        public EffectSet EffectsOnSpellCasted = null;
        [ShowIf(nameof(castType), SpellCastType.Hold)]
        public EffectSet EffectsOnSpellFinished = null;
    }
}
