using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    public class ComboChain
    {
        private readonly List<ComboChainLink> chain = new List<ComboChainLink>();

        public int Length => chain.Count;
        public ComboChainLink this[int i] { get => chain[i]; }

        public void Break()
        {
            chain.Clear();
        }

        public void AddChainLink(CombatMove comboElement, AttackData attack)
        {
            ComboChainLink link = new ComboChainLink(comboElement, attack);
            AddChainLink(link);
        }

        private void AddChainLink(ComboChainLink link)
        {
            chain.Add(link);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            string stringed = "[ ";
            foreach (ComboChainLink link in chain)
            {
                stringed += $"{link} ";
            }
            return stringed + "]";
        }
#endif
    }
}