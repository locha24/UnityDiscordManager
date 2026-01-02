using UnityEngine;
using UnityEditor;

namespace SimpleDiscord.Editor
{
    public static class DiscordMenu
    {
        // Change "Tools/Discord Rich Presence" to whatever path you prefer
        [MenuItem("Tools/Discord Rich Presence/Edit Settings", false, 0)]
        public static void SelectSettings()
        {
            // 1. Try to find the asset by type
            var settings = Resources.Load<DiscordSettings>("DiscordSettings");

            // 2. If not found in Resources, try to find it anywhere in the project
            if (settings == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:DiscordSettings");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    settings = AssetDatabase.LoadAssetAtPath<DiscordSettings>(path);
                }
            }

            // 3. If still not found, ask to create it
            if (settings == null)
            {
                bool create = EditorUtility.DisplayDialog(
                    "Discord Settings Not Found",
                    "No DiscordSettings file found. Would you like to create one now?",
                    "Yes", "No"
                );

                if (create)
                {
                    CreateSettingsAsset();
                }
            }
            else
            {
                // Select the object in the inspector
                Selection.activeObject = settings;
                EditorGUIUtility.PingObject(settings);
            }
        }

        private static void CreateSettingsAsset()
        {
            // Ensure the directory exists
            if (!System.IO.Directory.Exists("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            var newSettings = ScriptableObject.CreateInstance<DiscordSettings>();

            // Create the asset in a Resources folder so it's easy to load
            string path = "Assets/Resources/DiscordSettings.asset";
            AssetDatabase.CreateAsset(newSettings, path);
            AssetDatabase.SaveAssets();

            // Select it
            Selection.activeObject = newSettings;
            EditorGUIUtility.PingObject(newSettings);

            Debug.Log($"Created new DiscordSettings at {path}");
        }
    }
}