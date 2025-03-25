using UnityEditor;
using System.IO;

namespace GreedyLogger.Editor
{
    internal static class Utils
    {
        internal static string FindAssetPath(string asset)
        {
            string[] guids = AssetDatabase.FindAssets(asset);

            if (guids.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(guids[0]);
            }

            return null;
        }

        internal static string FindPackageRoot()
        {
            string[] guids = AssetDatabase.FindAssets("t:Folder GreedyLogger");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (path.EndsWith("GreedyLogger") && Directory.Exists(Path.Combine(path, "Runtime")))
                {
                    return path;
                }
            }
            return null;
        }
    }
}