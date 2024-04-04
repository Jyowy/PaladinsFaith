using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    [System.Serializable]
    public struct ComboChainLinkCondition
    {
        public enum MatchMode
        {
            ComboElement,
            Attack
        }

        public MatchMode matchMode;
        [ShowIf(nameof(matchMode), MatchMode.ComboElement)]
        public CombatMove comboElement;
        [ShowIf(nameof(matchMode), MatchMode.Attack)]
        public AttackData attack;
        
        public readonly bool MatchesChainLink(ComboChainLink chainLink)
        {
            return Matches(chainLink.combatMove, chainLink.attack);
        }

        private readonly bool Matches(CombatMove comboElement, AttackData attack)
        {
            bool match;
            if (matchMode == MatchMode.ComboElement)
            {
                match = SameComboElement(comboElement);
            }
            else
            {
                match = SameAttack(attack);
            }
            return match;
        }

        private readonly bool SameComboElement(CombatMove comboElement)
        {
            return comboElement == this.comboElement;
        }

        private readonly bool SameAttack(AttackData attack)
        {
            return attack == this.attack;
        }

#if UNITY_EDITOR
        public readonly bool SameCondition(ComboChainLinkCondition other)
        {
            return Matches(other.comboElement, other.attack);
        }
#endif
    }
}