using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public interface DamageReceiver
    {
        public void ReceiveDamage(DamageDealer damageDealer, float damage);
    }
}
