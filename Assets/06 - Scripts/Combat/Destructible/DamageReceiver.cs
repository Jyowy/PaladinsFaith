using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith
{
    public interface DamageReceiver
    {
        public void ReceiveDamage(float damage);
    }
}