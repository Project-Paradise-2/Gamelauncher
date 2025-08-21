using System;
using System.Net;

namespace ProjectParadise2.Core
{
    /// <summary>
    /// Custom WebClient class to manage web requests with configurable timeout.
    /// Inherits from WebClient and overrides the GetWebRequest method
    /// to adjust the timeout for web requests.
    /// </summary>
    internal class WebConnection : WebClient
    {
        /// <summary>
        /// Gets or sets the timeout duration in seconds.
        /// The default value is 10 seconds.
        /// </summary>
        internal int Timeout { get; set; } = 10;

        /// <summary>
        /// Overrides the GetWebRequest method to set a custom timeout for web requests.
        /// Converts the Timeout property (in seconds) to milliseconds and applies it
        /// to the WebRequest timeout.
        /// </summary>
        /// <param name="Address">The URI of the resource to request.</param>
        /// <returns>A WebRequest object with the specified timeout.</returns>
        protected override WebRequest GetWebRequest(Uri Address)
        {
            // Call the base method to create the web request
            WebRequest WebReq = base.GetWebRequest(Address);
            // Set the timeout in milliseconds (Timeout * 1000)
            WebReq.Timeout = Timeout * 1000;
            // Return the modified web request
            return WebReq;
        }
    }
}