using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.AlteredStates
{
    public class AlteredState
    {
        [System.Flags]
        public enum Type
        {
            None = 0,
            Stunned = 0x1,
            KnockedDown = 0x1 << 1,
            Shielded = 0x1 << 2,
        }

        public Type type = Type.Stunned;
        public float duration = 1f;
    }
}