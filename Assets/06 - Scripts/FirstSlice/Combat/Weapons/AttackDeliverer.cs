using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public interface AttackDeliverer
    {
        public void Attack(AttackReceiver receiver, Attack attack);
    }
}
