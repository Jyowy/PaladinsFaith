using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public interface AttackDeliverer
    {
        public void Attack(AttackReceiver receiver, Attack attack);
    }
}
