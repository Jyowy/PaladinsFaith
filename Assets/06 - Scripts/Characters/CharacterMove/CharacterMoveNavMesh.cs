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
        [SerializeField]
        private float speed = 10f;
        [SerializeField]
        private float acceleration = 10f;

        public UnityEvent OnMoveStarted = null;
        public UnityEvent OnMoveFinished = null;

        private bool moving = false;

        private void Awake()
        {
            agent.speed = speed;
            agent.acceleration = acceleration;
        }

        public override void MoveTo(Vector3 position)
        {
            if (beingPushed)
            {
                return;
            }

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
            if (!moving
                || beingPushed)
            {
                return;
            }

            moving = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            OnMoveFinished?.Invoke();
        }

        private void Update()
        {
            if (beingPushed)
            {
                return;
            }

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

        protected override void PushStarted(Vector3 direction, float power)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = false;
            Vector3 velocity = direction * power;
            Vector3 destination = agent.transform.position + velocity;
            agent.SetDestination(destination);
            agent.velocity = velocity;
        }

        protected override void PushFinished()
        {
            base.PushFinished();

            agent.velocity = Vector3.zero;
            agent.isStopped = false;
        }
    }
}