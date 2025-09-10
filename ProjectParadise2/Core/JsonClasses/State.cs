using System;

namespace ProjectParadise2.JsonClasses
{
    /// <summary>
    /// Represents the state of the application or server, including version, build details, and player count.
    /// </summary>
    [Serializable]
    public class State
    {
        /// <summary>
        /// The version of the application or server.
        /// </summary>
        public string version { get; set; } = "3.0.0";

        /// <summary>
        /// The build number or identifier of the application or server.
        /// </summary>
        public string build { get; set; } = "";

        /// <summary>
        /// The server build number, used for identifying the version of the server.
        /// </summary>
        public int serverbuild { get; set; }

        /// <summary>
        /// The current number of players connected to the server.
        /// </summary>
        public int playercount { get; set; }
    }
}
