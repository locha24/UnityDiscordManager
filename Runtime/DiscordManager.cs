using UnityEngine;
using Discord; // Uses the official Discord.cs namespace

namespace SimpleDiscord
{
    public class DiscordManager : MonoBehaviour
    {
        public DiscordSettings settings;
        private Discord.Discord _discord;
        private ActivityManager _activityManager;

        // Singleton instance
        public static DiscordManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeDiscord();
        }

        private void InitializeDiscord()
        {
            if (settings == null)
            {
                Debug.LogError("Discord Settings not assigned!");
                return;
            }

            try
            {
                // Create the Discord instance
                _discord = new Discord.Discord(settings.applicationId, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
                _activityManager = _discord.GetActivityManager();

                // Set default status
                UpdateStatus(settings.details, settings.state);
                Debug.Log("Discord RPC Started");
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Discord RPC failed to initialize (Discord might be closed): {e.Message}");
                Destroy(gameObject); // Optional: Destroy self if Discord isn't running
            }
        }

        private void Update()
        {
            if (_discord != null)
            {
                _discord.RunCallbacks();
            }
        }

        public void UpdateStatus(string details, string state, string largeImage = "", string largeText = "")
        {
            if (_activityManager == null) return;

            var activity = new Activity
            {
                Details = details,
                State = state,
                Assets = {
                    LargeImage = string.IsNullOrEmpty(largeImage) ? settings.largeImageKey : largeImage,
                    LargeText = string.IsNullOrEmpty(largeText) ? settings.largeImageText : largeText
                },
                Timestamps = {
                    Start = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                }
            };

            _activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Result.Ok) Debug.LogWarning("Failed to update Discord status: " + res);
            });
        }

        private void OnApplicationQuit()
        {
            if (_discord != null)
            {
                _discord.Dispose();
                _discord = null;
            }
        }
    }
}