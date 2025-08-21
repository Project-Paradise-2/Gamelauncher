using Microsoft.Win32;
using ProjectParadise2.Core.Classes;
using System;

namespace ProjectParadise2.Core
{
    /// <summary>
    /// Handles the interaction with the Windows registry to manage game installation and settings.
    /// </summary>
    class Regestry
    {
        /// <summary>
        /// List of NAT types for the game.
        /// </summary>
        public static string[] TDU2Nattype =
{
            "Strict:UncheckedInternet",
            "Strict:Blocked",
            "FullCone",
            "Open"
        };

        /// <summary>
        /// Retrieves the game installation path from the registry, either from Steam or a non-Steam installation.
        /// </summary>
        /// <returns>The installation path of the game.</returns>
        public static string GetPath()
        {
            string path = string.Empty;
            path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
            try
            {
                RegistryKey steamKey = Registry.LocalMachine.OpenSubKey(Constans.SteamGamekey, true);
                RegistryKey noSteamKey = Registry.LocalMachine.OpenSubKey(Constans.Gamekey, true);

                if (steamKey != null && steamKey.GetValue("installdir") != null)
                {
                    path = steamKey.GetValue("installdir").ToString();
                }
                else if (noSteamKey != null && noSteamKey.GetValue("installdir") != null)
                {
                    path = noSteamKey.GetValue("installdir").ToString();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: {ex.Message}", ex);
            }
            return path;
        }

        /// <summary>
        /// Updates the game registry keys, adding default values if they don't exist.
        /// </summary>
        /// <param name="IsSteam">Indicates if the installation is Steam-based.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public static bool UpdateKey(bool IsSteam = false)
        {
            try
            {
                if (IsSteam)
                {
                    RegistryKey Steam = Registry.LocalMachine.OpenSubKey(Constans.SteamGamekey, true);
                    if (Steam == null)
                        Registry.LocalMachine.CreateSubKey(Constans.SteamGamekey);
                    ExistKey("AudioLib", true);
                    ExistKey("ExePath", true);
                    ExistKey("GameProductVersion", true);
                    ExistKey("GUID", true);
                    ExistKey("installdir", true);
                    ExistKey("language", true);
                    ExistKey("languagePack", true);
                    ExistKey("LauncherPath", true);
                    ExistKey("NetworkNatType", true);
                    ExistKey("GameLastPlayerId", true);
                    ExistKey("Serial", true);
                    ExistKey("Project-Paradise2", true);
                }
                else
                {
                    RegistryKey NoSteam = Registry.LocalMachine.OpenSubKey(Constans.Gamekey, true);
                    if (NoSteam == null)
                        Registry.LocalMachine.CreateSubKey(Constans.Gamekey);
                    ExistKey("AudioLib");
                    ExistKey("ExePath");
                    ExistKey("GameProductVersion");
                    ExistKey("GUID");
                    ExistKey("installdir");
                    ExistKey("language");
                    ExistKey("languagePack");
                    ExistKey("LauncherPath");
                    ExistKey("NetworkNatType");
                    ExistKey("GameLastPlayerId");
                    ExistKey("Serial");
                    ExistKey("Project-Paradise2");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: {ex.Message}", ex);
                return false;
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: {ex.Message}", ex);
                return false;
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: {ex.Message}", ex);
                return false;
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: {ex.Message}", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves the audio mode setting from the system registry.
        /// Checks both Steam and non-Steam registry entries to determine the current audio mode.
        /// Default audio mode is set to <see cref="AudioMode.DirectSound"/>. 
        /// If the audio library is set to "XAudio2" in either registry key, the audio mode is changed to <see cref="AudioMode.XAudio2"/>.
        /// </summary>
        /// <returns>The current audio mode, which can either be <see cref="AudioMode.DirectSound"/> or <see cref="AudioMode.XAudio2"/>.</returns>
        public static AudioMode GetAudiosetting()
        {
            AudioMode audioMode = AudioMode.DirectSound;
            try
            {
                RegistryKey NoSteam = Registry.LocalMachine.OpenSubKey(Constans.Gamekey, true);
                RegistryKey Steam = Registry.LocalMachine.OpenSubKey(Constans.SteamGamekey, true);

                // Check if the AudioLib key exists and is set to "XAudio2"
                if (Steam?.GetValue("AudioLib") != null)
                    if (Steam.GetValue("AudioLib").ToString().Equals("XAudio2"))
                        audioMode = AudioMode.XAudio2;

                if (NoSteam?.GetValue("AudioLib") != null)
                    if (NoSteam.GetValue("AudioLib").ToString().Equals("XAudio2"))
                        audioMode = AudioMode.XAudio2;

            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: {ex.Message}", ex);
            }
            return audioMode;
        }

        /// <summary>
        /// Sets the audio mode in the system registry for both Steam and non-Steam entries.
        /// The <see cref="AudioMode"/> is updated in the registry key for both Steam and non-Steam versions of the game.
        /// </summary>
        /// <param name="audioMode">The audio mode to be set. It should be either <see cref="AudioMode.DirectSound"/> or <see cref="AudioMode.XAudio2"/>.</param>
        public static void SetAudiomode(AudioMode audioMode)
        {
            try
            {
                RegistryKey NoSteam = Registry.LocalMachine.OpenSubKey(Constans.Gamekey, true);
                RegistryKey Steam = Registry.LocalMachine.OpenSubKey(Constans.SteamGamekey, true);

                // Update the AudioLib registry key for Steam and non-Steam versions
                if (Steam?.GetValue("AudioLib") != null)
                    Steam.SetValue("AudioLib", audioMode.ToString());

                if (NoSteam?.GetValue("AudioLib") != null)
                    NoSteam.SetValue("AudioLib", audioMode.ToString());
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the installation paths (ExePath, installdir, and LauncherPath) in the registry.
        /// </summary>
        /// <param name="gameExe">The game executable path.</param>
        /// <param name="installdir">The installation directory path.</param>
        /// <param name="launcherPath">The launcher path.</param>
        /// <param name="isSteam">Indicates if the installation is Steam-based.</param>
        public static void UpdatePath(string gameExe, string installdir, string launcherPath, bool isSteam = false)
        {
            try
            {
                string registryPath = isSteam ? Constans.SteamGamekey : Constans.Gamekey;
                RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(registryPath, true);
                baseKey.SetValue("ExePath", gameExe);
                baseKey.SetValue("installdir", installdir);
                baseKey.SetValue("LauncherPath", launcherPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates or creates a specific registry key with a value.
        /// </summary>
        /// <param name="keyName">The registry key name.</param>
        /// <param name="keyValue">The value to set for the registry key.</param>
        /// <param name="isSteam">Indicates if the installation is Steam-based.</param
        public static void UpdateKey(string keyName, string keyValue, bool isSteam = false)
        {
            string registryPath = "";
            try
            {
                registryPath = isSteam ? Constans.SteamGamekey : Constans.Gamekey;
                RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(registryPath, true);
                baseKey.SetValue(keyName, keyValue);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: ", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: ", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: ", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: ", ex);
            }
        }

        /// <summary>
        /// Ensures a registry key exists. If it doesn't, it will be created with a default value.
        /// </summary>
        /// <param name="keyName">The name of the registry key.</param>
        /// <param name="isSteam">Indicates if the installation is Steam-based.</param>
        /// <returns>True if the key exists or was created.</returns>
        public static bool ExistKey(string keyName, bool isSteam = false)
        {
            try
            {
                string registryPath = isSteam ? Constans.SteamGamekey : Constans.Gamekey;
                RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(registryPath, true);
                if (baseKey.GetValue(keyName) == null)
                {
                    baseKey.SetValue(keyName, string.Empty);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Print($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Print($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Print($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"An error occurred: {ex.Message}", ex);
            }
            return true;
        }
    }
}