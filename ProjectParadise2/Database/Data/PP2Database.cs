using System;

namespace ProjectParadise2.Database.Data
{
    /// <summary>
    /// Represents the database for Project Paradise 2, storing user settings, a list of user-installed mods, and the database version.
    /// </summary>
    [Serializable]
    public class PP2Database
    {
        /// <summary>
        /// The user settings, including launcher preferences, game configurations, and other user-specific options.
        /// </summary>
        public Usersettings Usersettings { get; set; } = new Usersettings();

        /// <summary>
        /// The version of the database schema. Default value is "1.0.4".
        /// </summary>
        public string DatabaseVersion { get; set; } = "2.0.0";
    }
}