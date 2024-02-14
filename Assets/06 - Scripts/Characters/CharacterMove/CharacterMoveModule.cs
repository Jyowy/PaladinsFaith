using PaladinsFaith.Player;
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
        protected CharacterMoveType defaultMoveType = CharacterMoveType.Walking;
        [SerializeField]
        protected float walkFactor = 1f;
        [SerializeField]
        protected float runFactor = 2f;
        [SerializeField]
        private float dashConsume = 25f;
        [SerializeField]
        private float runConsumePerSecond = 10f;

        [ShowInInspector, ReadOnly]
        public CharacterMoveType MoveType { get; private set; } = CharacterMoveType.Walking;

        public UnityEvent<CharacterMoveType> OnMoveTypeChanged = new UnityEvent<CharacterMoveType>();
        public UnityEvent OnStopped = null;

        private ContinuousResource stamina = null;
        private bool fastMoveCancelled = false;

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
            if (MoveType == CharacterMoveType.Running
                && stamina != null)
            {
                float runConsume = runConsumePerSecond * Time.deltaTime;
                bool consumed = stamina.Consume(runConsume);
                if (!consumed)
                {
                    SetMoveType(CharacterMoveType.Walking);
                    stamina.RemoveAutomaticRegainBlocker();
                }
            }
        }

        public virtual void Move(Vector3 move) { }

        public virtual void PlanarMove(Vector3 move, float speedFactor) { }

        public virtual void MoveTo(Vector3 position) { }

        public void UpdateFastMove(bool fastMove, Vector3 direction, float speedFactor)
        {
            if (fastMove)
            {
                fastMoveCancelled = false;
                if (MoveType == CharacterMoveType.Walking
                    && stamina.HasEnough(dashConsume))
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
            stamina.Consume(dashConsume);
            SetMoveType(CharacterMoveType.Dashing);
            stamina.AddAutomaticRegainBlocker();
        }

        protected void DashCompleted()
        {
            if (fastMoveCancelled)
            {
                SetMoveType(CharacterMoveType.Walking);
                stamina.RemoveAutomaticRegainBlocker();
                return;
            }

            SetMoveType(CharacterMoveType.Running);
        }

        public virtual void Stop() { }

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
    }
}