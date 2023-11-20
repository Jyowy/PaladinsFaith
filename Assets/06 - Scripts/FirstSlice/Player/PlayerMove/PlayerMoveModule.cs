using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstSlice.Player
{
    public abstract class PlayerMoveModule : MonoBehaviour
    {
        public enum PlayerMoveMode
        {
            Walking,
            Running
        }

        protected PlayerMoveMode MoveMode { get; private set; } = PlayerMoveMode.Walking;

        protected void Awake()
        {
            MoveMode = PlayerMoveMode.Walking;
        }

        [Button]
        public abstract void PlanarMove(Vector3 worldDirection);

        public void SetMoveMode(PlayerMoveMode moveMode)
        {
            MoveMode = moveMode;
        }
    }
}
