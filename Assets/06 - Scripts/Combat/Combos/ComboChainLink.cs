using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.Combos
{
    public class ComboChainLink
    {
        public AttackData attack;
        public CombatMove combatMove;

        public ComboChainLink(CombatMove combatMove, AttackData attack)
        {
            this.combatMove = combatMove;
            this.attack = attack;
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"({combatMove}, {attack.attackName})";
        }
#endif
    }
}