using PaladinsFaith.Characters;
using PaladinsFaith.Spells;
using System.Collections;
using System.Collections.Generic;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Player
{
    public class PlayerSharedData
    {
        public PlayerCamera camera = null;
        public ContinuousResource stamina = null;
        public ContinuousResource mana = null;
        public CharacterMoveModule moveModule = null;
        public CombatModule combatModule = null;
        public SpellModule spellModule = null;
    }
}