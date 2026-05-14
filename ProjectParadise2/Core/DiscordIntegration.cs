// This class is responsible for integrating Discord RPC (Rich Presence) into the application. 
// It manages the communication with Discord to display the application's status, such as whether the game is in the launcher, 
// in online mode, in offline mode, or if there is an error. 
// The `DiscordRpcClient` is initialized and the status is updated to Discord with different `RichPresence` states, 
// including information like the game details, current state, and buttons for additional interaction.
using DiscordRPC;
using DiscordRPC.Message;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.Diagnostics;
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
            if(Regestry.GetUserid() == -1)
            {
                Log.Warning("UserId is -1, Regestry issue?");
            }

            SetRpcTime();
            _client = new DiscordRpcClient("964267884383711254", DiscordPipe, null, true)
            {

            };
            _client.RegisterUriScheme("", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Debug.WriteLine("Initializing Discord RPC client...: " + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            _client.OnReady += _client_OnReady;
            _client.Subscribe(EventType.Join);// | EventType.JoinRequest);
            _client.OnJoinRequested += OnJoinRequested;
            _client.OnJoin += OnJoin;
            _client.Initialize();
            UpdateRpc(Start);
        }
        private static void _client_OnReady(object sender, ReadyMessage args)
        {
            if (_client?.CurrentUser != null)
            {
                UserName = _client.CurrentUser.Username;
                DisplayName = _client.CurrentUser.Username;
                UserAvatar = _client.CurrentUser.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x32);
                MainWindow.DoWork();
            }
        }

        private static void OnJoinRequested(object sender, JoinRequestMessage args)
        {
            Debug.WriteLine("JoinReqest: " + args.User.Username);
        }

        private static void OnJoin(object sender, JoinMessage args)
        {
            var data = args.Secret.Split('|');
            var unk = args.Type;
            var LobbyId = int.Parse(data[0]);
            var currentplayer = int.Parse(data[1]);
            var maxplayer = int.Parse(data[2]);
            var userId = int.Parse(data[3]);
            var PartyId = data[4];
            var myplayer = Regestry.GetUserid();

            var party = _client.CurrentPresence.Party;
            party.ID = PartyId;

            if ((currentplayer + 1) < maxplayer)
            {
                string joindata = $"{myplayer}|{LobbyId}";
                _client.UpdateParty(party);
                using (WebConnection wc = new WebConnection())
                {
                    wc.Timeout = 10;
                    System.Text.Encoding.UTF8.GetString(wc.DownloadData("https://cdn.project-paradise2.de/Requests/playerjoinrequest.php?data=" + Uri.EscapeDataString(joindata)));
                }
                Log.Info($"Try Join player: {userId} via Discordrpc: {myplayer} to LobbyId: {LobbyId} Playercount: {currentplayer}/{maxplayer}");
            }
        }

        /// <summary>
        /// Event handler that runs when the Discord client is ready.
        /// It sets the current user's details like Username, DisplayName, and Avatar.
        /// </summary>


        /// <summary>
        /// Default Rich Presence for the game launcher.
        /// </summary>
        public static readonly RichPresence Start = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"},

                new Button(){ Label = "Website", Url = "https://project-paradise2.de"}
            },

            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },

            Details = "Launcher Started",
            Assets = new Assets()
            {
                LargeImageKey = "paradise2",
                LargeImageText = "Welcome to Project Paradise 2!",
                SmallImageKey = "moderate"
            },
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
            State = "Playing TDU2 Online",
            StateUrl = "https://project-paradise2.de",
            Assets = new Assets()
            {
                LargeImageKey = "tdu2",
                LargeImageText = "Enjoying the game online",
                SmallImageKey = "open"
            },

            Type = ActivityType.Playing,

            Party = new Party()
            {
                ID = Regestry.GetUserid() + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"),
                Privacy = Party.PrivacySetting.Public,
                Size = 1,
                Max = 1,
            }

        };

        /// <summary>
        /// Rich Presence when the user is in online mode.
        /// </summary>
        public static readonly RichPresence TDU = new RichPresence()
        {
            Buttons = new Button[]
            {
                new Button(){ Label = "Find Us", Url = "https://pfcard.link/ProjectParadise2"}
            },

            Timestamps = new Timestamps()
            {
                Start = DateTime.UtcNow
            },

            Details = "Test Drive Unlimited",
            State = "Playing TDU1",
            Assets = new Assets()
            {
                LargeImageKey = "tdu",
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
            State = "Playing TDU2 Offline",
            Assets = new Assets()
            {
                LargeImageKey = "tdu2",
                LargeImageText = "Enjoying the game offline",
                SmallImageKey = "blocked"
            },


            Type = ActivityType.Playing,
            Party = new Party()
            {
                ID = Regestry.GetUserid() + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"),
                Size = 1,

                Privacy = Party.PrivacySetting.Private,
                Max = 1,
            },
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

            Details = "Back to Launcher",
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
            TDU.Timestamps = new Timestamps()
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
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to update Discord Rich Presence. Ensure that Discord is running and the RPC client is initialized." + ex.ToString());
            }
        }

        internal static void UpdateParty(int playerId)
        {
            string Lobby;
            using (WebConnection wc = new WebConnection())
            {
                wc.Timeout = 10;
                Lobby = System.Text.Encoding.UTF8.GetString(wc.DownloadData("https://cdn.project-paradise2.de/Requests/getmyLobby.php?user=" + playerId));
            }

            var data = Lobby.Split('|');
            if (!string.IsNullOrEmpty(data[0]) && !string.IsNullOrEmpty(data[1]) && !string.IsNullOrEmpty(data[2]))
            {
                if (_client.CurrentPresence.Buttons != null)
                    _client.CurrentPresence.Buttons = null;

                if (_client.CurrentPresence.Party == null)
                {
                    return;
                }

                if (_client.CurrentPresence.Party.ID != data[0])
                {
                    var party = _client.CurrentPresence.Party;
                    party.ID = data[0];
                    _client.UpdateParty(party);
                }

                _client.UpdatePartySize(int.Parse(data[1]), int.Parse(data[2]));

                Secrets secrets = new Secrets()
                {
                    JoinSecret = Lobby + "|" + Regestry.GetUserid() + "|" + _client.CurrentPresence.Party.ID,
                };

                _client.UpdateSecrets(secrets);
                _client.Invoke();
            }
        }
    }
}