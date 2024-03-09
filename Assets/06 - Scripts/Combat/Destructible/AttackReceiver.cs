using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public enum AttackResult
    {
        Invalid,
        Defended,
        SuccessButNoDamage,
        Success
    }

    public interface AttackReceiver
    {
        public AttackResult ReceiveAttack(Attack attack);
    }
}
