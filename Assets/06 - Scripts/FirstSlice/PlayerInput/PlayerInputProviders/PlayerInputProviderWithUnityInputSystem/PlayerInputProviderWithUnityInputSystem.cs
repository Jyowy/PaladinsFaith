using Codice.Client.Common.GameUI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace FirstSlice.PlayerInput
{
    public class PlayerInputProviderWithUnityInputSystem : PlayerInputDataProvider
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
        private InputActionReference attack = null;

        [SerializeField]
        private InputActionReference rotation = null;

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
            attack = GetActionReference(inputActionMap, "Attack");
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

        public override PlayerInputData GetPlayerInputData()
        {
            Process();
            return playerInputData;
        }

        private void Process()
        {
            playerInputData.movement = move.action.ReadValue<Vector2>();
            playerInputData.cameraRotation = rotation.action.ReadValue<Vector2>();
            playerInputData.CheckAttackState();
        }

        private void OnEnable()
        {
            EnableInputAsset();
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

        private void InitializeInputMap()
        {
            inputActionMap = inputActionAsset.FindActionMap("PlayerInputMap");
        }

        private void SetupActionCallbacks()
        {
            AddPerformedCanceledCallbacks(run, OnRunStarted, OnRunFinished);
            AddPerformedCanceledCallbacks(defense, OnDefenseStarted, OnDefenseFinished);
            attack.action.performed += OnAttack;
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
            RemovePerformedCanceledCallbacks(run, OnRunStarted, OnRunFinished);
            RemovePerformedCanceledCallbacks(defense, OnDefenseStarted, OnDefenseFinished);
            attack.action.performed -= OnAttack;
        }

        private void RemovePerformedCanceledCallbacks(InputActionReference actionReference,
            Action<InputAction.CallbackContext> OnPerformed,
            Action<InputAction.CallbackContext> OnCanceled)
        {
            actionReference.action.performed -= OnPerformed;
            actionReference.action.canceled -= OnCanceled;
        }

        private void OnRunStarted(InputAction.CallbackContext _)
        {
            playerInputData.runMode = true;
        }

        private void OnRunFinished(InputAction.CallbackContext _)
        {
            playerInputData.runMode = false;
        }

        private void OnDefenseStarted(InputAction.CallbackContext _)
        {
            playerInputData.defenseActive = true;
        }

        private void OnDefenseFinished(InputAction.CallbackContext _)
        {
            playerInputData.defenseActive = false;
        }

        private void OnAttack(InputAction.CallbackContext _)
        {
            playerInputData.Attack();
        }
    }
}