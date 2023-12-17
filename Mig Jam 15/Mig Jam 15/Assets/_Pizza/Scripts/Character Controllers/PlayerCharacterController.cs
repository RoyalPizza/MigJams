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
        }

        private new void Start()
        {
            base.Start();
        }

        private void OnDestroy()
        {
            _inputManager.InputActions.Player.Move.performed -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var _inputDirection = context.ReadValue<Vector2>();

            // Because the read value will return values for both axis, limit to one for our player movement.
            if (_inputDirection.x != 0.0f & _inputDirection.y != 0.0f)
                _inputDirection.x = 0.0f;

            SetInput(_inputDirection);
        }
    }
}