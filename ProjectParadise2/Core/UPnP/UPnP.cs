using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectParadise2.Core.UPnP
{
    /// <summary>
    /// Represents network protocol types for port mapping.
    /// </summary>
    public enum Protocol
    {
        /// <summary>
        /// Unknown protocol type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Transmission Control Protocol (TCP).
        /// </summary>
        TCP = 1,

        /// <summary>
        /// User Datagram Protocol (UDP).
        /// </summary>
        UDP = 2
    }

    /// <summary>
    /// Provides methods for managing Universal Plug and Play (UPnP) operations, such as port forwarding and gateway discovery.
    /// </summary>
    public static class UPnP
    {
        private static bool searching = true;
        private static Gateway defaultGateway = null;

        /// <summary>
        /// Static constructor that initiates the process of finding a gateway.
        /// </summary>
        static UPnP()
        {
            FindGateway();
        }

        /// <summary>
        /// Gets the default gateway discovered for UPnP.
        /// Blocks until a gateway is found.
        /// </summary>
        private static Gateway Gateway
        {
            get
            {
                while (searching)
                {
                    Thread.Sleep(1);
                }

                return defaultGateway;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a UPnP gateway is available.
        /// </summary>
        public static bool IsAvailable => Gateway != null;

        /// <summary>
        /// Gets the external IP address of the UPnP gateway.
        /// </summary>
        public static IPAddress ExternalIP => Gateway?.ExternalIPAddress;

        /// <summary>
        /// Gets the local IP address of the device that is connected to the gateway.
        /// </summary>
        public static IPAddress LocalIP => Gateway?.InternalClient;

        /// <summary>
        /// Opens a port mapping on the UPnP gateway.
        /// </summary>
        /// <param name="protocol">The protocol (TCP/UDP) to open the port for.</param>
        /// <param name="externalPort">The external port to be opened on the gateway.</param>
        /// <param name="internalPort">The internal port to be forwarded (optional, defaults to the external port).</param>
        /// <param name="description">A description for the port mapping (optional).</param>
        public static void Open(Protocol protocol, ushort externalPort, ushort? internalPort = null, string description = null) => Gateway?.AddPortMapping(externalPort, protocol, internalPort, description);

        /// <summary>
        /// Closes an existing port mapping on the UPnP gateway.
        /// </summary>
        /// <param name="protocol">The protocol (TCP/UDP) for the port to be closed.</param>
        /// <param name="externalPort">The external port to be closed on the gateway.</param>
        public static void Close(Protocol protocol, ushort externalPort) => Gateway?.DeletePortMapping(externalPort, protocol);

        /// <summary>
        /// Checks if a specific port is open on the UPnP gateway.
        /// </summary>
        /// <param name="protocol">The protocol (TCP/UDP) for the port.</param>
        /// <param name="externalPort">The external port to check.</param>
        /// <returns>True if the port is open, otherwise false.</returns>
        public static bool IsOpen(Protocol protocol, ushort externalPort) => Gateway?.SpecificPortMappingExists(externalPort, protocol) ?? false;

        /// <summary>
        /// Retrieves a generic port mapping entry from the UPnP gateway.
        /// </summary>
        /// <param name="portMappingIndex">The index of the port mapping entry to retrieve.</param>
        /// <returns>A dictionary containing port mapping details.</returns>
        public static Dictionary<string, string> GetGenericPortMappingEntry(int portMappingIndex) => Gateway?.GetGenericPortMappingEntry(portMappingIndex);

        /// <summary>
        /// Initiates the process of finding a UPnP gateway by listening on all local IP addresses.
        /// </summary>
        public static void FindGateway()
        {
            List<Task> listeners = new List<Task>();

            foreach (var ip in GetLocalIPs())
            {
                listeners.Add(Task.Run(() => StartListener(ip)));
            }

            Task.WhenAll(listeners).ContinueWith(t => searching = false);
        }

        /// <summary>
        /// Listens for UPnP gateways on a specific local IP address.
        /// </summary>
        /// <param name="ip">The local IP address to listen on.</param>
        private static void StartListener(IPAddress ip)
        {
            if (Gateway.TryNew(ip, out Gateway gateway))
            {
                Interlocked.CompareExchange(ref defaultGateway, gateway, null);
                searching = false;
            }
        }

        /// <summary>
        /// Gets a list of local IP addresses on the machine.
        /// </summary>
        /// <returns>A collection of local IP addresses.</returns>
        private static IEnumerable<IPAddress> GetLocalIPs() => NetworkInterface.GetAllNetworkInterfaces().Where(IsValidInterface).SelectMany(GetValidNetworkIPs);

        /// <summary>
        /// Determines whether a network interface is valid for UPnP discovery.
        /// </summary>
        /// <param name="network">The network interface to check.</param>
        /// <returns>True if the network interface is valid, otherwise false.</returns>
        private static bool IsValidInterface(NetworkInterface network)
            => network.OperationalStatus == OperationalStatus.Up
            && network.NetworkInterfaceType != NetworkInterfaceType.Loopback
            && network.NetworkInterfaceType != NetworkInterfaceType.Ppp;

        /// <summary>
        /// Gets the valid network IP addresses for a given network interface.
        /// </summary>
        /// <param name="network">The network interface to retrieve IP addresses from.</param>
        /// <returns>A collection of valid network IP addresses.</returns>
        private static IEnumerable<IPAddress> GetValidNetworkIPs(NetworkInterface network) => network.GetIPProperties().UnicastAddresses
            .Select(a => a.Address)
            .Where(a => a.AddressFamily == AddressFamily.InterNetwork || a.AddressFamily == AddressFamily.InterNetworkV6);
    }
}
