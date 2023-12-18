using UnityEngine;

namespace Pizza.CharacterControl
{
    public interface ICharacterVision
    {
        void OnTargetInSight(Transform transform);

        void OnTargetLost(Transform transform);
    }
}