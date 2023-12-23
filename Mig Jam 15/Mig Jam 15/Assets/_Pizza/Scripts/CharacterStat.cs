using System;
using UnityEditor;
using UnityEngine;

namespace Pizza.Character
{
    [CreateAssetMenu(fileName = "New Character Stat", menuName = "Pizza/Character Stat")]
    public class CharacterStat : ScriptableObject
    {
        public int Health;

        [SerializeField]
        private string _guid;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_guid))
            {
                _guid = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
            }
        }
#endif

        [ContextMenu("Save")]
        public void SaveToFile()
        {
            //SOSaveLoad.SaveObjectAsJson("testers.json", this);
        }

        [ContextMenu("Load")]
        public void LoadFroMFile()
        {
            //SOSaveLoad.LoadObjectFromJsonOverwrite("testers.json", this);
        }
    }
}