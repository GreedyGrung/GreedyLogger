using UnityEngine;
using GreedyLogger.Settings;
using UnityEditor;

namespace GreedyLogger.Editor
{
    [CustomEditor(typeof(LoggingSettings))]
    public class GreedyLoggerSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Code"))
            {
                if (!((LoggingSettings)target).CanBeGenerated())
                {
                    Debug.LogError("Generation aborted due to inconsistent LogImportance values!");
                    return;
                }

                GreedyLoggerCodeGenerator.Generate();
            }

            if (GUILayout.Button("Reset Contexts Filter"))
            {
                ((LoggingSettings)target).ResetFilter();
            }

            if (GUILayout.Button("Restore to Defaults"))
            {
                ((LoggingSettings)target).RestoreToDefaults();
            }
        }
    }
}