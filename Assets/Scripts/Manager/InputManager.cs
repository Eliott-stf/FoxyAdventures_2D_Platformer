namespace Manager
{

    using UnityEngine;
    using UnityEngine.InputSystem;

    public class InputManager : MonoBehaviour
    {
        //On déclare nos variables
        public static PlayerInput PlayerInput;

        public static Vector2 Movement;

        //Bool pour gérer le 'AirTime' du player au saut
        public static bool JumpWasPressed;
        public static bool JumpIsHeld;
        public static bool JumpWasReleased;

        public static bool RunIsHeld;

        public static bool DashWasPressed;
        
        public static bool MenuWasPressed;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _runAction;
        private InputAction _dashAction;
        private InputAction _menuAction;

        private void Awake()
        {
            //On récupère le component 
            PlayerInput = GetComponent<PlayerInput>();

            _moveAction = PlayerInput.actions["Move"];
            _jumpAction = PlayerInput.actions["Jump"];
            _runAction = PlayerInput.actions["Run"];
            _dashAction = PlayerInput.actions["Dash"];
            _menuAction = PlayerInput.actions["Menu"];
        }

        // Update is called once per frame
        void Update()
        {
            Movement = _moveAction.ReadValue<Vector2>();

            JumpWasPressed = _jumpAction.WasPressedThisFrame();
            JumpIsHeld = _jumpAction.IsPressed();
            JumpWasReleased = _jumpAction.WasReleasedThisFrame();

            RunIsHeld = _runAction.IsPressed();

            DashWasPressed = _dashAction.WasPressedThisFrame();
            
            MenuWasPressed = _menuAction.WasPressedThisFrame();
        }
    }
}