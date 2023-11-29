using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class Destructible : MonoBehaviour, AttackReceiver
    {
        [SerializeField]
        private bool oneHit = true;
        [SerializeField, HideIf("oneHit")]
        private HealthBar healthBar = new HealthBar();

        [ShowInInspector, ReadOnly]
        private bool destroyed = false;

        private void Awake()
        {
            healthBar.Initialize(Destroy);
        }

        [Button]
        public void ReceiveAttack(Attack attack)
        {
            if (destroyed)
            {
                return;
            }

            if (oneHit)
            {
                Destroy();
            }
            else
            {
                healthBar.ReceiveAttack(attack);
            }
        }

        public void Destroy()
        {
            if (destroyed)
            {
                return;
            }

            destroyed = true;
            Destroy(gameObject);
        }
    }
}
