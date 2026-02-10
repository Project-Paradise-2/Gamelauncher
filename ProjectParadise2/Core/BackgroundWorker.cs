// This class is responsible for handling background tasks such as reading command-line arguments,
// checking the NAT type via STUN tests, managing UPnP port forwarding, initializing Discord RPC, 
// and interacting with the database. It also manages game startup and shutdown processes.
using Newtonsoft.Json;
using ProjectParadise2.Core.GameProfiles;
using ProjectParadise2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectParadise2.Core
{
    /// <summary>
    /// This class handles background tasks for network configurations, NAT type detection, 
    /// UPnP port forwarding, Discord integration, and game management.
    /// </summary>
    internal class BackgroundWorker
    {
        public static string SecSession { get; set; }
        public static bool NetworkTestsDone { get; set; } = false;
        public static string LauncherNews { get; set; } = "|" + Constans.LauncherVersion;
        public static string MyNatType { get; set; } = "Strict:UdpBlocked";
        public static List<GameProfile> GameProfiles = new List<GameProfile>();
        public static EventHandler OnLangset;
        public static GameProfile CurrentProfile()
        {
            try
            {
                return GameProfiles[Database.Database.p2Database.Usersettings.SelectedProfile];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the command-line arguments, processes UPnP settings, runs a STUN test to determine the NAT type,
        /// and initializes Discord integration and connection updates.
        /// </summary>
        public static void ReadArgs()
        {
            var args = Environment.GetCommandLineArgs();
            CommandLineArg.ReadArgs(args);
            GameProfiles = GameProfileReader.ReadGameProfiles();



            DoStunTest();

            while (Database.Database.IsLoadet)
                break;


            if (GameProfiles.Count == 0 && !string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Gamedirectory) && !string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
            {
                Log.Log.Info("Launcher update dedected, Try Auto Create Profile from Old Settings.");

                var result = MessageBox.Show(Lang.GetText(139), "Project Paradise 2 - Database Update", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    MessageBoxResult warning = MessageBox.Show(
                                    Lang.GetText(140),
                                    "Project Paradise 2 - Database Update",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                }
                else if (result == MessageBoxResult.Yes)
                {
                    GameProfile profile = new GameProfile("Converted Profile", Database.Database.p2Database.Usersettings.ExePath, "");
                    profile.OnlineMode = true;
                    string jsonData = JsonConvert.SerializeObject(profile, Formatting.Indented);

                    if (File.Exists(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile"))
                    {
                        File.Delete(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile");
                    }
                    File.WriteAllText(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile", jsonData);
                    MessageBoxResult warning = MessageBox.Show(
                        Lang.GetText(141),
                        "Project Paradise 2 - Database Update",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }

            try
            {
                if (Database.Database.p2Database.Usersettings.DiscordRPC)
                    DiscordIntegration.Init();
            }
            catch (Exception ex)
            {
                Log.Log.Error("Failed to init Discord RPC ", ex);
            }

            try
            {

                Utils.UpdateConnection();
            }
            catch (Exception ex)
            {
                Log.Log.Error("Failed to update the connection ", ex);
            }
        }

        public static void RefreshProfiles()
        {
            GameProfiles.Clear();
            GameProfiles = GameProfileReader.ReadGameProfiles();

            foreach (var profile in GameProfiles)
            {
                Log.Log.Info("Found Profile: " + profile.Profilename + " Path: " + profile.Gamepath + " Args: " + profile.Arguments);
            }
            HomeView.Instance.AddProfiles(GameProfiles);
        }

        /// <summary>
        /// Fetches the latest launcher version from a remote server and checks if an update is available.
        /// </summary>
        public static void GetLauncherVersion()
        {
            try
            {
                using (WebConnection wc = new WebConnection())
                {
                    wc.Timeout = 10;
                    LauncherNews = System.Text.Encoding.UTF8.GetString(wc.DownloadData("https://cdn.project-paradise2.de/Requests/Launcherversion.php"));
                }
                if (Database.Database.p2Database.Usersettings.Autoupdatecheck)
                {
                    var v = LauncherNews.Split("|");
                    Version currentVersion = new Version(Constans.LauncherVersion);
                    Version webVersion = new Version(v[0]);

                    if (webVersion > currentVersion)
                    {
                        Log.Log.Info($"Update available! Current version: {currentVersion}, New version: {webVersion}");
                        if (MainViewModel.Instance != null)
                            MainViewModel.OpenLauncherupdate();
                    }
                    else if (webVersion == currentVersion)
                    {
                        Console.WriteLine("App ist aktuell");
                        Log.Log.Info("Launcher is up to date");
                    }
                    else
                    {
                        Console.WriteLine("Lokale Version ist neuer (dev build?)");
                        Log.Log.Info("Local version is newer (dev build?)");
                    }

                }
            }
            catch (Exception ex)
            {
                LauncherNews = "Unable to Load Last News from Server|" + Constans.LauncherVersion;
                Log.Log.Error("Failed to get the launcher version ", ex);
            }
        }

        /// <summary>
        /// Performs a STUN test to determine the NAT type by querying multiple STUN servers.
        /// It updates the NAT type and logs relevant information about the public endpoint and NAT restrictions.
        /// </summary>
        public static void DoStunTest()
        {
            try
            {
                Task.Run(async () => await NatDetector.RunTest()).Wait();
            }
            catch (Exception ex)
            {
                Log.Log.Error("Failed to run the STUN test ", ex);
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
            GC.Collect();
            if (CurrentProfile() != null)
            {
                try
                {
                    Regestry.UpdateKey("NetworkNatType", BackgroundWorker.MyNatType, CurrentProfile().SteamBuild);
                }
                catch (Exception)
                {
                    Regestry.UpdateKey("NetworkNatType", BackgroundWorker.MyNatType, false);
                }
            }
            else
            {
                Regestry.UpdateKey("NetworkNatType", BackgroundWorker.MyNatType, false);
            }
            NetworkTestsDone = true;
            GetLauncherVersion();
        }

        public static List<NewsEntry> GetLauncherNews()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent", "ProjectParadise2-Launcher");

                    string json = wc.DownloadString("https://cdn.project-paradise2.de/Requests/news.json");
                    var newsList = JsonConvert.DeserializeObject<List<NewsEntry>>(json);
                    return newsList ?? new List<NewsEntry>();
                }
            }
            catch (Exception ex)
            {
                Log.Log.Error("Error Reading Launchernews: " + ex.Message);
                return new List<NewsEntry>();
            }
        }

        /// <summary>
        /// Closes the launcher, stops the Discord RPC, and writes the database changes.
        /// It also kills the game process and exits the application.
        /// </summary>
        public static void CloseLauncher()
        {
            try
            {
                DiscordIntegration.StopRPC();
            }
            catch (Exception ex)
            {
                Log.Log.Error("Failed to stop Discord RPC ", ex);
            }

            try
            {
                Database.Database.Write();
            }
            catch (Exception ex2)
            {
                Log.Log.Error("Failed to write the database on exit ", ex2);
            }
            Task.Run(async () => await NatDetector.RemoveUpnp()).Wait();

            GameRunner.KillGame();
            Environment.Exit(0);
        }

        public static string GetKey()
        {
            try
            {
                using (WebConnection wc = new WebConnection())
                {
                    wc.Timeout = 5;
                    Log.Log.Warning("Missing key, connect to the generator");
                    return System.Text.Encoding.UTF8.GetString(wc.DownloadData(Constans.Cdn + $"/Requests/serial.php"));
                }
            }
            catch (WebException)
            {
                return "";
            }
        }

        /// <summary>
        /// Runs the game by invoking the GameRunner to start the game process.
        /// </summary>
        public static void RunGame()
        {
            if (CurrentProfile() == null)
            {
                Log.Log.Error("Failed run Game -> no Profile Selected!");
                try
                {
                    HomeView.Instance.ShowRunGameButton();
                }
                catch (Exception ex)
                {
                    Log.Log.Error("Failed to hide the run game button ", ex);
                }
                return;
            }

            CurrentProfile().UpdateRegestry();
            Thread.Sleep(1000);
            GameRunner.RunGame(CurrentProfile());
        }

        /// <summary>
        /// Sets the language by invoking the OnLangset event.
        /// </summary>
        public static void SetLang()
        {
            OnLangset?.Invoke(null, null);
        }
    }
}