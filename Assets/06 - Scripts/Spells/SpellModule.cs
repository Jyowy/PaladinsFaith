using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Spells
{
    public class SpellModule : SerializedMonoBehaviour
    {
        [SerializeField]
        private GameObject caster = null;

        [SerializeField]
        private List<SpellData> spells = new List<SpellData>();

        public UnityEvent<SpellData> OnAvailableSpellChanged = null;
        public UnityEvent<Spell> OnSpellCasted = null;

        private int currentSpellIndex = -1;
        private SpellData currentSpellData = null;
        private Spell currentSpell = null;

        private bool casting = false;

        private void Start()
        {
            SetSpellIndex(0);
        }

        public void CastSpell()
        {
            if (currentSpellData == null)
            {
                return;
            }

            currentSpell = new Spell(caster, currentSpellData);
            bool casted = currentSpell.TryToCast();
            if (casted)
            {
                OnSpellCasted?.Invoke(currentSpell);
            }
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
            currentSpellData = spells[index];
            CurrentSpellChanged();
        }

        private void CurrentSpellChanged()
        {
            Debug.Log($"Current spell changed to {currentSpellData.spellName}");
            OnAvailableSpellChanged?.Invoke(currentSpellData);
        }
    }
}
