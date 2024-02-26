using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PaladinsFaith.Input
{
    public class PlayerInputProviderWithInputActions : PlayerInputDataProvider
    {
        [SerializeField, OnValueChanged("ReadActionMap")]
        private InputActionAsset inputActionAsset = null;

        private InputActionMap inputActionMap = null;

        [SerializeField]
        private InputActionReference move = null;
        [SerializeField]
        private InputActionReference run = null;
        [SerializeField]
        private InputActionReference defense = null;
        [SerializeField]
        private InputActionReference lightAttack = null;
        [SerializeField]
        private InputActionReference heavyAttack = null;

        [SerializeField]
        private InputActionReference rotation = null;

        [SerializeField]
        private InputActionReference spell = null;
        [SerializeField]
        private InputActionReference prevSpell = null;
        [SerializeField]
        private InputActionReference nextSpell = null;

        [SerializeField]
        private InputActionReference interact = null;

        [ShowInInspector, ReadOnly]
        private PlayerInputData playerInputData = null;

#if UNITY_EDITOR
        [Button]
        private void ReadActionMap()
        {
            if (inputActionAsset == null)
            {
                ClearActions();
                return;
            }

            InitializeInputMap();
            move = GetActionReference(inputActionMap, "Move");
            run = GetActionReference(inputActionMap, "Run");
            rotation = GetActionReference(inputActionMap, "CameraRotation");
            defense = GetActionReference(inputActionMap, "Defense");
            lightAttack = GetActionReference(inputActionMap, "LightAttack");
            heavyAttack = GetActionReference(inputActionMap, "HeavyAttack");
            spell = GetActionReference(inputActionMap, "Spell");
            prevSpell = GetActionReference(inputActionMap, "PrevSpell");
            nextSpell = GetActionReference(inputActionMap, "NextSpell");
            interact = GetActionReference(inputActionMap, "Interact");
        }

        [Button]
        private void ClearActions()
        {
            move = null;
            run = null;
            rotation = null;
        }

        private InputActionReference GetActionReference(InputActionMap actionMap, string actionName)
        {
            InputAction action = actionMap.FindAction(actionName);
            InputActionReference reference = InputActionReference.Create(action);
            return reference;
        }
#endif

        public override PlayerInputData UpdateAndGetPlayerInputData()
        {
            Process();
            return playerInputData;
        }

        private void Process()
        {
            playerInputData.move = move.action.ReadValue<Vector2>();
            playerInputData.cameraRotation = rotation.action.ReadValue<Vector2>();
            playerInputData.Update();
        }

        private void OnEnable()
        {
            EnableInputAsset();
        }

        private void InitializeInputMap()
        {
            inputActionMap = inputActionAsset.FindActionMap("PlayerInputMap");
        }

        private void EnableInputAsset()
        {
            inputActionAsset.Enable();
            if (inputActionMap == null)
            {
                InitializeInputMap();
            }
            inputActionMap.Enable();
        }

        private void OnDisable()
        {
            DisableInputAsset();
        }

        private void DisableInputAsset()
        {
            inputActionAsset.Disable();
            inputActionMap.Disable();
        }

        private void Awake()
        {
            InitializeData();
            InitializeInputMap();
            SetupActionCallbacks();
        }

        private void InitializeData()
        {
            playerInputData = new PlayerInputData();
        }

        private void SetupActionCallbacks()
        {
            AddPerformedCanceledCallbacks(run, OnFastMoveStarted, OnFastMoveFinished);
            AddPerformedCanceledCallbacks(defense, OnDefenseStarted, OnDefenseFinished);
            AddPerformedCanceledCallbacks(heavyAttack, OnHeavyAttackStarted, OnHeavyAttackFinished);

            lightAttack.action.performed += OnLightAttack;
            prevSpell.action.performed += OnPrevSpell;
            nextSpell.action.performed += OnNextSpell;
            spell.action.performed += OnSpell;
            interact.action.performed += OnInteract;
        }

        private void AddPerformedCanceledCallbacks(InputActionReference actionReference,
            Action<InputAction.CallbackContext> OnPerformed,
            Action<InputAction.CallbackContext> OnCanceled)
        {
            actionReference.action.performed += OnPerformed;
            actionReference.action.canceled += OnCanceled;
        }

        private void OnDestroy()
        {
            RemoveActionCallbacks();
        }

        private void RemoveActionCallbacks()
        {
            RemovePerformedCanceledCallbacks(run, OnFastMoveStarted, OnFastMoveFinished);
            RemovePerformedCanceledCallbacks(defense, OnDefenseStarted, OnDefenseFinished);
            RemovePerformedCanceledCallbacks(heavyAttack, OnHeavyAttackStarted, OnHeavyAttackFinished);

            lightAttack.action.performed -= OnLightAttack;
            prevSpell.action.performed -= OnPrevSpell;
            nextSpell.action.performed -= OnNextSpell;
            spell.action.performed -= OnSpell;
            interact.action.performed -= OnInteract;
        }

        private void RemovePerformedCanceledCallbacks(InputActionReference actionReference,
            Action<InputAction.CallbackContext> OnPerformed,
            Action<InputAction.CallbackContext> OnCanceled)
        {
            actionReference.action.performed -= OnPerformed;
            actionReference.action.canceled -= OnCanceled;
        }

        private void OnFastMoveStarted(InputAction.CallbackContext _)
        {
            playerInputData.fastMove = true;
        }

        private void OnFastMoveFinished(InputAction.CallbackContext _)
        {
            playerInputData.fastMove = false;
        }

        private void OnDefenseStarted(InputAction.CallbackContext _)
        {
            playerInputData.defenseActive = true;
        }

        private void OnDefenseFinished(InputAction.CallbackContext _)
        {
            playerInputData.defenseActive = false;
        }

        private void OnLightAttack(InputAction.CallbackContext _)
        {
            playerInputData.LightAttack();
        }

        private void OnHeavyAttackStarted(InputAction.CallbackContext _)
        {
            playerInputData.heavyAttack = false;
        }

        private void OnHeavyAttackFinished(InputAction.CallbackContext _)
        {
            playerInputData.heavyAttack = false;
        }

        private void OnPrevSpell(InputAction.CallbackContext _)
        {
            playerInputData.prevSpell.Activate();
        }

        private void OnNextSpell(InputAction.CallbackContext _)
        {
            playerInputData.prevSpell.Activate();
        }

        private void OnSpell(InputAction.CallbackContext _)
        {
            playerInputData.Spell();
        }

        private void OnInteract(InputAction.CallbackContext _)
        {
            playerInputData.Interact();
        }
    }
}