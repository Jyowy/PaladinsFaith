using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinsFaith.Effects
{
    [System.Serializable]
    public class PrefabEffect : EffectBase
    {
        [SerializeField]
        private GameObject prefab = null;

        protected override void ApplyToTarget(GameObject owner, GameObject target, float multiplier)
        {
            GameObject.Instantiate(prefab, target.transform);
        }
    }
}