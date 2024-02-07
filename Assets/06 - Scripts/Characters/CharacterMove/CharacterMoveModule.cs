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

        [ShowInInspector, ReadOnly]
        protected CharacterMoveType moveType = CharacterMoveType.Walking;

        public UnityEvent<CharacterMoveType> OnMoveTypeChanged = new UnityEvent<CharacterMoveType>();
        public UnityEvent OnStopped = null;

        protected virtual void Start()
        {
            SetMoveType(defaultMoveType);
        }

        public void SetMoveType(CharacterMoveType moveType)
        {
            this.moveType = moveType;
            OnMoveTypeChanged?.Invoke(moveType);
        }

        public virtual void Move(Vector3 move) { }

        public virtual void PlanarMove(Vector3 move) { }

        public virtual void MoveTo(Vector3 position) { }

        public virtual void Stop() { }

        public virtual void LookAt(Vector3 position) { }

        protected float GetMoveModeFactor()
        {
            float factor = moveType switch
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