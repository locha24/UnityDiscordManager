using UnityEngine;

namespace SimpleDiscord
{
    [CreateAssetMenu(fileName = "DiscordSettings", menuName = "Discord/Settings")]
    public class DiscordSettings : ScriptableObject
    {
        [Tooltip("Get this from the Discord Developer Portal")]
        public long applicationId;

        [Header("Default Presence")]
        public string details = "Playing a Game";
        public string state = "In Menu";
        public string largeImageKey = "logo";
        public string largeImageText = "My Game";
    }
}