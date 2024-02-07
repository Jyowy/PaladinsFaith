using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith
{
    public class Gauge : MonoBehaviour
    {
        [SerializeField]
        private float minValue = 0f;
        [SerializeField]
        private float maxValue = 1f;
        [SerializeField]
        private float defaultValue = 1f;

        public UnityEvent<float> OnValueChanged = null;
        public UnityEvent<float> OnProgressChanged = null;

        [ShowInInspector, ReadOnly]
        public float Value { get; private set; } = 0f;
        [ShowInInspector, ReadOnly]
        public float Progress { get; private set; } = 0f;

        protected virtual void Start()
        {
            SetValue(defaultValue);
        }

        [Button]
        public virtual void SetValue(float value)
        {
            Value = value;
            Progress = GetProgressFromValue(value); 
            ValueChanged();
        }

        private float GetProgressFromValue(float value)
        {
            float progress = Mathf.InverseLerp(minValue, maxValue, value);
            return progress;
        }

        [Button]
        public virtual void SetProgress(float progress)
        {
            Value = GetValueFromProgress(progress);
            Progress = progress;
            ValueChanged();
        }

        private float GetValueFromProgress(float progress)
        {
            float value = Mathf.Lerp(minValue, maxValue, progress);
            return value;
        }

        protected virtual void ValueChanged()
        {
            OnValueChanged?.Invoke(Value);
            OnProgressChanged?.Invoke(Progress);
        }

        protected void AddCallbacks(UnityAction<float> onValueChanged, UnityAction<float> onProgressChanged)
        {
            OnValueChanged?.AddListener(onValueChanged);
            OnProgressChanged?.AddListener(onProgressChanged);
        }

        protected virtual void OnDestroy()
        {
            RemoveAllCallbacks();
        }

        private void RemoveAllCallbacks()
        {
            OnValueChanged?.RemoveAllListeners();
            OnProgressChanged?.RemoveAllListeners();
        }
    }
}
