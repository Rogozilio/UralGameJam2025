using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts
{
    public class Input : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;

        public bool PressLeftMove => inputActionAsset.FindAction("LeftMove").ReadValue<float>() > 0;
        public bool PressRightMove => inputActionAsset.FindAction("RightMove").ReadValue<float>() > 0;
        public bool PressForwardMove => inputActionAsset.FindAction("ForwardMove").ReadValue<float>() > 0;
        public bool PressBackMove => inputActionAsset.FindAction("BackMove").ReadValue<float>() > 0;

        private void OnEnable()
        {
            inputActionAsset.Enable();
        }

        private void OnDisable()
        {
            inputActionAsset.Disable();
        }
    }
}