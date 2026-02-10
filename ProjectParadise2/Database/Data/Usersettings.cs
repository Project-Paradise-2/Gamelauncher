using System;

namespace ProjectParadise2.Database.Data
{
    /// <summary>
    /// Represents the user settings for the Project Paradise 2 application, including configuration options for the launcher, game, and backups.
    /// </summary>
    [Serializable]
    public class Usersettings
    {
        /// <summary>
        /// The version of the launcher.
        /// </summary>
        public string Launcherverion = Constans.LauncherVersion;

        /// <summary>
        /// The in-game name of the user.
        /// </summary>
        public string Ingamename = "";

        /// <summary>
        /// The lifetime of a mutex in minutes. Default is 30 minutes.
        /// </summary>
        public int Mutexlivetime { get; set; } = 30;

        /// <summary>
        /// The account name of the user.
        /// </summary>
        public string Acountname { get; set; }

        /// <summary>
        /// The account password of the user.
        /// </summary>
        public string Accountpassword { get; set; }

        /// <summary>
        /// Indicates whether the user is logged in.
        /// </summary>
        public bool IsLogged { get; set; }

        /// <summary>
        /// The directory where the game is installed.
        /// </summary>
        public string Gamedirectory { get; set; } = "";

        /// <summary>
        /// The path to the executable file of the game.
        /// </summary>
        public string ExePath { get; set; } = "";

        /// <summary>
        /// The file version of the game.
        /// </summary>
        public string FileVersion { get; set; } = "";

        /// <summary>
        /// The product version of the game.
        /// </summary>
        public string ProductVersion { get; set; } = "";

        /// <summary>
        /// A flag indicating whether the application is in online mode.
        /// </summary>
        public bool Onlinemode { get; set; } = true;

        /// <summary>
        /// A flag indicating whether the game is packed.
        /// </summary>
        public bool Packedgame { get; set; } = true;

        /// <summary>
        /// A flag indicating whether Discord Rich Presence (RPC) is enabled.
        /// </summary>
        public bool DiscordRPC { get; set; } = true;

        /// <summary>
        /// A flag indicating whether auto-update checks are enabled.
        /// </summary>
        public bool Autoupdatecheck { get; set; } = true;

        /// <summary>
        /// A flag indicating whether UPnP (Universal Plug and Play) is enabled for networking tasks.
        /// </summary>
        public bool UpnpWorker { get; set; } = true;

        /// <summary>
        /// A flag indicating whether launcher logging is enabled.
        /// </summary>
        public bool Launcherlogging { get; set; } = true;

        /// <summary>
        /// The language preference for the user interface (e.g., "en-EN" for English).
        /// </summary>
        public string Userlang { get; set; } = "en-EN";

        public int LangId { get; set; } = 1;

        /// <summary>
        /// A flag indicating whether the user is using a Steam build of the game.
        /// </summary>
        public bool IsSteambuild { get; set; } = false;

        /// <summary>
        /// A flag indicating whether local backups are enabled.
        /// </summary>
        public bool LocalBackup { get; set; } = false;

        /// <summary>
        /// The number of backups to keep. Default is 2.
        /// </summary>
        public int NumOfBackups { get; set; } = 2;

        /// <summary>
        /// The type of backup strategy (e.g., "OnStart").
        /// </summary>
        public BackUptype BackupType { get; set; } = BackUptype.OnStart;

        public int SelectedProfile { get; set; }
    }
}
