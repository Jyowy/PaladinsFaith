using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace FirstSlice
{
    public class CinematicPlayer : HiddenSingleton<CinematicPlayer>
    {
        [SerializeField]
        private PlayableDirector cinematicDirector = null;

        public static void PlayCinematic(PlayableAsset cinematic)
        {
            Instance.PlayCinematic_internal(cinematic);
        }

        private void PlayCinematic_internal(PlayableAsset cinematic)
        {
            StopDirector();
            cinematicDirector.playableAsset = cinematic;
            cinematicDirector.time = 0.0;
            cinematicDirector.Play();
        }

        private void StopDirector()
        {
            cinematicDirector.Stop();
            cinematicDirector.playableAsset = null;
        }
    }
}
