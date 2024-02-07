using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public interface AttackReceiver
    {
        public void ReceiveAttack(Attack attack);
    }
}
