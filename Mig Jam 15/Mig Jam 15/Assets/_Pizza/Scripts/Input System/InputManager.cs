using UnityEngine;

namespace Pizza.InputSystem
{
    /// <summary>
    /// A manager SO that handles the input asset.
    /// </summary>
    [CreateAssetMenu(fileName = "New Input Manager", menuName = "Pizza/Input Manager")]
    public class InputManager : ScriptableObject
    {
        /// <summary>
        /// An enum representing the avaialable input maps.
        /// </summary>
        public enum InputMaps
        {
            Player,
            UI
        }

        /// <summary>
        /// The current focused input map.
        /// </summary>
        /// <remarks>
        ///  The focused input map decided which input map is enabled/disabled
        ///  </remarks>
        [SerializeField]
        [Tooltip("To change this value while the game is runing, right click -> Update Action Maps.")]
        private InputMaps _focusedInputMap;

        /// <summary>
        /// 
        /// </summary>
        private PlayerInputActions _inputActions;

        /// <summary>
        /// 
        /// </summary>
        public PlayerInputActions InputActions { get { return _inputActions; } }

        private void OnEnable()
        {
            // Note: SO OnEnable is called before scene assets Awake()

            _inputActions = new PlayerInputActions();

            // Default to UI input only because a majority of the game uses UI at the start
            SetInputFocus(InputMaps.UI);
        }

        private void OnDisable()
        {
            _inputActions.Player.Disable();
            _inputActions.UI.Disable();
        }

        /// <summary>
        /// Set the focus input map. This enables/disables the maps.
        /// </summary>
        /// <param name="inputMaps">The input map to focus on.</param>
        public void SetInputFocus(InputMaps inputMaps)
        {
            _focusedInputMap = inputMaps;
            UpdateActionMaps();
        }

        /// <summary>
        /// Enables/Disables the input maps based on the focused input.
        /// </summary>
        [ContextMenu("Update Action Maps")]
        private void UpdateActionMaps()
        {
            switch (_focusedInputMap)
            {
                case InputMaps.UI:
                    _inputActions.UI.Enable();
                    _inputActions.Player.Disable();
                    break;
                case InputMaps.Player:
                    _inputActions.Player.Enable();
                    _inputActions.UI.Disable();
                    break;
            }
        }
    }
}