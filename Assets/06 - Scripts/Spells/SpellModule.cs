using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Spells
{
    public class SpellModule : MonoBehaviour
    {
        [SerializeField]
        private GameObject caster = null;

        [SerializeField]
        private List<SpellData> spells = new List<SpellData>();
        [Space]

        public UnityEvent NoSpellPrepared = null;
        public UnityEvent<SpellData> OnAvailableSpellChanged = null;
        public UnityEvent NotEnoughMana = null;
        public UnityEvent<Spell> OnSpellCasted = null;

        private int currentSpellIndex = -1;
        private SpellData preparedSpellData = null;
        private Spell castingSpell = null;

        private bool casting = false;

        private ContinuousResource mana = null;

        public void SetMana(ContinuousResource mana)
        {
            this.mana = mana;
        }

        private void Start()
        {
            SetSpellIndex(0);
        }

        public void CastPreparedSpell()
        {
            if (preparedSpellData == null)
            {
                NoSpellPrepared?.Invoke();
                return;
            }

            CastSpell(preparedSpellData);
        }

        private void CastSpell(SpellData spellData)
        {
            if (!HasEnoughManaToCast(spellData))
            {
                NotEnoughMana?.Invoke();
                return;
            }

            ConsumeManaForSpell(spellData);

            Spell spellToCast = CreateSpellInstance(preparedSpellData);
            spellToCast.StartCasting();
            castingSpell = spellToCast;
            OnSpellCasted?.Invoke(castingSpell);
        }

        private bool HasEnoughManaToCast(SpellData spellData)
        {
            float manaAmount = preparedSpellData.manaCost;
            return mana.HasEnough(manaAmount);
        }

        private void ConsumeManaForSpell(SpellData spellData)
        {
            mana.TryToConsume(spellData.manaCost);
        }

        private bool HasEnoughMana(float amount)
        {
            return mana.HasEnough(amount);
        }

        private Spell CreateSpellInstance(SpellData spellData)
        {
            Spell spell = new Spell(caster, spellData);
            return spell;
        }

        public void PrevSpell()
        {
            SetSpellIndex(currentSpellIndex - 1);
        }

        public void NextSpell()
        {
            SetSpellIndex(currentSpellIndex + 1);
        }

        private void SetSpellIndex(int index)
        {
            int count = spells.Count;

            if (index < 0)
            {
                index += count;
            }
            else if (index >= count)
            {
                index = count - index;
            }

            if (index == currentSpellIndex)
            {
                return;
            }

            currentSpellIndex = index;
            preparedSpellData = spells[index];
            CurrentSpellChanged();
        }

        private void CurrentSpellChanged()
        {
            Debug.Log($"Current spell changed to {preparedSpellData.spellName}");
            OnAvailableSpellChanged?.Invoke(preparedSpellData);
        }
    }
}