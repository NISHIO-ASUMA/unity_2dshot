using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveContoroller))]
public class WaveContorollerEditor : Editor
{
    SerializedProperty waves;
    SerializedProperty spawnPoints;
    SerializedProperty spawnInterval;

    void OnEnable()
    {
        waves = serializedObject.FindProperty("waves");
        spawnPoints = serializedObject.FindProperty("spawnPoints");
        spawnInterval = serializedObject.FindProperty("spawnInterval");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(spawnPoints, true);
        EditorGUILayout.PropertyField(spawnInterval);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Wave ê›íË", EditorStyles.boldLabel);

        for (int i = 0; i < waves.arraySize; i++)
        {
            SerializedProperty wave = waves.GetArrayElementAtIndex(i);
            SerializedProperty waveName = wave.FindPropertyRelative("waveName");
            SerializedProperty enemies = wave.FindPropertyRelative("enemies");

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            waveName.stringValue = EditorGUILayout.TextField("Wave Name", waveName.stringValue);

            if (GUILayout.Button("çÌèú", GUILayout.Width(50)))
            {
                waves.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(enemies, new GUIContent("Enemies"), true);
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Wave í«â¡"))
        {
            waves.arraySize++;
            SerializedProperty newWave = waves.GetArrayElementAtIndex(waves.arraySize - 1);

            SerializedProperty waveName = newWave.FindPropertyRelative("waveName");
            if (waveName != null) waveName.stringValue = "New Wave";

            SerializedProperty enemies = newWave.FindPropertyRelative("enemies");
            if (enemies != null) enemies.arraySize = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}