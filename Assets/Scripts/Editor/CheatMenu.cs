using System;
using System.IO;
using Core;
using Save;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CheatMenu : EditorWindow
    {
        private bool infiniteLife = false;
        private string confirmChoices = "Confirm";
        private string amountToCredit = "";
        private string levelToOpen = "";
        
        [MenuItem("Cheat Menu/Open")]
        public static void OpenWindow()
        {
            GetWindow(typeof(CheatMenu));
        }

        private void OnGUI()
        {
            GUILayout.Label("--- Cheat Menu ---", EditorStyles.boldLabel);
            GUILayout.Space(15f);
            infiniteLife = GUILayout.Toggle(infiniteLife, "Infinite Lives");
            if (infiniteLife)
            {
                Debug.Log(infiniteLife);
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("Credits to add: ");
            amountToCredit = GUILayout.TextField(amountToCredit);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Level to open: ");
            levelToOpen = GUILayout.TextField(levelToOpen);
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button(confirmChoices))
            {
                Save(levelToOpen, amountToCredit);
            }

            GUILayout.Space(30f);
            GUILayout.Label("The Button Below will delete your progress!", 
                new GUIStyle() {fontSize = 20, fontStyle = FontStyle.Bold});
            if (GUILayout.Button("Delete Save file", GUILayout.Width(100), GUILayout.Height(20)))
            {
                DeleteSaveFile();
            }
        }

        private void Save(string levels, string credit)
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Can only save when game is playing");
                return;
            }

            if (levels.Length != 0)
            {
                var splitLevels = levels.Trim().Split(',');

                foreach (var level in splitLevels)
                {
                    DataManager.SaveOnFinishedLevel("Level" + level, 0, 0);
                }
            }

            if (credit.Length != 0)
            {
                DataManager.SaveOnFinishedLevel("Level1", 0, int.Parse(credit));
            }

            GameManager.Instance.IsImortal = infiniteLife;

            levelToOpen = "";
            amountToCredit = "";
        }

        private void DeleteSaveFile()
        {
            try
            {
                var file = Path.Combine(Application.persistentDataPath, "data.json");
                if (File.Exists(file))
                {
                    File.Delete(file);
                    Debug.Log("<color=red>Save file deleted</color>");
                    PlayerPrefs.DeleteAll();
                }
                else
                {
                    Debug.Log("<color=red>Save file not found</color>");
                }
            }
            catch (Exception exception)
            {
                Debug.Log($"Cannot delete file: {exception.Message}");
            }
        }
    }
}