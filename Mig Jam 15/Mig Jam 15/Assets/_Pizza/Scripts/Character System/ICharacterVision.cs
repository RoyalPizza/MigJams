using UnityEngine;

namespace Pizza.CharacterSystem
{
    public interface ICharacterVision
    {
        void OnTargetInSight(Transform transform);

        void OnTargetLost(Transform transform);
    }
}