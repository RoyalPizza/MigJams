using Pizza.SimpleInput;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pizza.CharacterControl
{
    /// <summary>
    /// A character controller that accepts player input.
    /// </summary>
    public class PlayerCharacterController : BaseCharacterController
    {
        [SerializeField]
        private InputManager _inputManager;

        private void Awake()
        {
            Debug.LogWarning("TODO: Remove this defaulting to player input.");
            _inputManager.SetInputFocus(InputManager.InputMaps.Player);

            // Make sure we unsubscribe because the player is created/destoryed 
            _inputManager.InputActions.Player.Move.performed += OnMove;
            _inputManager.InputActions.Player.Run.performed += OnRun;
        }

        private new void Start()
        {
            base.Start();
        }

        private void OnDestroy()
        {
            _inputManager.InputActions.Player.Move.performed -= OnMove;
            _inputManager.InputActions.Player.Run.performed -= OnRun;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Player Collision Enter");
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Debug.Log("Player Collision Exit");
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log("Player Collision Stay");
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var inputDirection = context.ReadValue<Vector2>();

            // Because the read value will return values for both axis, limit to one for our player movement.
            if (inputDirection.x != 0.0f & inputDirection.y != 0.0f)
                inputDirection.x = 0.0f;

            SetInput(inputDirection);
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            bool isRunning = context.ReadValueAsButton();
            MovementSpeed movementSpeed = (isRunning == true) ? MovementSpeed.Fast : MovementSpeed.Slow;
            SetPreferredMovementSpeed(movementSpeed);
        }
    }
}