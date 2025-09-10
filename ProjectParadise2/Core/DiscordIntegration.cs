// This class is responsible for integrating Discord RPC (Rich Presence) into the application. 
// It manages the communication with Discord to display the application's status, such as whether the game is in the launcher, 
// in online mode, in offline mode, or if there is an error. 
// The `DiscordRpcClient` is initialized and the status is updated to Discord with different `RichPresence` states, 
// including information like the game details, current state, and buttons for additional interaction.
using DiscordRPC;
using KuxiiSoft.Utils;
using System;
using System.Threading;

namespace ProjectParadise2
{
    /// <summary>
    /// This class handles the integration of Discord's Rich Presence (RPC) into the application. 
    /// It manages the communication with Discord to update the status such as game mode, errors, etc.
    /// </summary>
    class DiscordIntegration
    {
        private static DiscordRpcClient _client;
        private const int DiscordPipe = -1;
        public static string UserName { get; set; } = "";
        public static string DisplayName { get; set; } = "";
        public static string UserAvatar { get; set; } = "";

        /// <summary>
        /// Initializes the Discord RPC client in a separate background thread.
        /// </summary>
        public static void Init()
        {
            var discord = new Thread(OnStart);
            discord.IsBackground = true;
            discord.Start();
        }

        /// <summary>
        /// Starts the Discord RPC client and sets the initial Rich Presence.
        /// </summary>
        public static void OnStart()
        {
            SetRpcTime();
            _client = new DiscordRpcClient("964267884383711254")//, pipe: DiscordPipe)
            {

            };
            _client.Initialize();
            _client.OnReady += _client_OnReady;
            UpdateRpc(Start);
        }

        /// <summary>
        /// Event handler that runs when the Discord client is ready.
        /// It sets the current user's details like Username, DisplayName, and Avatar.
        /// </summary>
        private static void _client_OnReady(object sender, DiscordRPC.Message.ReadyMessage args)
        {
            if (_client?.CurrentUser != null)
            {
                UserName = _client.CurrentUser.Username;
                DisplayName = _client.CurrentUser.Username;
                UserAvatar = _client.CurrentUser.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x32);
                MainWindow.DoWork();
            }
        }

        /// <summary>
        /// Default Rich Presence for the game launcher.
        /// </summary>
        public static readonly RichPresence Start = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"}
            },

            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },

            Details = "Test Drive Unlimited 2",
            State = "Launcher Started",
            Assets = new Assets()
            {
                LargeImageKey = "paradise2",
                LargeImageText = "Welcome to Project Paradise 2!",
                SmallImageKey = "moderate"
            }
        };

        /// <summary>
        /// Rich Presence when the user is in online mode.
        /// </summary>
        public static readonly RichPresence OnlineMode = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"}
            },

            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },

            Details = "Test Drive Unlimited 2",
            State = "Playing in online mode",
            Assets = new Assets()
            {
                LargeImageKey = "paradise2",
                LargeImageText = "Enjoying the game online",
                SmallImageKey = "open"
            }
        };

        /// <summary>
        /// Rich Presence when the user is in offline mode.
        /// </summary>
        public static readonly RichPresence OfflineMode = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"}
            },

            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },

            Details = "Test Drive Unlimited 2",
            State = "Playing in offline mode",
            Assets = new Assets()
            {
                LargeImageKey = "paradise2",
                LargeImageText = "Enjoying the game offline",
                SmallImageKey = "blocked"
            }
        };

        /// <summary>
        /// Rich Presence when there is an error collecting data.
        /// </summary>
        public static readonly RichPresence ErrorCollecting = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"}
            },

            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },

            Details = "Test Drive Unlimited 2",
            State = "Error Collecting Data",
            Assets = new Assets()
            {
                LargeImageKey = "paradise2",
                LargeImageText = "Troubleshooting data collection",
                SmallImageKey = "open"
            }
        };

        /// <summary>
        /// Rich Presence when the game is closed.
        /// </summary>
        public static readonly RichPresence Closed = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"}
            },

            Timestamps = new Timestamps()
            {
                Start = Start.Timestamps.Start,
                End = DateTime.UtcNow
            },

            Details = "Test Drive Unlimited 2",
            State = "Game Closed",
            Assets = new Assets()
            {
                LargeImageKey = "paradise2",
                LargeImageText = "Stopped Playing",
                SmallImageKey = "blocked"
            }
        };

        /// <summary>
        /// Sets the timestamps for the Rich Presence states to the current time.
        /// </summary>
        public static void SetRpcTime()
        {
            Start.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            OfflineMode.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            OnlineMode.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            ErrorCollecting.Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow,
            };
            _client?.UpdateStartTime();
        }

        /// <summary>
        /// Stops the Discord RPC client and cleans up the resources.
        /// It clears the presence, disposes of the client, and nullifies the reference to prevent accidental reuse.
        /// </summary>
        public static void StopRPC()
        {
            if (_client != null)
            {
                try
                {
                    // Optionally, update end time if you need to log session end time
                    _client.UpdateEndTime();
                    Log.Info("End time updated successfully.");

                    _client.UpdateClearTime();

                    _client.ClearPresence();

                    // Deinitialize the RPC client to clean up any active connections
                    _client.Deinitialize();
                    Log.Info("Discord RPC client deinitialized successfully.");

                    // Dispose of the client to free up resources
                    _client.Dispose();
                    Log.Info("Discord RPC client disposed of and resources freed.");

                    // Optionally, nullify the client reference to avoid accidental reuse
                    _client = null;
                    Log.Info("RPC client reference set to null to prevent accidental reuse.");
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during the shutdown process
                    Log.Error($"Error while stopping Discord RPC: {ex.Message}: " + ex);
                }
            }
        }

        /// <summary>
        /// Updates the current Rich Presence state to Discord.
        /// If the state is "Closed," it updates the end time and clears the presence.
        /// </summary>
        /// <param name="state">The new Rich Presence state to set.</param>
        public static void UpdateRpc(RichPresence state)
        {
            try
            {
                _client?.SetPresence(state);

                if (state == Closed)
                {
                    try
                    {
                        _client.UpdateEndTime();
                        _client.UpdateClearTime();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Failed to update end time or clear time: " + ex);
                    }
                }
                else if (state == Start)
                {
                    _client.UpdateStartTime();
                    _client.Invoke();
                }
                _client?.Invoke();
            }
            catch (Exception) { }
        }
    }
}