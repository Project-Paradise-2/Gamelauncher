using System;

namespace ProjectParadise2
{
    /// <summary>
    /// Represents the build type of the launcher.
    /// </summary>
    public enum Launcherbuild
    {
        /// <summary>
        /// Indicates the launcher is in packed build mode.
        /// </summary>
        Packed,

        /// <summary>
        /// Indicates the launcher is in unpacked build mode.
        /// </summary>
        Unpacked,
    }

    /// <summary>
    /// Contains constants and configuration values for the launcher.
    /// </summary>
    internal class Constans
    {
        /// <summary>
        /// The IP address of the server.
        /// </summary>
        internal const string ServerIP = "194.164.199.240";

        /// <summary>
        /// The version of the launcher.
        /// </summary>
        internal const string LauncherVersion = "3.0.12";

        /// <summary>
        /// The path to the documents folder for Project Paradise 2.
        /// </summary>
        internal static string DokumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Project-Paradise 2/";

        /// <summary>
        /// The path to the save folder for the game.
        /// </summary>
        internal static string SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Eden Games\Test Drive Unlimited 2\savegame\";

        /// <summary>
        /// The URL for the CDN hosting resources.
        /// </summary>
        internal const string Cdn = "https://cdn.project-paradise2.de";

        /// <summary>
        /// The URL for the mod loader.
        /// </summary>
        internal const string Modloader = "https://mods.project-paradise2.de/TDU2Mods";

        /// <summary>
        /// The registry key for the Steam game key.
        /// </summary>
        internal const string SteamGamekey = @"SOFTWARE\WOW6432Node\Atari\TDU2\Steam";

        /// <summary>
        /// The registry key for the game key.
        /// </summary>
        internal const string Gamekey = @"SOFTWARE\WOW6432Node\Atari\TDU2";

        /// <summary>
        /// The current build mode of the launcher.
        /// </summary>
        internal const Launcherbuild launcherbuild = Launcherbuild.Unpacked;
    }
}