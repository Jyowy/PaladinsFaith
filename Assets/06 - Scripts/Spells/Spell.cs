using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Spells
{
    public enum SpellStateAsdas
    {
        Ready,
        Casting,
        SearchingTarget,
        Impacting
    }

    public class Spell
    {
        private readonly SpellData data = null;
        private SpellStateAsdas state = SpellStateAsdas.Ready;
        private GameObject caster = null;

        public Spell(GameObject caster, SpellData data)
        {
            this.caster = caster;
            this.data = data;
        }

        public bool TryToCast()
        {
            if (state != SpellStateAsdas.Ready)
            {
                return false;
            }

            Cast();
            return true;
        }

        private void Cast()
        {
            // TODO
            Debug.Log($"Cast spell {data.spellName}");
            data.OnCast.Apply(caster);
            //data.OnCast.CastEffects();
        }
    }
}
