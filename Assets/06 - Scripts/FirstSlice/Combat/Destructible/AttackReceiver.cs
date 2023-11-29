using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public interface AttackReceiver
    {
        public void ReceiveAttack(Attack attack);
    }
}
