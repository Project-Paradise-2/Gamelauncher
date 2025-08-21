using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace ProjectParadise2.Core.UPnP
{
    // UPnP Specifications http://upnp.org/specs/gw/UPnP-gw-WANIPConnection-v1-Service.pdf
    // Helpful overview http://www.upnp-hacks.org/igd.html
    // Port tester https://www.yougetsignal.com/tools/open-ports/
    // Originally based on a port of waifuPnP. Though it has been nearly completely rewritten by this point.
    internal sealed class Gateway
    {
        public IPAddress InternalClient { get; }
        private readonly string serviceType = null;
        private readonly string controlURL = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gateway"/> class using the specified IP address and data.
        /// </summary>
        private Gateway(IPAddress ip, string data)
        {
            InternalClient = ip;
            string location = GetLocation(data);
            (serviceType, controlURL) = GetInfo(location);
        }

        private static readonly string[] searchMessageTypes = new[]
        {
            "urn:schemas-upnp-org:device:InternetGatewayDevice:1",
            "urn:schemas-upnp-org:service:WANIPConnection:1",
            "urn:schemas-upnp-org:service:WANPPPConnection:1"
        };

        /// <summary>
        /// Attempts to discover a gateway device on the specified IP address.
        /// </summary>
        /// <param name="ip">The IP address of the local network to search for a gateway.</param>
        /// <param name="gateway">The discovered gateway if successful, otherwise null.</param>
        /// <returns>True if a gateway was successfully discovered; otherwise false.</returns>
        public static bool TryNew(IPAddress ip, out Gateway gateway)
        {
            IPEndPoint endPoint = IPEndPoint.Parse("239.255.255.250:1900");
            Socket socket = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = 3000,
                SendTimeout = 3000
            };

            socket.Bind(new IPEndPoint(ip, 0));
            byte[] buffer = new byte[0x600];
            foreach (string type in searchMessageTypes)
            {
                string request = string.Join
                (
                    "\n",
                    "M-SEARCH * HTTP/1.1",
                    $"HOST: {endPoint}",
                    $"ST: {type}",
                    "MAN: \"ssdp:discover\"",
                    "MX: 2",
                    "", "" // End with double newlines
                );
                byte[] req = Encoding.ASCII.GetBytes(request);

                try
                {
                    socket.SendTo(req, endPoint);
                    int receivedCount = socket.Receive(buffer);
                    gateway = new Gateway(ip, Encoding.ASCII.GetString(buffer, 0, receivedCount));
                    return true;
                }
                catch { }
            }
            gateway = null;
            return false;
        }

        /// <summary>
        /// Extracts the location URL from the received data.
        /// </summary>
        /// <param name="data">The data containing the location information.</param>
        /// <returns>The location URL.</returns>
        private static string GetLocation(string data)
        {
            var lines = data.Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0);

            foreach (string line in lines)
            {
                if (line.StartsWith("HTTP/1.") || line.StartsWith("NOTIFY *"))
                    continue;

                int colonIndex = line.IndexOf(':');
                if (colonIndex < 0)
                    continue;

                string name = line[..colonIndex];
                string val = line.Length >= name.Length ? line[(colonIndex + 1)..].Trim() : null;

                if (name.ToLowerInvariant() == "location")
                {
                    if (val.IndexOf('/', 7) == -1) // Finds the first slash after http://
                    {
                        Log.Log.Print("Unsupported Gateway");
                        throw new Exception("Unsupported Gateway");
                    }
                    return val;
                }
            }
            Log.Log.Print("Unsupported Gateway");
            throw new Exception("Unsupported Gateway");
        }

        /// <summary>
        /// Retrieves service type and control URL from the specified location.
        /// </summary>
        /// <param name="location">The location URL where the gateway's service details are found.</param>
        /// <returns>A tuple containing the service type and control URL.</returns>
        private static (string serviceType, string controlURL) GetInfo(string location)
        {
            XDocument doc = XDocument.Load(location);
            var services = doc.Descendants().Where(d => d.Name.LocalName == "service");
            (string serviceType, string controlURL) ret = (null, null);
            foreach (XElement service in services)
            {
                string serviceType = null;
                string controlURL = null;
                foreach (var node in service.Nodes())
                {
                    if (!(node is XElement ele) || !(ele.FirstNode is XText n))
                    {
                        continue;
                    }

                    switch (ele.Name.LocalName.Trim().ToLowerInvariant())
                    {
                        case "servicetype": serviceType = n.Value.Trim(); break;
                        case "controlurl": controlURL = n.Value.Trim(); break;
                    }
                }

                if (serviceType != null && controlURL != null)
                {
                    if (serviceType.ToLowerInvariant().Contains(":wanipconnection:") || serviceType.ToLowerInvariant().Contains(":wanpppconnection:"))
                    {
                        ret.serviceType = serviceType;
                        ret.controlURL = controlURL;
                    }
                }
            }

            if (ret.controlURL is null)
            {
                Log.Log.Print("Unsupported Gateway");
                throw new Exception("Unsupported Gateway");
            }

            if (!ret.controlURL.StartsWith('/'))
            {
                ret.controlURL = "/" + ret.controlURL;
            }

            int slash = location.IndexOf('/', 7); // Finds the first slash after http://
            ret.controlURL = location[0..slash] + ret.controlURL;
            return ret;
        }

        /// <summary>
        /// Builds a string representation of the provided argument key-value pair.
        /// </summary>
        /// <param name="arg">The key-value pair to build the argument string.</param>
        /// <returns>The formatted argument string.</returns>
        private static string BuildArgString((string Key, object Value) arg) => $"<{arg.Key}>{arg.Value}</{arg.Key}>";

        /// <summary>
        /// Sends a SOAP request to the gateway and retrieves the response.
        /// </summary>
        /// <param name="action">The action to perform in the SOAP request.</param>
        /// <param name="args">The arguments for the action.</param>
        /// <returns>A dictionary containing the response from the gateway.</returns>
        private Dictionary<string, string> RunCommand(string action, params (string Key, object Value)[] args)
        {
            string requestData = GetRequestData(action, args);
            HttpWebRequest request = SendRequest(action, requestData);
            return GetResponse(request);
        }

        /// <summary>
        /// Constructs the XML request data for a SOAP action.
        /// </summary>
        /// <param name="action">The action name to include in the SOAP request.</param>
        /// <param name="args">The arguments for the action.</param>
        /// <returns>The XML string representing the SOAP request data.</returns>
        private string GetRequestData(string action, (string Key, object Value)[] args) => string.Concat
        (
            "<?xml version=\"1.0\"?>\n",
            "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">",
                "<SOAP-ENV:Body>",
                    $"<m:{action} xmlns:m=\"{serviceType}\">",
                        string.Concat(args.Select(BuildArgString)),
                    $"</m:{action}>",
                "</SOAP-ENV:Body>",
            "</SOAP-ENV:Envelope>"
        );

        /// <summary>
        /// Sends a SOAP request to the control URL of the gateway.
        /// </summary>
        /// <param name="action">The action for the request.</param>
        /// <param name="requestData">The XML data to send in the request body.</param>
        /// <returns>The HTTP web request representing the SOAP request.</returns>
        private HttpWebRequest SendRequest(string action, string requestData)
        {
            byte[] data = Encoding.ASCII.GetBytes(requestData);
            HttpWebRequest request = WebRequest.CreateHttp(controlURL);
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = data.Length;
            request.Headers.Add("SOAPAction", $"\"{serviceType}#{action}\"");
            using Stream requestStream = request.GetRequestStream();
            requestStream.Write(data);
            return request;
        }

        /// <summary>
        /// Parses the response from a SOAP request into a dictionary of key-value pairs.
        /// </summary>
        /// <param name="request">The SOAP request sent to the gateway.</param>
        /// <returns>A dictionary containing the response data.</returns>
        private static Dictionary<string, string> GetResponse(HttpWebRequest request)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            try
            {
                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) { return null; }

                XDocument doc = XDocument.Load(response.GetResponseStream());
                foreach (XNode node in doc.DescendantNodes())
                {
                    if (node is XElement ele && ele.FirstNode is XText txt)
                    {
                        ret[ele.Name.LocalName] = txt.Value;
                    }
                }
            }
            catch { }

            if (ret.TryGetValue("errorCode", out string errorCode))
            {
                Log.Log.Print("Response error: " + errorCode);
            }
            return ret;
        }

        /// <summary>
        /// Gets the external IP address of the gateway.
        /// </summary>
        public IPAddress ExternalIPAddress => RunCommand("GetExternalIPAddress").TryGetValue("NewExternalIPAddress", out string ret) ? IPAddress.Parse(ret) : null;

        /// <summary>
        /// Checks if a specific port mapping exists on the gateway.
        /// </summary>
        /// <param name="externalPort">The external port to check.</param>
        /// <param name="protocol">The protocol (e.g., TCP or UDP).</param>
        /// <returns>True if the port mapping exists, otherwise false.</returns>
        public bool SpecificPortMappingExists(ushort externalPort, Protocol protocol) => RunCommand("GetSpecificPortMappingEntry",
            ("NewRemoteHost", ""),
            ("NewExternalPort", externalPort),
            ("NewProtocol", protocol)
        ).ContainsKey("NewInternalPort");

        /// <summary>
        /// Adds a port mapping to the gateway.
        /// </summary>
        /// <param name="externalPort">The external port to map.</param>
        /// <param name="protocol">The protocol (e.g., TCP or UDP).</param>
        /// <param name="internalPort">The internal port (optional).</param>
        /// <param name="description">A description for the port mapping (optional).</param>
        public void AddPortMapping(ushort externalPort, Protocol protocol, ushort? internalPort = null, string description = null) => RunCommand("AddPortMapping",
            ("NewRemoteHost", ""),
            ("NewExternalPort", externalPort),
            ("NewProtocol", protocol),
            ("NewInternalClient", InternalClient),
            ("NewInternalPort", internalPort ?? externalPort),
            ("NewEnabled", 1),
            ("NewPortMappingDescription", description ?? externalPort.ToString()),
            ("NewLeaseDuration", 0)
        );

        /// <summary>
        /// Deletes a port mapping from the gateway.
        /// </summary>
        /// <param name="externalPort">The external port to delete.</param>
        /// <param name="protocol">The protocol (e.g., TCP or UDP).</param>
        public void DeletePortMapping(ushort externalPort, Protocol protocol) => RunCommand("DeletePortMapping",
            ("NewRemoteHost", ""),
            ("NewExternalPort", externalPort),
            ("NewProtocol", protocol)
        );

        /// <summary>2.4.14.GetGenericPortMappingEntry</summary>
        public Dictionary<string, string> GetGenericPortMappingEntry(int portMappingIndex) => RunCommand("GetGenericPortMappingEntry",
            ("NewPortMappingIndex", portMappingIndex)
        );
    }
}