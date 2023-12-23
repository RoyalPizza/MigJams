using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pizza.SaveSystem
{
    [CreateAssetMenu(fileName = "New Save Manger", menuName = "Pizza/Save Manager")]
    public class SaveManager : ScriptableObject
    {
        public event EventHandler GameSaving;
        public event EventHandler GameLoading;
        private Dictionary<string, string> _saveData;

        private void OnGameSave()
        {
            GameSaving?.Invoke(this, EventArgs.Empty);
        }

        private void OnGameLoad()
        {
            GameLoading?.Invoke(this, EventArgs.Empty);
        }

        public void AddSaveData(string guid, string json)
        {
            if (_saveData.ContainsKey(guid) == true)
            {
                _saveData[guid] = json;
            }
            else
            {
                _saveData.Add(guid, json);
            }
        }
    }
}