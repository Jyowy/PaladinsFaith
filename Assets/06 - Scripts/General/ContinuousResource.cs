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
        private string resourceName = "resource";
        [SerializeField]
        private float maxValue = 0f;
        [SerializeField]
        private float autoRegainPerSecond = 10f;
        [SerializeField]
        private float cooldownAfterConsume = 1f;
        [SerializeField]
        private float progressToConsiderEmpty = 0.05f;
        [SerializeField]
        private bool fullRefillAfterDeplete = true;

        public UnityEvent<float> OnValueChanged = new UnityEvent<float>();
        public UnityEvent<float> OnProgressChanged = new UnityEvent<float>();
        public UnityEvent OnFullRefillStarted = new UnityEvent();
        public UnityEvent OnFullRefillFinished = new UnityEvent();

        private GameObject owner = null;
        [ShowInInspector, ReadOnly]
        private int automaticRegainBlockers = 0;
        [ShowInInspector]
        private bool AutomaticRegainBlocked => automaticRegainBlockers > 0;
        [ShowInInspector, ReadOnly]
        private float autoRegainFactor = 1f;
        [ShowInInspector, ReadOnly]
        private bool fullRefilling = false;

        [ShowInInspector, ReadOnly]
        public float Value { get; private set; } = 0f;
        [ShowInInspector, ReadOnly]
        public float Progress { get; private set; } = 0f;

        [ShowInInspector, ReadOnly]
        public bool IsFull() => Progress == 1f;
        [ShowInInspector, ReadOnly]
        public bool IsNotFull() => Progress < 1f;
        [ShowInInspector, ReadOnly]
        public bool IsEmpty() => Progress <= progressToConsiderEmpty;
        [ShowInInspector, ReadOnly]
        public bool IsNotEmpty() => Progress > progressToConsiderEmpty;

        [ShowInInspector, ReadOnly]
        private bool consumeCooldownActive = false;

        [Button]
        public void Initialize(GameObject owner)
        {
            this.owner = owner;

            Fill();
        }

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
        public bool TryToConsume(float amount)
        {
            if (fullRefillAfterDeplete
                && fullRefilling)
            {
                return false;
            }

            amount = Mathf.Max(amount, 0f);
            bool hasEnough = HasEnough(amount);
            if (hasEnough)
            {
                Consume(amount);
            }
            return hasEnough;
        }

        [Button]
        public void Consume(float amount)
        {
            if (fullRefillAfterDeplete
                && fullRefilling)
            {
                return;
            }
            if (amount == 0f)
            {
                return;
            }

            amount = Mathf.Max(amount, 0f);
            SetValue(Value - amount);
            Consumed();
        }

        private void Consumed()
        {
            if (!consumeCooldownActive)
            {
                consumeCooldownActive = true;
                AddAutomaticRegainBlocker();
            }
            string name = $"{resourceName}ConsumeCooldown";
            Timers.StartGameTimer(owner, name, cooldownAfterConsume, ConsumedCooldownFinished);

            CheckEmpty();
        }

        private void ConsumedCooldownFinished()
        {
            RemoveAutomaticRegainBlocker();
            consumeCooldownActive = false;
        }

        private void CheckEmpty()
        {
            if (IsNotEmpty())
            {
                return;
            }

            fullRefilling = true;
            SetAutomaticRegainFactor(2f);
            OnFullRefillStarted?.Invoke();
        }

        [Button]
        public void Regain(float amount)
        {
            amount = Mathf.Max(amount, 0f);
            SetValue(Value + amount);

            CheckFull();
        }

        private void CheckFull()
        {
            if (IsNotFull())
            {
                return;
            }

            fullRefilling = false;
            SetAutomaticRegainFactor(1f);
            OnFullRefillFinished?.Invoke();
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
            if (IsFull()
                || AutomaticRegainBlocked)
            {
                return;
            }

            AutoFill(dt);
        }

        private void AutoFill(float dt)
        {
            float autoGain = autoRegainPerSecond * autoRegainFactor * dt;
            Regain(autoGain);
        }
    }
}