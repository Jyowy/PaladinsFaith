using PaladinsFaith.Characters;
using PaladinsFaith.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PaladinsFaith.Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector moveDirector = null;
        [SerializeField]
        private PlayableDirector defenseDirector = null;
        [SerializeField]
        private PlayableDirector attackDirector = null;

        [SerializeField]
        private PlayableAsset walkTimeline = null;
        [SerializeField]
        private PlayableAsset runTimeline = null;

        [SerializeField]
        private CombatModule combatModule = null;

        public UnityEvent OnNextAttackReady = null;
        public UnityEvent OnAttackFinished = null;

        public void PlayerStopped()
        {
            StopMoveAnimations();
        }

        public void StopMoveAnimations()
        {
            if (moveDirector.state != PlayState.Playing)
            {
                return;
            }

            moveDirector.time = moveDirector.playableAsset.duration;
            moveDirector.Evaluate();
            moveDirector.Stop();
            moveDirector.playableAsset = null;
        }

        public void PlayerMoveModeChanged(CharacterMoveType mode)
        {
            PlayMoveAnimation(mode);
        }

        private void PlayMoveAnimation(CharacterMoveType mode)
        {
            PlayableAsset timeline = mode switch
            {
                CharacterMoveType.Walking => walkTimeline,
                CharacterMoveType.Running => runTimeline,

                _ => walkTimeline
            };

            PlayTimeline(moveDirector, timeline);
        }

        private void PlayTimeline(PlayableDirector director, PlayableAsset timeline)
        {
            director.playableAsset = timeline;
            director.time = 0.0;
            director.Play();
        }

        public void DefenseStarted()
        {
            StopMoveAnimations();
            PlayDefenseAnimation();
        }

        private void PlayDefenseAnimation()
        {
            defenseDirector.Play();
        }

        public void DefenseFinished()
        {
            defenseDirector.time = 0f;
            defenseDirector.Evaluate();
            defenseDirector.Stop();
        }

        public void AttackStarted(AttackData attackData)
        {
            PlayAttackAnimation(attackData.animation);
        }

        private void PlayAttackAnimation(PlayableAsset timeline)
        {
            PlayTimeline(attackDirector, timeline);
        }

        public void NextAttackReady()
        {
            OnNextAttackReady?.Invoke();
        }

        public void AttackAnimationFinished()
        {
            AttackFinished();
        }

        public void AttackFinished()
        {
            OnAttackFinished?.Invoke();
        }

        public void CancelAttack()
        {
            attackDirector.Stop();
        }

        public void LoopAttack()
        {
            TimelineAsset timeline = (TimelineAsset)attackDirector.playableAsset;
            IEnumerable<IMarker> markers = timeline.markerTrack.GetMarkers();
            foreach (IMarker marker in markers)
            {
                if (marker is LoopStartMarker)
                {
                    double time = marker.time;
                    attackDirector.time = time;
                    break;
                }
            }
        }

        [Button]
        public void ReleaseHoldingAttack()
        {
            TimelineAsset asset = (TimelineAsset)attackDirector.playableAsset;
            IEnumerable<IMarker> markers = asset.markerTrack.GetMarkers();
            foreach (IMarker marker in markers)
            {
                if (marker is ReleaseAttackMarker)
                {
                    attackDirector.Pause();
                    double time = marker.time;
                    attackDirector.time = time;
                    attackDirector.Resume();
                    break;
                }
            }    
        }
    }
}
