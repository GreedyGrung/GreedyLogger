using UnityEngine;
using GreedyLogger.Settings;
using UnityEditor;

namespace GreedyLogger.Editor
{
    internal static class GreedyLoggerMenu
    {
        [MenuItem("Tools/GreedyLogger/Instantiate GreedyLoggerInitializer")]
        public static void InstantiateGreedyLoggerInitializer()
        {
            string initializerPath = Utils.FindAssetPath(Constants.InitializerPrefabFilter);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(initializerPath);

            if (prefab == null)
            {
                Debug.LogError($"GreedyLoggerInitializer prefab not found at path: {initializerPath}");

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
            string settingsPath = Utils.FindAssetPath(Constants.SettingsAssetFilter);
            LoggingSettings settings = AssetDatabase.LoadAssetAtPath<LoggingSettings>(settingsPath);

            if (settings == null)
            {
                Debug.LogError($"GreedyLoggerSettings asset not found at path: {settingsPath}");

                return;
            }

            EditorGUIUtility.PingObject(settings);
            AssetDatabase.OpenAsset(settings);
        }
    }
}