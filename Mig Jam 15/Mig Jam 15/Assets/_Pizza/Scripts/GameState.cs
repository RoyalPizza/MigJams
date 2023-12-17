using UnityEngine;

namespace Pizza
{
    /// <summary>
    /// A SO representing the current game state.
    /// </summary>
    [CreateAssetMenu(fileName = "New Game State", menuName = "Pizza/Game State")]
    public class GameState : ScriptableObject
    {
        public GameState_Pause PauseState;

        private void OnEnable()
        {
#warning TODO: Figure out how to only call this when going into play mode.
            PauseState?.SetPaused(false);
        }
    }
}