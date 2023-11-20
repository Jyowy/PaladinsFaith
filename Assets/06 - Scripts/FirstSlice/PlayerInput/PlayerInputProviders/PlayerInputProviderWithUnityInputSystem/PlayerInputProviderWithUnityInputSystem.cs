using Codice.Client.Common.GameUI;
using Sirenix.OdinInspector;
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
            run.action.performed += OnRunStarted;
            run.action.canceled += OnRunFinished;
        }

        private void OnDestroy()
        {
            RemoveActionCallbacks();
        }

        private void RemoveActionCallbacks()
        {
            run.action.performed -= OnRunStarted;
            run.action.canceled -= OnRunFinished;
        }

        private void OnRunStarted(InputAction.CallbackContext _)
        {
            playerInputData.run = true;
        }

        private void OnRunFinished(InputAction.CallbackContext _)
        {
            playerInputData.run = false;
        }
    }
}