using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public interface DamageDealer
    {
        public void Damage(DamageReceiver receiver, float damage);
    }
}
