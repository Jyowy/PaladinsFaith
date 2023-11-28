using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstSlice.PlayerInput;
using UnityEngine.Events;

namespace FirstSlice.Player
{
    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Defending,
        Attacking
    }

    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputDataProvider inputDataProvider = null;
        [SerializeField]
        private PlayerDataProvider dataProvider = null;
        [SerializeField]
        private new PlayerCamera camera = null;
        [SerializeField]
        private PlayerMoveModule moveModule = null;
        [SerializeField]
        private PlayerCombatModule combatModule = null;

        [SerializeField]
        private float defendingMovePenalizer = 0.5f;

        public UnityEvent<PlayerMoveMode> OnPlayerMoveModeChanged = null;
        public UnityEvent OnPlayerStopped = null;

        private PlayerState currentState = PlayerState.Idle;
        private PlayerMoveMode currentMoveMode = PlayerMoveMode.None;
        private float currentSpeed = 0f;

        private void FixedUpdate()
        {
            ProcessPhysicsData();
        }

        private void ProcessPhysicsData()
        {
            PlayerInputData playerInputData = inputDataProvider.GetPlayerInputData();
            PlayerData playerData = dataProvider.GetPlayerData();

            UpdateMoveMode(playerInputData.runMode);
            UpdateMove(playerInputData.movement);

            UpdateCameraRotation(playerInputData.cameraRotation);

            UpdateCombat(playerInputData);
        }

        private void UpdateMoveMode(bool run)
        {
            PlayerMoveMode newMoveMode = run
                ? PlayerMoveMode.Running
                : PlayerMoveMode.Walking;
            moveModule.SetMoveMode(newMoveMode);

            if (newMoveMode != currentMoveMode
                && currentSpeed > 0f)
            {
                OnPlayerMoveModeChanged?.Invoke(newMoveMode);
            }

            currentMoveMode = newMoveMode;

            if (currentState != PlayerState.Defending
                && currentState != PlayerState.Attacking)
            {
                ForceMoveState();
            }
        }

        private void ForceMoveState()
        {
            PlayerState newState;

            if (currentSpeed > 0f)
            {
                newState = currentMoveMode == PlayerMoveMode.Running
                    ? PlayerState.Running
                    : PlayerState.Walking;
            }
            else
            {
                newState = PlayerState.Idle;
            }

            if (newState == currentState)
            {
                return;
            }

            currentState = newState;
            if (newState == PlayerState.Idle)
            {
                OnPlayerStopped?.Invoke();
            }
            else
            {
                OnPlayerMoveModeChanged?.Invoke(currentMoveMode);
            }
        }

        private void UpdateMove(Vector2 move)
        {
            Vector3 worldMove = GetWorldDirectionFrom2DInput(move);
            if (currentState == PlayerState.Defending)
            {
                worldMove *= defendingMovePenalizer;
            }
            moveModule.PlanarMove(worldMove);

            float speed = move.magnitude;
            if (speed == 0f
                && currentSpeed > 0f)
            {
                OnPlayerStopped?.Invoke();
                if (currentState == PlayerState.Walking
                    || currentState == PlayerState.Running)
                {
                    currentState = PlayerState.Idle;
                }
            }
            else if (speed > 0f
                && currentSpeed == 0f)
            {
                OnPlayerMoveModeChanged?.Invoke(currentMoveMode);
            }

            currentSpeed = speed;
        }

        private Vector3 GetWorldDirectionFrom2DInput(Vector2 input2D)
        {
            Vector3 planarDirection = new Vector3(input2D.x, 0f, input2D.y);
            Vector3 worldDirection = camera.GetVectorRelativeToView(planarDirection);
            Vector3 normalizedWorldDirection = worldDirection.NormalizedWithoutY();
            return normalizedWorldDirection;
        }

        private void UpdateCameraRotation(Vector2 cameraRotation)
        {
            float angle = cameraRotation.x;
            camera.HorizontalRotation(angle);

            float zoom = -cameraRotation.y;
            camera.VerticalRotation(zoom);
        }

        private void UpdateCombat(PlayerInputData inputData)
        {
            if (currentState != PlayerState.Attacking)
            {
                if (inputData.AttackRequested)
                {
                    inputData.ConsumeAttack();
                    combatModule.Attack();
                }
                else if (inputData.defenseActive
                    && currentState != PlayerState.Defending)
                {
                    currentState = PlayerState.Defending;
                    combatModule.StartDefending();
                }
                else if (currentState == PlayerState.Defending
                    && !inputData.defenseActive)
                {
                    combatModule.StopDefending();
                    ForceMoveState();
                }
            }
        }

        public void AttackFinished()
        {
            if (currentState != PlayerState.Attacking)
            {
                return;
            }

            ForceMoveState();
        }
    }
}