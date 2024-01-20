using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice
{
    public class CinematicTrigger : MonoBehaviour
    {
        [SerializeField]
        private Cinematic cinematic = null;
        [SerializeField]
        private bool triggerOnStart = true;

        private void Start()
        {
            if (triggerOnStart)
            {
                Play();
            }
        }

        [Button]
        public void Play()
        {
            CinematicPlayer.PlayCinematic(cinematic);
        }

        [Button]
        public void Stop()
        {
            CinematicPlayer.SkipCinematic();
        }
    }
}
