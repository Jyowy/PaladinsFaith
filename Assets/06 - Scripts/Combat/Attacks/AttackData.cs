using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

using PaladinsFaith.Effects;
using Sirenix.OdinInspector;

namespace PaladinsFaith.Combat
{
    [CreateAssetMenu()]
    public class AttackData : SerializedScriptableObject
    {
        public string attackName = "";
        public string description = "";
        public PlayableAsset animation = null;
        public EffectSet effectsOnImpact = null;
    }
}
