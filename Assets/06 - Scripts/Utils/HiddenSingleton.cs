using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PaladinsFaith
{
    public class HiddenSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T Instance { get; private set; } = null;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.LogError($"Multiple Instances of '{this.GetType()}' detected. Previous is in '{Instance.name}', and new one is in '{name}'");
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}