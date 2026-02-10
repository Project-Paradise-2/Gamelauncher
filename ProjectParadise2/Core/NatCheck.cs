using Open.Nat;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Views;
using STUN;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectParadise2
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
        public NATStatus Status { get; set; } = NATStatus.Unknown;
        public IPAddress PublicIp { get; set; } = null;
        public int Port { get; set; }
        public bool ExternalReachable { get; set; } = false;
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
        public NatResult AfterUpnp { get; set; } = new NatResult();
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
        private static string ExternalCheckUrl = $"http://{Constans.Server}:9024/conncheck/check";

        public static NatTestResult Result { get; set; }
        public static bool CanRun { get; set; } = false;
        public static Mapping CurrentMapping { get; set; }

        /// <summary>
        /// Runs the NAT detection test, including UPnP port mapping and STUN tests, and updates the NatTestResult with the results.
        /// </summary>
        /// <returns></returns>
        public static async Task RunTest()
        {
            NatDevice? device = null;
            bool upnpSuccess = false;
            int mappedPort = 0;
            string statusMessage = "Not checked";
            Forwarding type = Forwarding.None;
            Result = new NatTestResult();
            if (Database.Database.p2Database.Usersettings.UpnpWorker.Equals(true))
            {
                try
                {
                    var discoverer = new NatDiscoverer();
                    using var cts = new CancellationTokenSource(5000);
                    device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
                    type = Forwarding.Upnp;
                    ///Mapping now for 24hours.. 
                    CurrentMapping = new Mapping(Protocol.Udp, TestPort, TestPort, 86400000, "TDU2");
                    await device.CreatePortMapAsync(CurrentMapping);

                    upnpSuccess = true;
                    mappedPort = TestPort;
                    statusMessage = $"Port {TestPort} successfully mapped via UPnP";
                    Log.Info(statusMessage);
                }
                catch
                {
                    try
                    {
                        var discoverer = new NatDiscoverer();
                        using var cts = new CancellationTokenSource(5000);
                        device = await discoverer.DiscoverDeviceAsync(PortMapper.Pmp, cts);
                        await device.CreatePortMapAsync(CurrentMapping);
                        type = Forwarding.Natpmp;
                        upnpSuccess = true;
                        mappedPort = TestPort;
                        statusMessage = $"Port {TestPort} successfully mapped via NAT-PMP";
                        Log.Info(statusMessage);
                    }
                    catch (Exception exPmp)
                    {
                        type = Forwarding.None;
                        upnpSuccess = false;
                        mappedPort = 0;
                        statusMessage = "Port mapping failed (UPnP + NAT-PMP)\nDevice does not support it or access was denied";
                        Log.Error(statusMessage + " : " + exPmp.Message);
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
            GetServerResponse(TestPort);
            Result.AfterUpnp.ExternalReachable = await CheckExternalReachability(Result.AfterUpnp.PublicIp, Result.AfterUpnp.Port);
            CanRun = true;
            if (CommandLineArg.AutoRun)
                BackgroundWorker.RunGame();
        }

        public static async Task RemoveUpnp()
        {
            try
            {
                var discoverer = new NatDiscoverer();
                using var cts = new CancellationTokenSource(5000);
                var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                try
                {
                    await device.DeletePortMapAsync(CurrentMapping);
                    Log.Info($"Port {CurrentMapping.PrivatePort} unmapped successfully.");
                }
                catch (Exception ex)
                {
                    Log.Error("Error while cleaning up port mapping: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error while finding UPnP device: " + ex.Message);
            }
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

                Log.Info($"Public IP from STUN: {stunIp}, Public IP from ipify: {publicIp}, NAT Type: {natResult.NATType}, Has IPv6: {IsPrivate(publicIp)} Has CGNat: {IsCgnat(publicIp)}");

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
            Debug.WriteLine($"NAT Detection Result: {result}");
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
                string ServerURI = $"{ExternalCheckUrl}?ip={ip}&port={port}";
                Debug.WriteLine($"Checking external reachability via {ServerURI}");
                var response = await client.GetAsync(ServerURI);
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"External reachability check result: {result}");
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
        private static async Task GetServerResponse(int port)
        {
            string Statemsg = "No response from the server. Your connection may be blocked.";
            var udp = new UdpClient(port);
            Log.Info($"Listening on {udp.Client.LocalEndPoint}");
            try
            {
                var receiveTask = udp.ReceiveAsync();
                if (await Task.WhenAny(receiveTask, Task.Delay(5000)) == receiveTask)
                {
                    var result = await receiveTask;
                    string msg = Encoding.UTF8.GetString(result.Buffer);
                    Log.Info($"Received UDP message: {msg} from {result.RemoteEndPoint}");

                    if (msg == "NAT_TEST")
                    {
                        Statemsg = $"NAT test packet received successfully: {msg}";
                        Result.InternalReachable = true;
                        Result.AfterUpnp.InternalReachable = true;
                        BackgroundWorker.MyNatType = "FullCone";
                        Regestry.UpdateKey("NetworkNatType", "FullCone", Database.Database.p2Database.Usersettings.IsSteambuild);
                    }
                    else
                    {
                        Statemsg = $"Unexpected response from server: {msg}";
                        Result.AfterUpnp.Status = NATStatus.Blocked;
                        Regestry.UpdateKey("NetworkNatType", "Strict:Blocked", Database.Database.p2Database.Usersettings.IsSteambuild);
                    }

                    byte[] echo = Encoding.UTF8.GetBytes("ECHO:" + msg);
                    await udp.SendAsync(echo, echo.Length, result.RemoteEndPoint);
                    Statemsg += $", sent confirmation back to server (ECHO:{msg})";
                }
                else
                {
                    Log.Info("NAT test listener timed out after 5 seconds.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in NAT test listener: {ex.Message}");
            }
            finally
            {
                udp.Dispose();
            }

            CanRun = true;
            HomeView.Instance?.ShowRunGameButton();
            Log.Info(Statemsg);
        }

        /// <summary>
        /// Check if IPv4 address is in CGNAT range (100.64.0.0/10).
        /// </summary>
        public static bool IsCgnat(IPAddress ip)
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork)
                return false;

            var b = ip.GetAddressBytes();
            return b[0] == 100 && b[1] >= 64 && b[1] <= 127;
        }

        /// <summary>
        /// Check if IP address is private (IPv4 RFC1918, IPv6 ULA/LinkLocal/Loopback).
        /// </summary>
        public static bool IsPrivate(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                var b = ip.GetAddressBytes();
                return b[0] == 10 ||
                       (b[0] == 172 && b[1] >= 16 && b[1] <= 31) ||
                       (b[0] == 192 && b[1] == 168);
            }
            else if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                var b = ip.GetAddressBytes();

                // Unique Local Address (fc00::/7 → fc00::/8 und fd00::/8)
                if ((b[0] & 0xFE) == 0xFC) return true;

                // Link-Local (fe80::/10)
                if (b[0] == 0xFE && (b[1] & 0xC0) == 0x80) return true;

                // Loopback (::1)
                if (ip.Equals(IPAddress.IPv6Loopback)) return true;
            }

            return false;
        }
    }
}