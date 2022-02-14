using System;
using Core;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelProgressionSo))]
    public class LevelProgressionInspectorWindow : UnityEditor.Editor
    {
        
        public override void OnInspectorGUI()
        {
            var body = (LevelProgressionSo) target;
            serializedObject.Update();
            EditorGUILayout.LabelField("Level Editor:", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemySpawnList"),true);
            
            if(body.EnemyCount > body.SpawnEachAmount.Count)
                body.SpawnEachAmount.Add(1);
            if (body.EnemyCount < body.SpawnEachAmount.Count && body.SpawnEachAmount.Count > 0)
            {
                try
                {
                    body.SpawnEachAmount.RemoveAt(body.EnemyCount - 1);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.LogError($"Enemy list in {body.name} stage data cannot be left empty! \n{e.Message}");
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnEachAmount"),true);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(20);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("delaySpawnTimer"),true);
            serializedObject.ApplyModifiedProperties();
            
        }
    }
}