using UnityEngine;

namespace Pizza
{
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class GameState_Pause
    {
        private const float PAUSED_TIME_SCALE = 0.0f;
        private const float NORMAL_TIME_SCALE = 1.0f;

        /// <summary>
        /// States if the game is paused or not.
        /// </summary>
        [SerializeField]
        [Tooltip("To change this value while the game is runing, right click -> Update Time Scale.")]
        private bool _isPaused = false;

#warning TOOD: Add editor UI for this variable.
        /// <summary>
        /// States whether pausing the game will affect Time.timeScale or not.
        /// </summary>
        /// <remarks>
        /// This value is serialized, but is also overriden by the set paused method.
        /// It is serialized only to allow in editor manipulation for testing.
        /// </remarks>
        [SerializeField]
        [Tooltip("To change this value while the game is runing, right click -> Update Time Scale. This value is overriden at runtime when the SetPause() function is called.")]
        private bool _pauseAffectsTimeScale = true;

        /// <summary>
        /// A custom time scale that overrides the Time.timeScale value.
        /// </summary>
        /// <remarks>
        /// This is an editor only feature.
        /// This will reset to 1.0f.
        /// </remarks>
        [Min(0.1f)]
        [SerializeField]
        [Tooltip("To change this value while the game is runing, right click -> Update Time Scale.")]
        private float _timeScaleOverride = 1.0f;

        /// <summary>
        /// States if the game is paused or not.
        /// </summary>
        public bool IsPaused { get { return _isPaused; } }

        /// <summary>
        /// This is a custom time scale that is updated based of game paused.
        /// </summary>
        /// <remarks>
        /// This is simply an alternative to using the bool if there is code where
        /// it is easier to multiply by a float value than do the bool check.
        /// </remarks>
        public float TimeScale
        {
            get
            {
                return (_isPaused == true) ? PAUSED_TIME_SCALE : NORMAL_TIME_SCALE;
            }
        }

        /// <summary>
        /// Sets the state of whether the game is paused or not.
        /// </summary>
        /// <param name="isPaused">Whether the game should become paused or not.</param>
        /// <param name="affectTimeScale">States if Time.timeScale should be affected. Default is true.</param>
        public void SetPaused(bool isPaused, bool affectTimeScale = true)
        {
            _isPaused = isPaused;
            _pauseAffectsTimeScale = affectTimeScale;
            UpdateTimeScale();
        }

        /// <summary>
        /// Updates the time scale based on whether the game is paused or not.
        /// </summary>
        [ContextMenu("Update Time Scale")]
        private void UpdateTimeScale()
        {
            // If the game is paused and we should affect time scale, set it to 0;
            if (_isPaused == true && _pauseAffectsTimeScale == true)
            {
                Time.timeScale = PAUSED_TIME_SCALE;
            }

            // Otherwise, put the time scale back to normal.
            // This allows the "soft pause" where isPaused can be true, but time scale is 1.0.
            else
            {
                Time.timeScale = NORMAL_TIME_SCALE;
            }

#if UNITY_EDITOR
            // If we are in the Unity Editor, apply our custom Time Scale when not paused
            if (_isPaused == false)
            {
                Time.timeScale = _timeScaleOverride;
            }
#endif
        }
    }
}