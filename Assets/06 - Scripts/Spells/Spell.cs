using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Spells
{
    public class Spell
    {
        private enum SpellState
        {
            Ready,
            Casting,
            Holding
        }

        private readonly SpellData data = null;
        private readonly GameObject caster = null;
        private SpellState state = SpellState.Ready;

        public Spell(GameObject caster, SpellData data)
        {
            this.caster = caster;
            this.data = data;
            state = SpellState.Ready;
        }

        public void StartCasting()
        {
            if (state != SpellState.Ready)
            {
                throw new System.Exception("Spell was already casted.");
            }

            state = SpellState.Casting;
            ApplyStartCastingEffects();

            float timeToCast = data.timeToCast;
            if (timeToCast > 0)
            {
                Timers.StartGameTimer(caster, "Preparing cast", timeToCast, Cast);
            }
            else
            {
                Cast();
            }
        }

        private void ApplyStartCastingEffects()
        {
            if (data.EffectsOnStartCasting == null)
            {
                return;
            }

            data.EffectsOnStartCasting.Apply(caster);
        }

        private void Cast()
        {
            Debug.Log($"Cast spell {data.spellName}");

            data.EffectsOnSpellCasted.Apply(caster);
        }
    }
}
