using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    [CreateAssetMenu]
    public class Combo : ScriptableObject
    {
        public string comboName = "combo";
        public AttackData attack = null;

        [SerializeField]
        private CombatMove comboElement;
        [SerializeField]
        private bool hasChainCondition = true;
        [SerializeField, ShowIf(nameof(hasChainCondition))]
        private ComboChainCondition chainCondition;

        private bool NoCondition => !hasChainCondition || chainCondition.Length == 0;

        public bool BrokenMatch(CombatMove combatMove)
        {
            return combatMove == this.comboElement
                && NoCondition;
        }

        public bool PerfectMatch(ComboChain chain, CombatMove comboElement)
        {
            bool match = comboElement == this.comboElement;
            if (hasChainCondition)
            {
                match = match && chainCondition.MatchesChain(chain);
            }
            else
            {
                match = match && chain.Length == 0;
            }
            return match;
        }

#if UNITY_EDITOR
        public bool HasSameConditionsAs(Combo other)
        {
            return ShareSameComboElement(other)
                && ShareSameConditions(other);
        }

        private bool ShareSameComboElement(Combo other)
        {
            return comboElement == other.comboElement;
        }

        private bool ShareSameConditions(Combo other)
        {
            return chainCondition.ShareSameConditions(other.chainCondition);
        }
#endif
    }
}