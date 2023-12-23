using Pizza.GameStateSystem;
using System.Linq;
using UnityEngine;

namespace Pizza.CharacterSystem
{
    /// <summary>
    /// A base class for character controlling.
    /// </summary>
    /// <remarks>
    /// To use this component, create a chile class and provide input via SetInput.
    /// </remarks>
    public abstract class BaseCharacterController : MonoBehaviour
    {
        /// <summary>
        /// The animator layer to play states on.
        /// </summary>
        protected const int ANIM_LAYER = 0;

        /// <summary>
        /// The name of the animator paramter for Vertical
        /// </summary>
        protected const string ANIM_PARAM_VERTICAL = "Vertical";

        /// <summary>
        /// The name of the animator paramter for Horizontal
        /// </summary>
        protected const string ANIM_PARAM_HORIZONTAL = "Horizontal";

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Even though it shows at 0 in the UI, it starts with 1.
        /// </remarks>
        private const int DEFAULT_LAYER_MASK = 1;

        /// <summary>
        /// An array of transport configurations to use
        /// </summary>
        [SerializeField]
        protected MovementTransportConfig[] _transportConfigs;

        /// <summary>
        /// A reference to the game state SO
        /// </summary>
        [SerializeField]
        protected GameState _gameState;

        /// <summary>
        /// A reference to the character animator component
        /// </summary>
        [SerializeField]
        protected Animator _animator;

        /// <summary>
        /// A reference to the character rigibody component
        /// </summary>
        [SerializeField]
        protected Rigidbody2D _rigidbody;

        /// <summary>
        /// A reference to the character collider component
        /// </summary>
        [SerializeField]
        protected Collider2D _collider;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        private Vector2 _rigidbodyCastPositionOffset = new Vector2(0.0f, 0.3f);

        /// <summary>
        /// The currently cached movement input being applied to the character.
        /// </summary>
        /// <remarks>
        /// This might be player input, AI input, cutscene input. It depends on who called SetInput.
        /// </remarks>
        protected Vector2 _movementInput = Vector2.zero;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 _movementInputLineCast = Vector2.zero;

        /// <summary>
        /// The current direction the character is facing (enum)
        /// </summary>
        protected MovementDirection _movementDirection = MovementDirection.Down;

        /// <summary>
        /// The current direction the character is facing
        /// </summary>
        protected Vector2 _movementDirectionValue = Vector2.zero;

        /// <summary>
        /// The currently moving speed (enum)
        /// </summary>
        protected MovementSpeed _movementSpeed = MovementSpeed.Idle;

        /// <summary>
        /// The currently moving speed.
        /// </summary>
        protected float _movementSpeedValue = 0.0f;

        /// <summary>
        /// The moving speed to use when movement starts.
        /// </summary>
        protected MovementSpeed _preferredMovingSpeed = MovementSpeed.Slow;

        /// <summary>
        /// The current movement transport for the character. (enum)
        /// </summary>
        protected MovementTransport _movementTransport = MovementTransport.Normal;

        /// <summary>
        /// The current movement transport for the character.
        /// </summary>
        protected MovementTransportConfig _movementTransportValue;

        /// <summary>
        /// The position to do raycasts from based on rigidbody position
        /// </summary>
        private Vector2 _rigidbodyCastPosition = Vector2.zero;

        /// <summary>
        /// States if the character is moving or not.
        /// </summary>
        public bool IsMoving => _movementSpeed != MovementSpeed.Idle;

        private bool _playingCustomState;
        private bool _lineCastResult;
        private Vector2 _lineCastTargetPosition;
        private LayerMask _colliderLayerMask;

        protected void OnValidate()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();

            if (_animator == null)
                _animator = GetComponent<Animator>();

            if (_colliderLayerMask == 0)
                _colliderLayerMask = LayerMask.GetMask("Default");

            if (_collider == null)
                _collider = GetComponent<Collider2D>();
        }

        protected void Start()
        {
            //
            _lineCastTargetPosition = _rigidbody.position + _movementInputLineCast;

            // Apply the default values
            SetMovementTransport(MovementTransport.Normal);
            SetPreferredMovementSpeed(MovementSpeed.Slow);
            SetCustomMovementDirection(MovementDirection.Down);
        }

        protected void Update()
        {
            Debug.DrawLine(_rigidbodyCastPosition, _lineCastTargetPosition, Color.red);
        }

        protected void FixedUpdate()
        {
            if (_movementSpeed != MovementSpeed.Idle)
            {
                _rigidbodyCastPosition = _rigidbody.position + _rigidbodyCastPositionOffset;
                _lineCastTargetPosition = _rigidbodyCastPosition + _movementInputLineCast;

                // Do a linecast check for collision
                _lineCastResult = false;
                var raycastHits = Physics2D.LinecastAll(_rigidbodyCastPosition, _lineCastTargetPosition, DEFAULT_LAYER_MASK);
                foreach (var hit in raycastHits)
                {
                    // If this is our own collider, move on
                    if (hit.collider == _collider)
                        continue;

                    // As soon as we hit something that is not our own collider, return.
                    _lineCastResult = true;
                    break;
                }

                // Only allow movement if our raycast does not detect a blocking object
                if (_lineCastResult == false)
                {
                    _rigidbody.MovePosition(_rigidbody.position + (_movementInput * _movementSpeedValue * Time.fixedDeltaTime));
                }
            }
        }

        /// <summary>
        /// Updates the animator based on current states.
        /// </summary>
        protected void UpdateAnimator()
        {
            // Decide on state name based on our current speed
            string animStateName = _movementTransportValue.GetStateName(_movementSpeed);

            // Check to see if the animator is in the state we want to be in
            if (_playingCustomState == false && _animator.GetCurrentAnimatorStateInfo(ANIM_LAYER).IsName(animStateName) == false)
            {
                _animator.Play(animStateName, ANIM_LAYER);
            }

            // Always update the floats based on our current direction state
            _animator.SetFloat(ANIM_PARAM_HORIZONTAL, _movementDirectionValue.x);
            _animator.SetFloat(ANIM_PARAM_VERTICAL, _movementDirectionValue.y);
        }

        /// <summary>
        /// Sets the active movement transport. This will update the character.
        /// </summary>
        /// <param name="movementTransport">The desired transport</param>
        protected void SetMovementTransport(MovementTransport movementTransport)
        {
            // Set the motor config based on the new transport state
            _movementTransport = movementTransport;
            _movementTransportValue = _transportConfigs.Where(mc => mc.TransportState == _movementTransport).First();

            // Update the animations based on the new move state
            UpdateAnimator();
        }

        /// <summary>
        /// Sets the preferred movement speed to use when movement starts.
        /// </summary>
        /// <param name="movementSpeed">The desired speed</param>
        protected void SetPreferredMovementSpeed(MovementSpeed movementSpeed)
        {
            _preferredMovingSpeed = movementSpeed;
            UpdateMovementSpeed();
        }

        /// <summary>
        /// A method to override the current movement direction.
        /// </summary>
        /// <remarks>
        /// This should only be used in cutscenes or to apply default values.
        /// </remarks>
        /// <param name="movementDirection">The custom direction</param>
        protected void SetCustomMovementDirection(MovementDirection movementDirection)
        {
            // Override our direciton enum state
            _movementDirection = movementDirection;

            // Override our direction vector (used by animator)
            _movementDirectionValue = movementDirection switch
            {
                MovementDirection.Up => Vector2.up,
                MovementDirection.Right => Vector2.right,
                MovementDirection.Down => Vector2.down,
                MovementDirection.Left => Vector2.left,
                _ => Vector2.down
            };

            // Update the animations based on the new move state
            UpdateAnimator();
        }

        protected void SetCustomState(string state)
        {
            _playingCustomState = true;
            _animator.Play(state);
        }

        protected void ClearCustomState()
        {
            _playingCustomState = false;
        }

        /// <summary>
        /// Applies input to the character controller. This will move the character.
        /// </summary>
        /// <param name="movementInput">A vector 2 of movement input. X = horizontal, Y = Vertical.</param>
        protected void SetInput(Vector2 movementInput)
        {
            // Cache our raw input
            _movementInput = movementInput;
            _movementInputLineCast = movementInput * 0.5f;

            // Update our internal states
            UpdateMovementDirection();
            UpdateMovementSpeed();

            // Update the animations based on the new input
            UpdateAnimator();
        }

        /// <summary>
        /// Update the movement direction based on current user input
        /// </summary>
        /// <remarks>
        /// This is a helper function for SetInput.
        /// </remarks>
        private void UpdateMovementDirection()
        {
            // Store an enum version of the direction
            if (_movementInput == Vector2.up)
                _movementDirection = MovementDirection.Up;
            else if (_movementInput == Vector2.down)
                _movementDirection = MovementDirection.Down;
            else if (_movementInput == Vector2.left)
                _movementDirection = MovementDirection.Left;
            else if (_movementInput == Vector2.right)
                _movementDirection = MovementDirection.Right;

            // Store a vector 2 version of the direction
            if (_movementInput != Vector2.zero)
                _movementDirectionValue = _movementInput;
        }

        /// <summary>
        /// Update the movement speed based on current user input
        /// </summary>
        /// <remarks>
        /// This is a helper function for SetInput.
        /// </remarks>
        private void UpdateMovementSpeed()
        {
            // If we are not moving, state we are idle
            if (_movementInput == Vector2.zero)
                _movementSpeed = MovementSpeed.Idle;

            // Else use the preffereed moving state.
            else
                _movementSpeed = _preferredMovingSpeed;

            // Get the speed based on the current speed state
            _movementSpeedValue = _movementTransportValue.GetSpeed(_movementSpeed);
        }

#if UNITY_EDITOR
        protected void OnGUI()
        {
            if (UnityEditor.Selection.activeGameObject != this.gameObject)
                return;

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Stats", PizzaOnGUI.HeaderLabel);
            GUILayout.Label($"Direction State: {_movementDirection}", PizzaOnGUI.NormalLabel);
            GUILayout.Label($"Speed State: {_movementSpeed}", PizzaOnGUI.NormalLabel);
            GUILayout.Label($"Transport State: {_movementTransport}", PizzaOnGUI.NormalLabel);
            GUILayout.Label($"Movement Input: {_movementInput}", PizzaOnGUI.NormalLabel);
            GUILayout.Label($"Move Speed: {_movementSpeed}", PizzaOnGUI.NormalLabel);

            if (_movementTransportValue is null)
                GUILayout.Label("Transport State SO is null!", PizzaOnGUI.RedLabel);

            GUILayout.Space(10);
            if (GUILayout.Button("Slow", PizzaOnGUI.NormalButton))
                SetPreferredMovementSpeed(MovementSpeed.Slow);
            if (GUILayout.Button("Fast", PizzaOnGUI.NormalButton))
                SetPreferredMovementSpeed(MovementSpeed.Fast);

            GUILayout.Space(10);
            GUILayout.Label($"Line Cast: {_rigidbodyCastPosition} - {_lineCastTargetPosition}", PizzaOnGUI.NormalLabel);
            GUILayout.Label($"Line Cast Result: {_lineCastResult}", PizzaOnGUI.NormalLabel);

            GUILayout.EndVertical();
        }
#endif
    }
}