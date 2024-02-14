using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using PaladinsFaith.PlayerInput;
using PaladinsFaith.Characters;
using PaladinsFaith.Spells;
using PaladinsFaith.Dialogs;

using Sirenix.OdinInspector;

namespace PaladinsFaith.Player
{
    public enum PlayerMode
    {
        ControllingCharacter,
        Dialog,
        Cinematic
    }

    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Dashing,
        Casting
    }

    public class Player : CombatantCharacter
    {
        [SerializeField]
        private PlayerInputDataProvider inputDataProvider = null;
        [SerializeField]
        private PlayerDataProvider dataProvider = null;
        [SerializeField]
        private new PlayerCamera camera = null;
        [SerializeField]
        private SpellModule spellModule = null;

        [SerializeField]
        private float defendingMovePenalizer = 0.5f;

        public UnityEvent<CharacterMoveType> OnPlayerMoveModeChanged = null;
        public UnityEvent OnPlayerStopped = null;

        private PlayerSharedData sharedData = new PlayerSharedData();
        [ShowInInspector, ReadOnly]
        private PlayerMode playerMode = PlayerMode.ControllingCharacter;
        [ShowInInspector, ReadOnly]
        private PlayerState currentState = PlayerState.Idle;
        [ShowInInspector, ReadOnly]
        private CharacterMoveType currentMoveMode = CharacterMoveType.Walking;
        private float currentSpeed = 0f;

        private readonly List<Interactable> availableInteractables = new List<Interactable>();

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            healthBar.Initialize(OnDead);
            stamina.Fill();

            InitializeSharedData();
        }

        private void InitializeSharedData()
        {
            sharedData.camera = camera;
            sharedData.stamina = stamina;
            sharedData.moveModule = moveModule;
            sharedData.combatModule = combatModule;
            sharedData.spellModule = spellModule;
        }

        public void SetPlayerMode(PlayerMode playerMode)
        {
            this.playerMode = playerMode;

            if (playerMode == PlayerMode.Dialog)
            {
                StopPlayer();
            }
        }

        private void StopPlayer()
        {
            moveModule.Stop();
            currentSpeed = 0f;
        }

        public void SetNormalMode()
        {
            SetPlayerMode(PlayerMode.ControllingCharacter);
        }

        public void SetDialogMode()
        {
            if (playerMode == PlayerMode.Cinematic)
            {
                return;
            }

            SetPlayerMode(PlayerMode.Dialog);
        }

        public void SetCinematicMode()
        {
            SetPlayerMode(PlayerMode.Cinematic);
        }

        protected override void Update()
        {
            base.Update();
            ProcessInteraction();
        }

        private void ProcessInteraction()
        {
            PlayerInputData playerInputData = inputDataProvider.UpdateAndGetPlayerInputData();

            if (playerMode == PlayerMode.ControllingCharacter)
            {
                if (playerInputData.IsInteractActive()
                    && CanInteract())
                {
                    playerInputData.ConsumeInteract();
                    Interact();
                }
            }
            else if (playerMode == PlayerMode.Dialog)
            {
                if (playerInputData.IsInteractActive())
                {
                    playerInputData.ConsumeInteract();
                    DialogPlayer.CompleteLine();
                }
            }
            else if (playerMode == PlayerMode.Cinematic)
            {
                // Nothing yet
            }
        }

        private bool CanInteract()
        {
            bool canInteract = availableInteractables.Count > 0;
            return canInteract;
        }

        private void Interact()
        {
            availableInteractables[0].Interact();
        }

        private void OnTriggerEnter(Collider other)
        {
            Interactable interactable = GetInteractable(other.gameObject);
            if (interactable != null
                && !availableInteractables.Contains(interactable))
            {
                availableInteractables.Add(interactable);
                interactable.InRangeOfInteraction();
            }
        }

        private Interactable GetInteractable(GameObject gameObject)
        {
            return gameObject.GetComponent<Interactable>();
        }

        private void OnTriggerExit(Collider other)
        {
            Interactable interactable = GetInteractable(other.gameObject);
            if (interactable != null
                && availableInteractables.Contains(interactable))
            {
                availableInteractables.Remove(interactable);
                interactable.OutRangeOfInteraction();
            }
        }

        private void FixedUpdate()
        {
            ProcessPhysicsData();
        }

        private bool CharacterControlBlocked()
        {
            return playerMode != PlayerMode.ControllingCharacter;
        }

        private void ProcessPhysicsData()
        {
            PlayerInputData playerInputData = inputDataProvider.UpdateAndGetPlayerInputData();
            PlayerData playerData = dataProvider.UpdateAndGetPlayerData(this);

            if (CharacterControlBlocked())
            {
                return;
            }

            UpdateMove(playerInputData.fastMove, playerInputData.move);

            UpdateCameraRotation(playerInputData.cameraRotation);

            UpdateCombat(playerInputData);

            UpdateMagic(playerInputData);
        }

        private void UpdateMove(bool fastMove, Vector2 move)
        {
            Vector3 worldMove = GetWorldDirectionFrom2DInput(move);
            float speedFactor = 1f;
            if (combatModule.IsDefending)
            {
                speedFactor = defendingMovePenalizer;
            }

            CheckFastMove(fastMove, worldMove, speedFactor);
            moveModule.PlanarMove(worldMove, speedFactor);

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
            Vector3 relativeWorldDirection = camera.GetVectorRelativeToView(planarDirection);
            Vector3 normalizedWorldDirection = relativeWorldDirection.NormalizedWithoutY();
            Vector3 worldDirection = normalizedWorldDirection * planarDirection.magnitude;
            return worldDirection;
        }

        private void CheckFastMove(bool fastMove, Vector3 worldMove, float speedFactor)
        {
            moveModule.UpdateFastMove(fastMove, worldMove, speedFactor);
        }

        private void SetMoveState()
        {
            PlayerState newState;

            if (currentSpeed > 0f)
            {
                newState = currentMoveMode == CharacterMoveType.Running
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

        private void UpdateCameraRotation(Vector2 cameraRotation)
        {
            float angle = cameraRotation.x;
            camera.HorizontalRotation(angle);

            float zoom = -cameraRotation.y;
            camera.VerticalRotation(zoom);
        }

        private void UpdateCombat(PlayerInputData inputData)
        {
            if (!combatModule.IsAttacking)
            {
                if (inputData.IsLightAttackActive())
                {
                    inputData.ConsumeLightAttack();
                    combatModule.Attack();
                }
                else if (inputData.defenseActive
                    && !combatModule.IsDefending)
                {
                    combatModule.StartDefending();
                }
                else if (combatModule.IsDefending
                    && !inputData.defenseActive)
                {
                    combatModule.StopDefending();
                    SetMoveState();
                }
            }
            else
            {
                if (inputData.IsLightAttackActive())
                {
                    inputData.ConsumeLightAttack();
                    combatModule.Attack();
                }
            }
        }

        public void AttackFinished()
        {
            if (!combatModule.IsAttacking)
            {
                return;
            }

            SetMoveState();
        }

        private void UpdateMagic(PlayerInputData inputData)
        {
            if (inputData.IsPrevSpellRequested())
            {
                inputData.ConsumePrevSpellRequest();
                spellModule.PrevSpell();
            }
            else if (inputData.IsNextSpellRequested())
            {
                inputData.ConsumeNextSpellRequest();
                spellModule.NextSpell();
            }

            if (!combatModule.IsAttacking)
            {
                if (inputData.IsSpellActive())
                {
                    inputData.ConsumeSpell();
                    CastSpell();
                }
            }
        }

        private void CastSpell()
        {
            spellModule.CastSpell();
        }

        public void MagicCastFinished()
        {
            if (currentState != PlayerState.Casting)
            {
                return;
            }

            SetMoveState();
        }

        public override void ReceiveAttack(Attack attack)
        {
            if (combatModule.IsDefending)
            {
                return;
            }

            base.ReceiveAttack(attack);
        }

        private void OnDead()
        {
            Debug.Log($"GameOver");

            gameObject.SetActive(false);
        }
    }
}