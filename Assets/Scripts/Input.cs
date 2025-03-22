using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class Input : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;

        public float mouseSensitivity = 1f;
        public bool PressLeftMove => inputActionAsset.FindAction("LeftMove").ReadValue<float>() > 0;
        public bool PressRightMove => inputActionAsset.FindAction("RightMove").ReadValue<float>() > 0;
        public bool PressForwardMove => inputActionAsset.FindAction("ForwardMove").ReadValue<float>() > 0;
        public bool PressBackMove => inputActionAsset.FindAction("BackMove").ReadValue<float>() > 0;
        public Vector2 deltaMovePosition => inputActionAsset.FindAction("DeltaMouse").ReadValue<Vector2>();

        public Action OnAction;
        public Action OnActionEcs;

        private void OnEnable()
        {
            inputActionAsset.Enable();

            inputActionAsset.FindAction("Action").performed += (ctx) => { OnAction?.Invoke(); };
            inputActionAsset.FindAction("Ecs").performed += (ctx) => { OnActionEcs?.Invoke(); };
        }

        private void OnDisable()
        {
            inputActionAsset.FindAction("Action").performed -= (ctx) => { OnAction?.Invoke(); };
            inputActionAsset.FindAction("Ecs").performed -= (ctx) => { OnActionEcs?.Invoke(); };
            
            inputActionAsset.Disable();
        }

        public void ChangeMouseSensitivity(float value)
        {
            mouseSensitivity = value;
        }
    }
}