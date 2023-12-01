using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Spells
{
    [CreateAssetMenu]
    public class SpellData : SerializedScriptableObject
    {
        public string spellName = "";
        public string description = "";

        public bool isInstant = true;
        public bool doesImpact = false;

        public SpellEffects OnCast = null;
        [HideIf("isInstant")]
        public SpellEffects OnHold = null;
        [ShowIf("doesImpact")]
        public SpellEffects OnImpact = null;
        [HideIf("isInstant")]
        public SpellEffects OnFinish = null;
    }
}
