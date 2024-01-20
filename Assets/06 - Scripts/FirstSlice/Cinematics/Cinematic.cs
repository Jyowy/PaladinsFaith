using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace FirstSlice
{
    [CreateAssetMenu()]
    public class Cinematic : ScriptableObject
    {
        public bool disablePlayerController = true;
        public PlayableAsset playableAsset = null;
    }
}
