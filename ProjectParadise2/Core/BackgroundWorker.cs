// This class is responsible for handling background tasks such as reading command-line arguments,
// checking the NAT type via STUN tests, managing UPnP port forwarding, initializing Discord RPC, 
// and interacting with the database. It also manages game startup and shutdown processes.
using KuxiiSoft.Utils.Crashreport;
using ProjectParadise2.Views;
using System;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;

namespace ProjectParadise2.Core
{
    /// <summary>
    /// This class handles background tasks for network configurations, NAT type detection, 
    /// UPnP port forwarding, Discord integration, and game management.
    /// </summary>
    internal class BackgroundWorker
    {
        public static KuxiiSoft.Utils.Crashreport.Report _report;
        public static string LauncherNews { get; set; } = "|" + Constans.LauncherVersion;
        public static string MyNatType { get; set; } = "Strict:UdpBlocked";
        public static EventHandler OnLangset;

        /// <summary>
        /// Reads the command-line arguments, processes UPnP settings, runs a STUN test to determine the NAT type,
        /// and initializes Discord integration and connection updates.
        /// </summary>
        public static void ReadArgs()
        {
            var args = Environment.GetCommandLineArgs();
            CommandLineArg.ReadArgs(args);

            _report = new Report("PP2 Launcher", Constans.LauncherVersion, AppDomain.CurrentDomain.BaseDirectory);
            AppDomain.CurrentDomain.UnhandledException += Report.Generate;
            DoStunTest();
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
                    if (Constans.LauncherVersion != v[0])
                    {
                        if (MainViewModel.Instance != null)
                            MainViewModel.OpenLauncherupdate();
                    }
                }
            }
            catch (Exception ex)
            {
                LauncherNews = "|" + Constans.LauncherVersion;
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
                Task.Run(async () =>
                {
                    await NatDetector.RunTest();
                });
            }
            catch (Exception ex)
            {
                Log.Log.Error("Failed to run the STUN test ", ex);
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
            GC.Collect();
            GetLauncherVersion();
            Regestry.UpdateKey("NetworkNatType", BackgroundWorker.MyNatType, Database.Database.p2Database.Usersettings.IsSteambuild);
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
            GameRunner.RunGame();
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