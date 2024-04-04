using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Combat.AlteredStates
{
    public interface AlteredStateReceiver
    {
        public void ReceiveAlteredState(AlteredState state);
    }
}