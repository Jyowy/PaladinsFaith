using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith
{
    [Serializable]
    public class ContinuousResource
    {
        public ContinuousResource(float maxValue)
        {
            this.maxValue = maxValue;
        }

        [SerializeField]
        private float maxValue = 0f;
        [SerializeField]
        private float autoRegainPerSecond = 10f;

        public UnityEvent<float> OnValueChanged = null;
        public UnityEvent<float> OnProgressChanged = null;

        [ShowInInspector, ReadOnly]
        private int automaticRegainBlockers = 0;
        [ShowInInspector]
        private bool AutomaticRegainBlocked => automaticRegainBlockers > 0;
        [ShowInInspector, ReadOnly]
        private float autoRegainFactor = 1f;
        [ShowInInspector, ReadOnly]
        public float Value { get; private set; } = 0f;
        [ShowInInspector, ReadOnly]
        public float Progress { get; private set; } = 0f;

        [Button]
        public void Fill()
        {
            SetValue(maxValue);
        }

        [Button]
        private void SetValue(float value)
        {
            value = Mathf.Clamp(value, 0f, maxValue);
            if (value == Value)
            {
                return;
            }

            Value = value;
            Progress = Mathf.Clamp01(Value / maxValue);
            OnValueChanged?.Invoke(Value);
            OnProgressChanged?.Invoke(Progress);
        }

        public bool HasEnough(float amount) => Value >= amount;

        [Button]
        public bool Consume(float amount)
        {
            amount = Mathf.Max(amount, 0f);
            bool hasEnough = HasEnough(amount);
            if (hasEnough)
            {
                SetValue(Value - amount);
            }
            return hasEnough;
        }

        [Button]
        public void Regain(float amount)
        {
            amount = Mathf.Max(amount, 0f);
            SetValue(Value + amount);
        }

        [Button]
        private void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            float value = maxValue * progress;
            SetValue(value);
        }

        public bool HasEnoughPorgress(float progress) => Progress >= progress;

        [Button]
        public bool ConsumeProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            bool hasEnough = HasEnoughPorgress(progress);
            if (hasEnough)
            {
                SetProgress(Progress - progress);
            }
            return hasEnough;
        }

        [Button]
        public void RegainProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            SetProgress(Progress + progress);
        }

        public void AddAutomaticRegainBlocker()
        {
            automaticRegainBlockers++;
        }

        public void RemoveAutomaticRegainBlocker()
        {
            automaticRegainBlockers--;
        }

        public void SetAutomaticRegainFactor(float factor)
        {
            autoRegainFactor = factor;
        }

        public void Update(float dt)
        {
            if (Value >= maxValue
                || AutomaticRegainBlocked)
            {
                return;
            }

            float autoGain = autoRegainPerSecond * autoRegainFactor * dt;
            Regain(autoGain);
        }

    }
}