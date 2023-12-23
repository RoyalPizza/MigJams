using UnityEngine;

namespace Pizza.CharacterSystem
{
    /// <summary>
    /// This class repreents a movement transport configuration.
    /// </summary>
    /// <remarks>
    /// This is so that configuration of idle, slow, fast for different types of transportation could be abstracted.
    /// This is intended to be used by the character controller.
    /// </remarks>
    [CreateAssetMenu(fileName = "New Movement Transport", menuName = "Pizza/Movement Transport")]
    public class MovementTransportConfig : ScriptableObject
    {
        /// <summary>
        /// 
        /// </summary>
        public MovementTransport TransportState;

        /// <summary>
        /// 
        /// </summary>
        public string IdleAnimStateName;

        /// <summary>
        /// 
        /// </summary>
        public string SlowAnimStateName;

        /// <summary>
        /// 
        /// </summary>
        public string FastAnimStateName;

        /// <summary>
        /// 
        /// </summary>
        public float SlowSpeed = 2.5f;

        /// <summary>
        /// 
        /// </summary>
        public float FastSpeed = 5.0f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speedState"></param>
        /// <returns></returns>
        public string GetStateName(MovementSpeed speedState)
        {
            return speedState switch
            {
                MovementSpeed.Idle => IdleAnimStateName,
                MovementSpeed.Slow => SlowAnimStateName,
                MovementSpeed.Fast => FastAnimStateName,
                _ => IdleAnimStateName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speedState"></param>
        /// <returns></returns>
        public float GetSpeed(MovementSpeed speedState)
        {
            return speedState switch
            {
                MovementSpeed.Idle => 0.0f,
                MovementSpeed.Slow => SlowSpeed,
                MovementSpeed.Fast => FastSpeed,
                _ => 0.0f
            };
        }
    }
}