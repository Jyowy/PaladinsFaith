using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    public enum ComboChainMatch
    {
        ChainMatch,
        ChainBroken
    }

    public struct ComboMatch
    {
        public ComboChainMatch type;
        public AttackData attack;

        public ComboMatch(ComboChainMatch type, AttackData attack)
        {
            this.type = type;
            this.attack = attack;
        }
    }

    [CreateAssetMenu]
    public class ComboList : ScriptableObject
    {
        [SerializeField]
        private List<Combo> combos = new List<Combo>();

        public ComboMatch GetComboMatch(ComboChain comboChain, CombatMove combatMove)
        {
            ComboChainMatch chainMatch = ComboChainMatch.ChainMatch;
            AttackData attack = GetConsecutiveMatch(comboChain, combatMove);

            if (attack == null)
            {
                chainMatch = ComboChainMatch.ChainBroken;
                attack = GetStartMatch(combatMove);
            }

            ComboMatch comboResult = new ComboMatch(chainMatch, attack);
            return comboResult;
        }

        private AttackData GetConsecutiveMatch(ComboChain comboChain, CombatMove combatMove)
        {
            AttackData attack = null;

            foreach (Combo combo in combos)
            {
                bool comboMatch = combo.PerfectMatch(comboChain, combatMove);
                if (comboMatch)
                {
                    attack = combo.attack;
                    break;
                }
            }

            return attack;
        }

        private AttackData GetStartMatch(CombatMove combatMove)
        {
            AttackData attack = null;

            foreach (Combo combo in combos)
            {
                bool comboMatch = combo.BrokenMatch(combatMove);
                if (comboMatch)
                {
                    attack = combo.attack;
                    break;
                }
            }

            return attack;
        }

#if UNITY_EDITOR
        [Button]
        public void CheckRepetitions()
        {
            bool allCombosUnique = true;
            for (int i = 0; i < combos.Count; i++)
            {
                Combo combo1 = combos[i];
                for (int j = i + 1; j < combos.Count; j++)
                {
                    Combo combo2 = combos[j];
                    bool sameCombo = combo1.HasSameConditionsAs(combo2);
                    if (sameCombo)
                    {
                        allCombosUnique = false;
                        Debug.LogWarning($"Combos '{combo1.comboName}' and '{combo2.comboName}' are triggered with the same conditions.");
                    }
                }
            }

            if (allCombosUnique)
            {
                Debug.Log($"No combos are triggered with the same conditions.");
            }
        }
#endif
    }
}