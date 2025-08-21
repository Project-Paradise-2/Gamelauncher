// This class is responsible for handling background tasks such as reading command-line arguments,
// checking the NAT type via STUN tests, managing UPnP port forwarding, initializing Discord RPC, 
// and interacting with the database. It also manages game startup and shutdown processes.
using KuxiiSoft.Utils.Crashreport;
using ProjectParadise2.Core.Classes;
using ProjectParadise2.Core.Stun;
using ProjectParadise2.Core.UPnP;
using ProjectParadise2.Views;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;

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
        public static string MyNatType { get; private set; } = "Strict:UdpBlocked";
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
            if (CommandLineArg.SkipUpnp || Database.Database.p2Database.Usersettings.UpnpWorker.Equals(false))
            {
                Log.Log.Print("Skip Upnp");
            }
            else
            {
                var state = UPnP.UPnP.IsOpen(Protocol.UDP, 8889);
                if (!state)
                {
                    try
                    {
                        UPnP.UPnP.FindGateway();
                        UPnP.UPnP.Open(Protocol.UDP, 8889, 8889, "TDU2");
                        var state2 = UPnP.UPnP.IsOpen(Protocol.UDP, 8889);
                        Log.Log.Print("Forwarding from: " + UPnP.UPnP.ExternalIP + " to " + UPnP.UPnP.LocalIP);
                    }
                    catch (Exception ex)
                    {
                        Log.Log.Print("Failed to set forwarding rule ", ex);
                    }
                }
            }
            DoStunTest();
            try
            {
                if (Database.Database.p2Database.Usersettings.DiscordRPC)
                    DiscordIntegration.Init();

            }
            catch (Exception ex)
            {
                Log.Log.Print("Failed to patch the original domain ", ex);
            }
        }

        public static string[] StunServer = { "stun1.l.google.com", "proxy.project-paradise2.de", "stun3.l.google.com" };
        static int stunTry = 0;

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
                Log.Log.Print("Unable Load Launcher Version: ", ex);
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
                STUN_Result result = null;
                try
                {
                    result = STUN_Client.Query(StunServer[stunTry], 3478, new IPEndPoint(IPAddress.Any, 8889));
                }
                catch (Exception ex)
                {
                    Log.Log.Print("Error querying STUN server: ", ex);
                    return;
                }

                if (result == null)
                {
                    Log.Log.Print("No response received from the STUN server.");
                    return;
                }

                switch (result.NetType)
                {
                    case STUN_NetType.UdpBlocked:
                        Log.Log.Print("UDP is blocked. This is a strict NAT type, and communication is highly limited.");
                        break;

                    case STUN_NetType.OpenInternet:
                        Log.Log.Print("No NAT or firewall detected. Public IP with no restrictions.");
                        break;

                    case STUN_NetType.SymmetricUdpFirewall:
                        Log.Log.Print("Symmetric UDP firewall detected. Symmetric NAT may exist, and incoming connections are blocked.");
                        break;

                    case STUN_NetType.FullCone:
                        Log.Log.Print("Full Cone NAT detected. You can receive incoming connections from any external host.");
                        break;

                    case STUN_NetType.RestrictedCone:
                        Log.Log.Print("Restricted Cone NAT detected. Only previously contacted external hosts can reach you.");
                        break;

                    case STUN_NetType.PortRestrictedCone:
                        Log.Log.Print("Port Restricted Cone NAT detected. External hosts must match both the IP and port to communicate.");
                        break;

                    case STUN_NetType.Symmetric:
                        Log.Log.Print("Symmetric NAT detected. Only specific, previously contacted external hosts can communicate with you.");
                        break;

                    default:
                        result.NetType = STUN_NetType.UdpBlocked;
                        Log.Log.Print("Unknown or unchecked NAT type.");
                        break;
                }

                MyNatType = STUN_NatType.GetType(result.NetType);

                if (result.NetType != STUN_NetType.UdpBlocked)
                {
                    IPEndPoint publicEP = result.PublicEndPoint;
                    Log.Log.Print("Public endpoint detected: " + publicEP + ", mapped endpoint: " + UPnP.UPnP.ExternalIP);
                }
                else
                {
                    if (stunTry != StunServer.Length - 1)
                    {
                        stunTry++;
                        Log.Log.Print($"UDP is blocked or the STUN server is not responding. Trying another server [{stunTry}|{StunServer.Length - 1}]");
                        DoStunTest();
                    }
                    else
                    {
                        Log.Log.Print("UDP is blocked or the STUN server is not responding. After 5 attempts, stopping.");
                    }
                }

                if (result != null)
                {
                    try
                    {
                        Regestry.UpdateKey("NetworkNatType", MyNatType, Database.Database.p2Database.Usersettings.IsSteambuild);
                    }
                    catch (Exception ex)
                    {
                        Log.Log.Print("Failed Update Regestry: ", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log.Print("Failed on Stun Test: ", ex);
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
            GC.Collect();
            GetLauncherVersion();
        }

        /// <summary>
        /// Closes the launcher, stops the Discord RPC, and writes the database changes.
        /// It also kills the game process and exits the application.
        /// </summary>
        public static void CloseLauncher()
        {
            try
            {
                var state = UPnP.UPnP.IsOpen(Protocol.UDP, 8889);
                if (state)
                    UPnP.UPnP.Close(Protocol.UDP, 8889);
                DiscordIntegration.StopRPC();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            try
            {
                Database.Database.Write();
            }
            catch (Exception ex2)
            {
                Log.Log.Print("Failed to update the database ", ex2);
            }
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
                    Log.Log.Print("[WR]Missing key, connect to the generator");
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
            Utils.UpdateConnection();
            GameRunner.RunGame();
        }

        public static void SetLang()
        {
            OnLangset?.Invoke(null, null);
        }
    }
}
