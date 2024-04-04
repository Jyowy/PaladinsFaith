using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    public class ComboManager
    {
        private readonly ComboList comboList = null;
        private readonly ComboChain chain = null;

        public ComboManager(ComboList comboList)
        {
            this.comboList = comboList;
            chain = new ComboChain();
        }

        public AttackData AddCombo(CombatMove comboElement)
        {
            ComboMatch comboMatch = comboList.GetComboMatch(chain, comboElement);
            AttackData attack = comboMatch.attack;

            if (comboMatch.type == ComboChainMatch.ChainBroken)
            {
                chain.Break();
            }

            chain.AddChainLink(comboElement, attack);

            return attack;
        }

        public void BreakCombo()
        {
            chain.Break();
        }
    }
}