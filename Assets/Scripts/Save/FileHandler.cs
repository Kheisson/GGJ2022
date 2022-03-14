using System;
using System.IO;
using UnityEngine;

namespace Save
{
    public class FileHandler
    {
        #region Fields

        private readonly string _saveDataPath;
        private const string SaveFileName = "data.json";

        #endregion

        public FileHandler()
        {
            _saveDataPath = Path.Combine(Application.persistentDataPath, SaveFileName);
        }

        #region Methods

        private bool SaveFileExists => File.Exists(_saveDataPath);

        /// <summary>
        /// Loads save if there is one, if not then it creates a defaulted one 
        /// </summary>
        /// <returns>A loaded or new save from a JSON</returns>
        public SaveData LoadSaveData()
        {
            var saveRaw = "";
            
            if (!SaveFileExists)
            {
                SaveToStorage(new SaveData());
            }
            
            using var file = new FileStream(_saveDataPath, FileMode.Open);
            using var stream = new StreamReader(file);
            saveRaw = stream.ReadToEnd();

            return JsonUtility.FromJson<SaveData>(saveRaw);
        }

        /// <summary>
        /// Will save data to a json file on the users device
        /// </summary>
        /// <param name="data">Save data that originated from GameManager</param>
        public void SaveToStorage(SaveData data)
        {
            var writeData = JsonUtility.ToJson(data, true);

            try
            {
                using var fileStream = new FileStream(_saveDataPath, FileMode.Create);
                using var writer = new StreamWriter(fileStream);
                writer.Write(writeData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Encountered an error while saving to file \n {e.Message}");
            }
        }

        #endregion
    }
}