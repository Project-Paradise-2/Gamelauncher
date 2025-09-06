using Open.Nat;
using ProjectParadise2.Core.Classes;
using STUN;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectParadise2.Core
{
    /// <summary>
    /// Indicates the NAT (Network Address Translation) status of the network connection.
    /// </summary>
    public enum NATStatus
    {
        Open,
        Moderate,
        Strict,
        Blocked,
        CGNAT,
        DSLite,
        Unknown
    }

    /// <summary>
    /// Indicates the type of port forwarding used for NAT traversal.
    /// </summary>
    public enum Forwarding
    {
        None, Upnp, Natpmp
    }

    /// <summary>
    /// Represents the result of a NAT detection test, including status, public IP, port, and reachability information.
    /// </summary>
    public class NatResult
    {
        public NATStatus Status { get; set; }
        public IPAddress PublicIp { get; set; }
        public int Port { get; set; }
        public bool ExternalReachable { get; set; }
        public bool InternalReachable { get; set; } = false;

        public override string ToString()
        {
            string reach = ExternalReachable ? "The server is reachable." : "The server is not reachable";
            return $"{Status} ({PublicIp}:{Port})\nServer → {reach}";
        }

        public string InternToString()
        {
            if (!InternalReachable)
                Status = NATStatus.Blocked;
            string reach = InternalReachable ? "The player is reachable" : "The player is not reachable";
            return $"Player → {reach}";
        }
    }

    /// <summary>
    /// Represents the overall result of a NAT test, including results before and after UPnP, UPnP availability, port, status, forwarding type, and reachability information.
    /// </summary>
    public class NatTestResult
    {
        public NatResult BeforeUpnp { get; set; }
        public NatResult AfterUpnp { get; set; }
        public bool UpnpAvailable { get; set; }
        public int UpnpPort { get; set; }
        public string UpnpStatus { get; set; }
        public Forwarding ForwardingType { get; set; }
        public bool InternalReachable { get; set; } = false;
    }

    /// <summary>
    /// Handles NAT detection, UPnP port forwarding, and STUN tests to determine the NAT type and reachability of the network connection.
    /// </summary>
    public class NatDetector
    {
        private const int TestPort = 8889;
        /// <summary>
        /// URL of the external service used to check external reachability.
        /// </summary>
        private static string ExternalCheckUrl = $"http://{Constans.ServerIP}:9024/conncheck/check";

        public static NatTestResult Result { get; set; }

        /// <summary>
        /// Runs the NAT detection test, including UPnP port mapping and STUN tests, and updates the NatTestResult with the results.
        /// </summary>
        /// <returns></returns>
        public static async Task RunTest()
        {
            Result = new NatTestResult();
            using var listenerCts = new CancellationTokenSource();
            _ = StartEchoListener(TestPort, listenerCts.Token);
            //Only used by Testing..
            //Result.BeforeUpnp = await Detect(TestPort);
            bool upnpSuccess = false;
            int mappedPort = 0;
            string statusMessage = "Not checked";
            Forwarding type = Forwarding.None;

            if (!CommandLineArg.SkipUpnp || !Database.Database.p2Database.Usersettings.UpnpWorker.Equals(false))
            {
                try
                {
                    var discoverer = new NatDiscoverer();
                    using var cts = new CancellationTokenSource(5000);
                    var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
                    type = Forwarding.Upnp;
                    await device.CreatePortMapAsync(new Mapping(Protocol.Udp, TestPort, TestPort, "TDU2"));
                    upnpSuccess = true;
                    mappedPort = TestPort;
                    statusMessage = $"Port {TestPort} successfully mapped via UPnP";
                    Console.WriteLine(statusMessage);
                }
                catch
                {
                    try
                    {
                        var discoverer = new NatDiscoverer();
                        using var cts = new CancellationTokenSource(5000);
                        var device = await discoverer.DiscoverDeviceAsync(PortMapper.Pmp, cts);
                        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, TestPort, TestPort, "TDU2"));
                        type = Forwarding.Natpmp;
                        upnpSuccess = true;
                        mappedPort = TestPort;
                        statusMessage = $"Port {TestPort} successfully mapped via NAT-PMP";
                        Console.WriteLine(statusMessage);
                    }
                    catch (Exception exPmp)
                    {
                        type = Forwarding.None;
                        upnpSuccess = false;
                        mappedPort = 0;
                        statusMessage = "Port mapping failed (UPnP + NAT-PMP)\nDevice does not support it or access was denied";
                        Log.Log.Error(statusMessage + " : " + exPmp.Message);
                    }
                }
            }
            else
            {
                statusMessage = "UPnP/NAT-PMP skipped by user";
            }
            Result.ForwardingType = type;
            Result.UpnpAvailable = upnpSuccess;
            Result.UpnpPort = mappedPort;
            Result.UpnpStatus = statusMessage;
            Result.AfterUpnp = await Detect(TestPort);
            Result.AfterUpnp.ExternalReachable = await CheckExternalReachability(Result.AfterUpnp.PublicIp, Result.AfterUpnp.Port);
            listenerCts.Cancel();

            if (CommandLineArg.AutoRun)
                BackgroundWorker.RunGame();
        }

        /// <summary>
        /// Performs a STUN test to detect the NAT type and public IP address.
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static async Task<NatResult> Detect(int port)
        {
            var result = new NatResult
            {
                Status = NATStatus.Unknown,
                PublicIp = null,
                Port = port,
                ExternalReachable = false
            };

            try
            {
                using var client = new HttpClient();
                var publicIpStr = await client.GetStringAsync("https://api.ipify.org");
                var publicIp = IPAddress.Parse(publicIpStr);
                var server = new IPEndPoint(Dns.GetHostAddresses("stun1.l.google.com")[0], 3478);
                #region Stun Test IP and Nat
                var stunResult = await STUNClient.QueryAsync(server, STUNQueryType.PublicIP, true);
                var stunIp = stunResult.PublicEndPoint?.Address;
                var natResult = await STUNClient.QueryAsync(server, STUNQueryType.OpenNAT, true);
                #endregion
                var stunNatType = stunResult.NATType;
                bool hasIpv6 = Dns.GetHostAddresses(Dns.GetHostName()).Any(ip => ip.AddressFamily == AddressFamily.InterNetworkV6 && !ip.IsIPv6LinkLocal);
                result.PublicIp = stunIp ?? publicIp;
                if (stunIp == null)
                {
                    result.Status = NATStatus.Blocked;
                    return result;
                }

                if (IsCgnat(publicIp))
                {
                    result.Status = hasIpv6 ? NATStatus.DSLite : NATStatus.CGNAT;
                    return result;
                }

                if (IsPrivate(publicIp))
                {
                    result.Status = hasIpv6 ? NATStatus.DSLite : NATStatus.Strict;
                    return result;
                }

                Log.Log.Info($"Public IP from STUN: {stunIp}, Public IP from ipify: {publicIp}, NAT Type: {natResult.NATType}, Has IPv6: {IsPrivate(publicIp)} Has CGNat: {IsCgnat(publicIp)} IP: {stunIp}");

                switch (natResult.NATType)
                {
                    case STUNNATType.OpenInternet:
                    case STUNNATType.FullCone:
                        result.Status = NATStatus.Open;
                        break;
                    case STUNNATType.SymmetricUDPFirewall:
                        result.Status = NATStatus.Strict;
                        break;
                    case STUNNATType.PortRestricted:
                        result.Status = NATStatus.Blocked;
                        break;
                    default:
                        result.Status = NATStatus.Moderate;
                        break;
                }
            }
            catch
            {
                result.Status = NATStatus.Unknown;
            }

            return result;
        }

        /// <summary>
        /// Checks if the given IP and port are reachable from an external service.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private static async Task<bool> CheckExternalReachability(IPAddress ip, int port)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync($"{ExternalCheckUrl}?ip={ip}&port={port}");
                var result = await response.Content.ReadAsStringAsync();
                return result.Contains("OK");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Starts a UDP listener that echoes received messages back to the sender.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task StartEchoListener(int port, CancellationToken token)
        {
            using var udp = new UdpClient(port);

            Log.Log.Info($"NAT test listener started on port {port}.");
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var result = await udp.ReceiveAsync();
                    if (token.IsCancellationRequested) break;
                    string msg = Encoding.UTF8.GetString(result.Buffer);
                    if (msg == "NAT_TEST")
                    {
                        Result.InternalReachable = true;
                        Result.AfterUpnp.InternalReachable = true;
                        BackgroundWorker.MyNatType = "FullCone";
                        Regestry.UpdateKey("NetworkNatType", "FullCone", Database.Database.p2Database.Usersettings.IsSteambuild);
                        Log.Log.Info($"Received NAT test packet from Server: {msg}");
                    }
                    else
                    {
                        Result.AfterUpnp.Status = NATStatus.Blocked;
                        Regestry.UpdateKey("NetworkNatType", "Strict:Blocked", Database.Database.p2Database.Usersettings.IsSteambuild);
                        Log.Log.Warning($"Unexpected packet received from Server: {msg}");
                    }

                    byte[] echo = Encoding.UTF8.GetBytes("ECHO:" + msg);
                    await udp.SendAsync(echo, echo.Length, result.RemoteEndPoint);
                    Log.Log.Debug($"Nat reply sent to Server: ECHO:{msg}");
                }
                catch (OperationCanceledException)
                {
                    Log.Log.Info("NAT test listener was canceled.");
                }
                catch (Exception ex)
                {
                    Log.Log.Error($"NAT test listener error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Check CGNat
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsCgnat(IPAddress ip)
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork) return false;
            var b = ip.GetAddressBytes();
            return b[0] == 100 && b[1] >= 64 && b[1] <= 127;
        }

        /// <summary>
        /// Check Private IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsPrivate(IPAddress ip)
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork) return false;
            var b = ip.GetAddressBytes();
            return b[0] == 10 ||
                   (b[0] == 172 && b[1] >= 16 && b[1] <= 31) ||
                   (b[0] == 192 && b[1] == 168);
        }
    }
}