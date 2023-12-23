using UnityEngine;

namespace Pizza.CharacterSystem
{
    public class SimpleEnemyCharacterController : BaseCharacterController, ICharacterVision
    {
        [SerializeField]
        private Transform[] _targets;

        [SerializeField]
        private bool _moveCharacter = true;

        [SerializeField]
        private CharacterVision _characterVision;

        private int _currentTarget = 0;
        private float _distanceToTarget;
        private float _distanceToTargetGoal = 0.5f;

        private Transform _enemyTarget;
        private Vector2 _enemyTargetDirection;


#if UNITY_EDITOR
        private new void OnValidate()
        {
            base.OnValidate();

            if (_characterVision.TargetInSight.GetPersistentEventCount() == 0)
                UnityEditor.Events.UnityEventTools.AddPersistentListener(_characterVision.TargetInSight, OnTargetInSight);
            if (_characterVision.TargetLost.GetPersistentEventCount() == 0)
                UnityEditor.Events.UnityEventTools.AddPersistentListener(_characterVision.TargetLost, OnTargetLost);
        }
#endif

        private new void Start()
        {
            base.Start();

            // Set the direction to move
            DirectCharacter();
        }

        private new void Update()
        {
            base.Update();

            _distanceToTarget = Vector2.Distance(_rigidbody.position, _targets[_currentTarget].position);
            if (_distanceToTarget <= _distanceToTargetGoal)
            {
                // Reached our target, change target
                _currentTarget++;

                if (_currentTarget >= _targets.Length)
                    _currentTarget = 0;

                DirectCharacter();
            }

            if (_enemyTarget != null)
            {
                Vector2 direction = CalculateDirectionToPosition(_rigidbody.position, (Vector2)_enemyTarget.position);
                if (_enemyTargetDirection != direction)
                {
                    _enemyTargetDirection = direction;
                    SetCustomState("Ranged Attack");
                    SetInput(direction);
                }
            }
        }

        private new void FixedUpdate()
        {
            if (_moveCharacter == true)
                base.FixedUpdate();
        }

#if UNITY_EDITOR
        protected new void OnGUI()
        {
            if (UnityEditor.Selection.activeGameObject != this.gameObject)
                return;

            base.OnGUI();

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label($"Current Target: {_currentTarget}", PizzaOnGUI.NormalLabel);
            GUILayout.Label($"Distance to Target: {_distanceToTarget}", PizzaOnGUI.NormalLabel);

            if (_enemyTarget != null)
            {
                Vector2 direction = CalculateDirectionToPosition(_rigidbody.position, (Vector2)_enemyTarget.position);
                GUILayout.Label($"Direction to Enemy: {direction}", PizzaOnGUI.NormalLabel);
            }

            GUILayout.EndVertical();
        }
#endif

        private void DirectCharacter()
        {
            if (_rigidbody.position.x > _targets[_currentTarget].position.x)
                SetInput(Vector2.left);
            else
                SetInput(Vector2.right);
        }

        public void OnTargetInSight(Transform transform)
        {
            _moveCharacter = false;

            Debug.Log("Target In Sight: " + transform.name);
            _enemyTarget = transform;
            Vector2 direction = CalculateDirectionToPosition(_rigidbody.position, (Vector2)_enemyTarget.position);
            _enemyTargetDirection = direction;
            SetCustomState("Ranged Attack");
            SetInput(direction);
        }

        public void OnTargetLost(Transform transform)
        {
            Debug.Log("Target Lost: " + transform.name);
            _enemyTarget = null;
            _moveCharacter = true;
            ClearCustomState();
            DirectCharacter();
        }

        private static Vector2 CalculateDirectionToPosition(Vector2 position, Vector2 target)
        {
            Vector2 direction;
            Vector2 positionDifference = target - position;

            // If the target is futher away on the X axis, use Left/Right
            if (Mathf.Abs(positionDifference.x) > Mathf.Abs(positionDifference.y))
            {
                // Positve values are right, negative values are left
                direction = (positionDifference.x > 0) ? Vector2.right : Vector2.left;
            }

            // If the target was further away on the Y axis, use Up/Down
            else
            {
                // Positve values are up, negative values are down
                direction = (positionDifference.y > 0) ? Vector2.up : Vector2.down;
            }


            return direction;
        }
    }
}