using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    [System.Serializable]
    public struct ComboChainCondition
    {
        public enum MatchMode
        {
            FullMatch,
            OnlyTail
        }

        [SerializeField]
        private MatchMode matchMode;
        [SerializeField]
        private List<ComboChainLinkCondition> conditions;
        
        public readonly int Length => conditions.Count;

        public readonly bool MatchesChain(ComboChain chain)
        {
            if (InadequateSize(chain))
            {
                return false;
            }

            bool match = true;

            int chainStartIndex = chain.Length - Length;
            int conditionsLength = Length;
            for (int index = 0; match && index < conditionsLength; ++index)
            {
                int chainIndex = chainStartIndex + index;
                match = conditions[index].MatchesChainLink(chain[chainIndex]);
            }

            return match;
        }

        private readonly bool InadequateSize(ComboChain chain)
        {
            return chain.Length < Length
                || (matchMode == MatchMode.FullMatch && chain.Length != Length);
        }

#if UNITY_EDITOR
        public readonly bool ShareSameConditions(ComboChainCondition other)
        {
            bool same = true;

            if (matchMode == MatchMode.FullMatch
                && other.matchMode == MatchMode.FullMatch)
            {
                if (Length != other.Length)
                {
                    same = false;
                }
                else
                {
                    for (int i = 0; i < Length; ++i)
                    {
                        conditions[i].SameCondition(other.conditions[i]);
                    }
                }
            }
            else
            {
                int minLength = Mathf.Min(Length, other.Length);
                int maxLength = Mathf.Max(Length, other.Length);
                int lengthDiff = maxLength - minLength;
                int thisStart;
                int otherStart;
                if (Length < other.Length)
                {
                    thisStart = 0;
                    otherStart = lengthDiff;
                }
                else
                {
                    thisStart = lengthDiff;
                    otherStart = 0;
                }

                for (int i = 0; same && i < minLength; ++i)
                {
                    int thisIndex = thisStart + i;
                    int otherIndex = otherStart + i;

                    same = conditions[thisIndex].SameCondition(other.conditions[otherIndex]);
                }
            }

            return same;
        }
#endif
    }
}