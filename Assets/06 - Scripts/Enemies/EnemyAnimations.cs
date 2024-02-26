using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using PaladinsFaith.Combat;

namespace PaladinsFaith.Enemies
{
    public class EnemyAnimations : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector moveDirector = null;
        [SerializeField]
        private PlayableDirector combatDirector = null;

        public void StartMoving()
        {
            moveDirector.Play();
        }

        public void StopMoving()
        {
            moveDirector.Stop();
        }

        public void AttackTriggered(AttackData attackData)
        {
            PlayableAsset timeline = attackData.animation;
            combatDirector.playableAsset = timeline;
            combatDirector.time = 0.0;
            combatDirector.Play();
        }
    }
}
