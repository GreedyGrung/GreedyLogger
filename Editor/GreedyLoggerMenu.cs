using UnityEngine;
using GreedyLogger.Settings;
using UnityEditor;

namespace GreedyLogger.Editor
{
    public static class GreedyLoggerMenu
    {
        [MenuItem("Tools/GreedyLogger/Instantiate GreedyLoggerInitializer")]
        public static void InstantiateGreedyLoggerInitializer()
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(Utils.FindAssetPath(Constants.InitializerPrefabFilter));

            if (prefab == null)
            {
                Debug.LogError($"GreedyLoggerInitializer prefab not found at path: {Utils.FindAssetPath(Constants.InitializerPrefabFilter)}");

                return;
            }

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (instance != null)
            {
                Undo.RegisterCreatedObjectUndo(instance, "Instantiate GreedyLoggerInitializer");
                Selection.activeObject = instance;
            }
            else
            {
                Debug.LogError("Failed to instantiate GreedyLoggerInitializer prefab.");
            }
        }

        [MenuItem("Tools/GreedyLogger/Open Settings")]
        public static void OpenGreedyLoggerSettings()
        {
            LoggingSettings settings = AssetDatabase.LoadAssetAtPath<LoggingSettings>(Utils.FindAssetPath(Constants.SettingsAssetFilter));

            if (settings == null)
            {
                Debug.LogError($"GreedyLoggerSettings asset not found at path: {Utils.FindAssetPath(Constants.SettingsAssetFilter)}");

                return;
            }

            EditorGUIUtility.PingObject(settings);
            AssetDatabase.OpenAsset(settings);
        }
    }
}