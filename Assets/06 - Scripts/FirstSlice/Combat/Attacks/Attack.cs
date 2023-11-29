using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public struct Attack
    {
        public GameObject attacker;
        public float damage;

        public Attack(GameObject attacker, float damage)
        {
            this.attacker = attacker;
            this.damage = damage;
        }
    }
}
