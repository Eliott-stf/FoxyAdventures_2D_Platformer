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
        public static bool InteractWasPressed;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _runAction;
        private InputAction _dashAction;
        private InputAction _menuAction;
        private InputAction _interactAction;

        private void Awake()
        {
            //On récupère le component 
            PlayerInput = GetComponent<PlayerInput>();
            
            //On set nos controls 
            _moveAction = PlayerInput.actions["Move"];
            _jumpAction = PlayerInput.actions["Jump"];
            _runAction = PlayerInput.actions["Run"];
            _dashAction = PlayerInput.actions["Dash"];
            _menuAction = PlayerInput.actions["Menu"];
            _interactAction = PlayerInput.actions["Interact"];
        }

        // Update is called once per frame
        void Update()
        {
            //on set pour nos inputs
            Movement = _moveAction.ReadValue<Vector2>();

            JumpWasPressed = _jumpAction.WasPressedThisFrame();
            JumpIsHeld = _jumpAction.IsPressed();
            JumpWasReleased = _jumpAction.WasReleasedThisFrame();

            RunIsHeld = _runAction.IsPressed();

            DashWasPressed = _dashAction.WasPressedThisFrame();
            
            MenuWasPressed = _menuAction.WasPressedThisFrame();

            InteractWasPressed = _interactAction.WasPressedThisFrame();
        }
    }
}