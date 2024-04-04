using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PaladinsFaith.Cinematics
{
    public class CinematicPlayer : HiddenSingleton<CinematicPlayer>
    {
        private enum State
        {
            Idle,
            Playing,
            Finishing,
        }

        [SerializeField]
        private PlayableDirector cinematicDirector = null;

        public UnityEvent OnCinematicStarted = null;
        public UnityEvent OnCinematicFinished = null;

        [ShowInInspector, ReadOnly]
        private Cinematic currentCinematic = null;
        [ShowInInspector, ReadOnly]
        private State state = State.Idle;

        private void Start()
        {
            cinematicDirector.stopped += OnDirectorStopped;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (cinematicDirector != null)
            {
                cinematicDirector.stopped -= OnDirectorStopped;
            }
        }

        public static void PlayCinematic(Cinematic cinematic)
        {
            Instance.PlayCinematic_internal(cinematic);
        }

        private void PlayCinematic_internal(Cinematic cinematic)
        {
            if (state != State.Idle)
            {
                Debug.LogError($"Can't play cinematic '{cinematic.name}' because cinematic '{currentCinematic.name}' isn't over yet (current state is '{state}').");
                return;
            }

            ClearCinematic();

            currentCinematic = cinematic;
            state = State.Playing;
            StartCinematic(currentCinematic.playableAsset);
        }

        private void ClearCinematic()
        {
            currentCinematic = null;
            cinematicDirector.Stop();
            cinematicDirector.playableAsset = null;
        }

        private void StartCinematic(PlayableAsset playableAsset)
        {
            cinematicDirector.playableAsset = playableAsset;
            cinematicDirector.time = 0.0;
            cinematicDirector.Play();

            Debug.Log($"Cinematic Started");
            OnCinematicStarted?.Invoke();
        }

        [Button]
        public static void SkipCinematic()
        {
            Instance.SkipCinematic_Internal();
        }

        private void SkipCinematic_Internal()
        {
            if (currentCinematic == null
                || state != State.Playing)
            {
                return;
            }

            state = State.Finishing;
            double endCinematicTime = GetEndCinematicTime();
            cinematicDirector.time = endCinematicTime;
        }

        private double GetEndCinematicTime()
        {
            double time = 0.0;

            TimelineAsset asset = (TimelineAsset)cinematicDirector.playableAsset;
            IEnumerable<IMarker> markers = asset.markerTrack.GetMarkers();
            foreach (IMarker marker in markers)
            {
                if (marker is EndPointMarker)
                {
                    time = marker.time;
                    break;
                }
            }

            return time;
        }

        private void OnDirectorStopped(PlayableDirector _)
        {
            Debug.Log($"OnDirectorStopped: {currentCinematic != null}");
            if (currentCinematic == null)
            {
                return;
            }

            state = State.Idle;
            Debug.Log($"Cinematic Finished");
            OnCinematicFinished?.Invoke();

            ClearCinematic();
        }
    }
}