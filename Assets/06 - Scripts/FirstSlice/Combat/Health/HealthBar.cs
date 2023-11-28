using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice
{
    [System.Serializable]
    public class HealthBar : DamageReceiver
    {
        [SerializeField]
        private float maxHealth = 10f;

        [ShowInInspector, ReadOnly]
        public float CurrentHealth { get; private set; } = 0f;
        [ShowInInspector, ReadOnly]
        public bool IsAlive { get; private set; } = true;

        public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();
        public UnityEvent OnDead = new UnityEvent();

        public void Initialize(UnityAction onDead)
        {
            OnDead?.AddListener(onDead);
            Fill();
        }

        public void Fill()
        {
            if (IsAlive)
            {
                CurrentHealth = maxHealth;
            }
        }

        public void ReceiveDamage(DamageDealer damageDealer, float damage)
        {
            if (IsAlive)
            {
                ChangeHealth(-damage);
            }
        }

        private void ChangeHealth(float change)
        {
            float newHealth = Mathf.Clamp(CurrentHealth + change, 0f, maxHealth);
            if (newHealth != CurrentHealth)
            {
                CurrentHealth = newHealth;
                OnHealthChanged.Invoke(newHealth);

                if (newHealth == 0f)
                {
                    IsAlive = false;
                    OnDead?.Invoke();
                }
            }
        }
    }
}
