using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FirstSlice
{
    [System.Serializable]
    public class HealthBar
    {
        [SerializeField]
        private float maxHealth = 10f;

        [ShowInInspector, ReadOnly]
        public bool IsAlive { get; private set; } = true;
        [ShowInInspector, ReadOnly]
        public float CurrentHealth { get; private set; } = 0f;

        public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();
        public UnityEvent<float> OnHealthProgressChanged = new UnityEvent<float>();
        public UnityEvent OnDead = new UnityEvent();

        public void Initialize(UnityAction onDead)
        {
            OnDead?.AddListener(onDead);
            Fill();
        }

        public void Fill()
        {
            if (!IsAlive)
            {
                Debug.LogError($"Trying to fill healthbar when is already dead.");
                return;
            }

            CurrentHealth = maxHealth;
        }

        public void InflictDamage(float damage)
        {
            if (!IsAlive)
            {
                Debug.LogError($"Trying to inflict damage to healthbar when is already dead.");
                return;
            }

            float healthChange = -damage;
            ChangeHealth(healthChange);
        }

        public void Heal(float heal)
        {
            if (!IsAlive)
            {
                Debug.LogError($"Trying to heal a healthbar when is already dead.");
                return;
            }

            ChangeHealth(heal);
        }

        private void ChangeHealth(float change)
        {
            float newHealth = Mathf.Clamp(CurrentHealth + change, 0f, maxHealth);
            if (newHealth != CurrentHealth)
            {
                CurrentHealth = newHealth;
                HealthChanged();
            }
        }

        private void HealthChanged()
        {
            float progress = Mathf.InverseLerp(0f, maxHealth, CurrentHealth);

            OnHealthChanged.Invoke(CurrentHealth);
            OnHealthProgressChanged?.Invoke(progress);

            if (CurrentHealth == 0f)
            {
                IsAlive = false;
                OnDead?.Invoke();
            }
        }
    }
}
