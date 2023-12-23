using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Pizza.SaveSystem
{
    [Serializable]
    public struct SaveFlagVector2 : ISerializationCallbackReceiver
    {
        [HideInInspector]
        public string Id;

        public string Name;

        [TableColumnWidth(125, resizable: false)]
        public Vector2 Value;

        public void OnAfterDeserialize()
        {
            // Do nothing
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}