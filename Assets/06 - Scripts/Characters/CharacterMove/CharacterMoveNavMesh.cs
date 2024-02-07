using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace PaladinsFaith.Characters
{
    public class CharacterMoveNavMesh : CharacterMoveModule
    {
        [SerializeField]
        private NavMeshAgent agent = null;

        public UnityEvent OnMoveStarted = null;
        public UnityEvent OnMoveFinished = null;

        private bool moving = false;

        public override void MoveTo(Vector3 position)
        {
            bool wasMoving = moving;
            moving = agent.SetDestination(position);

            agent.isStopped = !moving;

            if (!wasMoving && moving)
            {
                OnMoveStarted?.Invoke();
            }
            else if (wasMoving && !moving)
            {
                OnMoveFinished?.Invoke();
            }
        }

        public override void LookAt(Vector3 position)
        {
            agent.transform.LookAt(position, Vector3.up);
        }

        public override void Stop()
        {
            if (!moving)
            {
                return;
            }

            moving = false;
            agent.isStopped = true;

            OnMoveFinished?.Invoke();
        }

        private void Update()
        {
            if (moving)
            {
                CheckDestinationReached();
            }
        }

        private void CheckDestinationReached()
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Stop();
            }
        }
    }
}
