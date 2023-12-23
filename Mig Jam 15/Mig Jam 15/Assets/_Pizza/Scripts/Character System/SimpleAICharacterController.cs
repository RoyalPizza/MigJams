using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pizza.CharacterSystem
{
    public class SimpleAICharacterController : BaseCharacterController
    {
        [SerializeField]
        private Transform[] _targets;

        [SerializeField]
        private bool _moveCharacter = true;

        private int _currentTarget = 0;
        private float _distanceToTarget;
        private float _distanceToTargetGoal = 0.5f;

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
        }

        private new void FixedUpdate()
        {
            if (_moveCharacter == true)
                base.FixedUpdate();
        }

        private void DirectCharacter()
        {
            if (_rigidbody.position.x > _targets[_currentTarget].position.x)
                SetInput(Vector2.left);
            else
                SetInput(Vector2.right);
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
            GUILayout.EndVertical();
        }
#endif
    }
}