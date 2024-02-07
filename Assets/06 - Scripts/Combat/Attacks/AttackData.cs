using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace PaladinsFaith
{
    [CreateAssetMenu()]
    public class AttackData : ScriptableObject
    {
        public string attackName = "";
        public string description = "";
        public float damageMultiplier = 1f;
        public PlayableAsset animation = null;
    }
}
