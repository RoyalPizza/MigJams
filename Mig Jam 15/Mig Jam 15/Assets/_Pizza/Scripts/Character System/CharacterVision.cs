using UnityEngine;
using UnityEngine.Events;

namespace Pizza.CharacterSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterVision : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public UnityTransformEvent TargetInSight;

        /// <summary>
        /// 
        /// </summary>
        public UnityTransformEvent TargetLost;

        [SerializeField]
        private string[] _tagsToAttack = new string[] { "Player" };

        [SerializeField]
        private ICharacterVision[] characterVisions;

        private Transform _target;

        private void OnTriggerStay2D(Collider2D collision)
        {
            // Decide if this is a target we should attack
            bool attackTarget = false;
            foreach (var tagToAttack in _tagsToAttack)
            {
                if (collision.tag == tagToAttack)
                {
                    attackTarget = true;
                    break;
                }
            }

            if (attackTarget == true && _target == null)
            {
                _target = collision.transform;
                TargetInSight?.Invoke(_target);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // If our target has left our field of vision, drop them as a target
            if (_target != null && collision.tag == _target.tag)
            {
                TargetLost?.Invoke(_target);
                _target = null;
            }
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (UnityEditor.Selection.activeGameObject != this.gameObject)
                return;

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Stats", PizzaOnGUI.HeaderLabel);
            GUILayout.Label($"Target: {_target?.name}", PizzaOnGUI.NormalLabel);
            GUILayout.EndVertical();
        }
#endif
    }
}