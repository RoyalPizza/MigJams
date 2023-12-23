using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Pizza.SaveSystem
{
    [Serializable]
    public struct SaveFlagBoolean : ISerializationCallbackReceiver
    {
        [HideInInspector]
        public string Id;

        public string Name;

        [TableColumnWidth(50, resizable: false)]
        public bool Value;

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