using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaladinsFaith.Characters
{
    public enum CharacterMoveType
    {
        Walking,
        Dashing,
        Running
    }

    public abstract class CharacterMoveModule : MonoBehaviour
    {
        [SerializeField]
        private Transform characterTransform = null;
        [SerializeField]
        protected CharacterMoveType defaultMoveType = CharacterMoveType.Walking;
        [SerializeField]
        protected float walkFactor = 1f;
        [SerializeField]
        protected float runFactor = 2f;
        [SerializeField]
        private float dashConsume = 25f;
        [SerializeField]
        private float runConsumePerSecond = 10f;

        [SerializeField]
        private float simplePushBackStrength = 1f;
        [SerializeField]
        private float simplePushBackDuration = 0.5f;

        [SerializeField]
        private float standUpDuration = 1f;

        [ShowInInspector, ReadOnly]
        public CharacterMoveType MoveType { get; private set; } = CharacterMoveType.Walking;

        public UnityEvent<CharacterMoveType> OnMoveTypeChanged = new UnityEvent<CharacterMoveType>();
        public UnityEvent OnStopped = null;
        public UnityEvent OnPushed = null;
        public UnityEvent<float> OnPushedDuration = null;
        public UnityEvent OnKnockedDown = null;
        public UnityEvent OnStandUp = null;
        public UnityEvent OnStandedUp = null;

        private ContinuousResource stamina = null;
        private bool fastMoveCancelled = false;

        [ShowInInspector, ReadOnly]
        protected bool beingPushed = false;
        [ShowInInspector, ReadOnly]
        protected bool knockedDown = false;

        public void SetStamina(ContinuousResource stamina)
        {
            this.stamina = stamina;
        }

        protected virtual void Start()
        {
            SetMoveType(defaultMoveType);
        }

        public void SetMoveType(CharacterMoveType moveType)
        {
            MoveType = moveType;
            OnMoveTypeChanged?.Invoke(moveType);
        }

        private void Update()
        {
            CheckRunningStamina();
        }

        private void CheckRunningStamina()
        {
            if (MoveType != CharacterMoveType.Running
                || stamina == null)
            {
                return;
            }

            float runConsume = runConsumePerSecond * Time.deltaTime;
            stamina.Consume(runConsume);
            if (stamina.IsEmpty())
            {
                StaminaDepletedWhileRunning();
            }
        }

        private void StaminaDepletedWhileRunning()
        {
            SetMoveType(CharacterMoveType.Walking);
            stamina.RemoveAutomaticRegainBlocker();
        }

        public virtual void Move(Vector3 move) { }

        public virtual void PlanarMove(Vector3 move, float speedFactor) { }

        public virtual void MoveTo(Vector3 position) { }

        bool canDashAgain = false;

        public void UpdateFastMove(bool fastMove, Vector3 direction, float speedFactor)
        {
            if (!canDashAgain)
            {
                canDashAgain = !fastMove;
            }

            if (fastMove && !beingPushed)
            {
                fastMoveCancelled = false;
                if (MoveType == CharacterMoveType.Walking
                    && stamina.HasEnough(dashConsume)
                    && canDashAgain)
                {
                    StartDash(direction, speedFactor);
                }
            }
            else
            {
                fastMoveCancelled = true;
                if (MoveType == CharacterMoveType.Running)
                {
                    stamina.RemoveAutomaticRegainBlocker();
                    SetMoveType(CharacterMoveType.Walking);
                }
            }
        }

        protected virtual void StartDash(Vector3 direction, float speedFactor)
        {
            Debug.Log($"DashStarted");
            bool canDash = stamina.TryToConsume(dashConsume);
            if (canDash)
            {
                SetMoveType(CharacterMoveType.Dashing);
                stamina.AddAutomaticRegainBlocker();
            }
        }

        protected virtual void DashCompleted()
        {
            Debug.Log($"DashCompleted");
            if (fastMoveCancelled)
            {
                Stop();
                SetMoveType(CharacterMoveType.Walking);
                stamina.RemoveAutomaticRegainBlocker();
                return;
            }

            canDashAgain = false;

            SetMoveType(CharacterMoveType.Running);
        }

        public virtual void Stop()
        {
            canDashAgain = false;
        }

        public virtual void LookAt(Vector3 position) { }

        protected float GetMoveModeFactor()
        {
            float factor = MoveType switch
            {
                CharacterMoveType.Walking => walkFactor,
                CharacterMoveType.Running => runFactor,

                _ => walkFactor
            };
            return factor;
        }

        protected virtual void OnMoved()
        {
            RotateToForward();
        }

        protected virtual void RotateToForward() { }

        [Button]
        public void Push(Vector3 direction, float power, float duration)
        {
            Stop();
            beingPushed = true;
            Timers.StartGameTimer(this, "Being Pushed", duration, PushProgress, PushFinished);
            PushStarted(direction, power);
            OnPushed?.Invoke();
            OnPushedDuration?.Invoke(duration);
        }

        [Button]
        public virtual void SimplePushBack()
        {
            Vector3 backwards = -characterTransform.forward;
            Push(backwards, simplePushBackStrength, simplePushBackDuration);
        }

        protected virtual void PushStarted(Vector3 direction, float power) { }

        protected virtual void PushProgress(float progress) { }

        protected virtual void PushFinished()
        {
            beingPushed = false;
            MoveType = CharacterMoveType.Walking;
        }

        public virtual void KnockDownStarted()
        {
            knockedDown = true;
            Stop();
            OnKnockedDown?.Invoke();
        }

        public virtual void KnockDownFinished()
        {
            OnStandUp?.Invoke();

            Debug.Log($"Character '{name}' is standing up");
            Timers.StartGameTimer(this, "StandingUp", standUpDuration, StandedUp);
        }

        protected virtual void StandedUp()
        {
            knockedDown = false;
            OnStandedUp?.Invoke();
        }
    }
}