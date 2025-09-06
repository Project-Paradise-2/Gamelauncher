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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);

            string[] subKeyPaths = new string[]
            {
                @"SOFTWARE\Atari\TDU2\Steam",  // Steam
                @"SOFTWARE\Atari\TDU2"         // Non-Steam
            };

            try
            {
                foreach (var subKeyPath in subKeyPaths)
                {
                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                           .OpenSubKey(subKeyPath, writable: false))
                    {
                        if (baseKey == null)
                            continue;

                        object value = baseKey.GetValue("installdir");
                        if (value != null)
                        {
                            path = value.ToString();
                            break; // Wenn wir einen gültigen Pfad gefunden haben, abbrechen
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return path;
        }


        /// <summary>
        /// Updates the game registry keys, adding default values if they don't exist.
        /// </summary>
        /// <param name="IsSteam">Indicates if the installation is Steam-based.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public static bool UpdateKey(bool isSteam = false)
        {
            string subKeyPath = isSteam ? @"SOFTWARE\Atari\TDU2\Steam" : @"SOFTWARE\Atari\TDU2";
            string[] keysToEnsure = new string[]
            {
                "AudioLib",
                "ExePath",
                "GameProductVersion",
                "GUID",
                "installdir",
                "language",
                "languagePack",
                "LauncherPath",
                "NetworkNatType",
                "GameLastPlayerId",
                "Serial",
                "Project-Paradise2"
            };

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                       .OpenSubKey(subKeyPath, writable: true)
                       ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                     .CreateSubKey(subKeyPath, writable: true))
                {
                    if (baseKey == null)
                    {
                        Log.Log.Error($"Could not open or create registry key: {subKeyPath}");
                        return false;
                    }

                    foreach (var keyName in keysToEnsure)
                    {
                        if (baseKey.GetValue(keyName) == null)
                        {
                            baseKey.SetValue(keyName, string.Empty);
                        }
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return false;
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

            string[] subKeyPaths = new string[]
            {
                @"SOFTWARE\Atari\TDU2",          // Non-Steam
                @"SOFTWARE\Atari\TDU2\Steam"     // Steam
            };

            try
            {
                foreach (var subKeyPath in subKeyPaths)
                {
                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                           .OpenSubKey(subKeyPath, writable: false))
                    {
                        if (baseKey == null)
                            continue;

                        object value = baseKey.GetValue("AudioLib");
                        if (value != null && value.ToString().Equals("XAudio2", StringComparison.OrdinalIgnoreCase))
                        {
                            audioMode = AudioMode.XAudio2;
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return audioMode;
        }


        /// <summary>
        /// Sets the audio mode in the system registry for both Steam and non-Steam entries.
        /// The <see cref="AudioMode"/> is updated in the registry key for both Steam and non-Steam versions of the game.
        /// </summary>
        /// <param name="audioMode">The audio mode to be set. It should be either <see cref="AudioMode.DirectSound"/> or <see cref="AudioMode.XAudio2"/>.</param>
        public static bool SetAudiomode(AudioMode audioMode)
        {
            string[] subKeyPaths = new string[]
            {
                @"SOFTWARE\Atari\TDU2",          // Non-Steam
                @"SOFTWARE\Atari\TDU2\Steam"     // Steam
            };

            try
            {
                foreach (var subKeyPath in subKeyPaths)
                {
                    using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                           .OpenSubKey(subKeyPath, writable: true)
                           ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                         .CreateSubKey(subKeyPath, writable: true))
                    {
                        if (baseKey == null)
                        {
                            Log.Log.Error($"Could not open or create registry key: {subKeyPath}");
                            continue;
                        }

                        // Setze den AudioLib-Wert
                        baseKey.SetValue("AudioLib", audioMode.ToString());
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return false;
        }


        /// <summary>
        /// Updates the installation paths (ExePath, installdir, and LauncherPath) in the registry.
        /// </summary>
        /// <param name="gameExe">The game executable path.</param>
        /// <param name="installdir">The installation directory path.</param>
        /// <param name="launcherPath">The launcher path.</param>
        /// <param name="isSteam">Indicates if the installation is Steam-based.</param>
        public static bool UpdatePath(string gameExe, string installdir, string launcherPath, bool isSteam = false)
        {
            string subKeyPath = isSteam ? @"SOFTWARE\Atari\TDU2\Steam" : @"SOFTWARE\Atari\TDU2";

            try
            {
                // Öffne HKEY_LOCAL_MACHINE im 32-Bit-View
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                       .OpenSubKey(subKeyPath, writable: true)
                       ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                     .CreateSubKey(subKeyPath, writable: true))
                {
                    if (baseKey == null)
                    {
                        Log.Log.Error($"Could not open or create registry key: {subKeyPath}");
                        return false;
                    }

                    baseKey.SetValue("ExePath", gameExe);
                    baseKey.SetValue("installdir", installdir);
                    baseKey.SetValue("LauncherPath", launcherPath);
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return false;
        }


        /// <summary>
        /// Updates or creates a specific registry key with a value.
        /// </summary>
        /// <param name="keyName">The registry key name.</param>
        /// <param name="keyValue">The value to set for the registry key.</param>
        /// <param name="isSteam">Indicates if the installation is Steam-based.</param
        public static bool UpdateKey(string keyName, string keyValue, bool isSteam = false)
        {
            string subKeyPath = isSteam ? @"SOFTWARE\Atari\TDU2\Steam" : @"SOFTWARE\Atari\TDU2";

            try
            {
                // Öffne HKEY_LOCAL_MACHINE im 32-Bit-View (WOW6432Node wird automatisch berücksichtigt)
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                       .OpenSubKey(subKeyPath, writable: true)
                       ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                     .CreateSubKey(subKeyPath, writable: true))
                {
                    if (baseKey == null)
                    {
                        Log.Log.Error($"Could not open or create registry key: {subKeyPath}");
                        return false;
                    }

                    baseKey.SetValue(keyName, keyValue);
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return false;
        }

        /// <summary>
        /// Ensures a registry key exists. If it doesn't, it will be created with a default value.
        /// </summary>
        /// <param name="keyName">The name of the registry key.</param>
        /// <param name="isSteam">Indicates if the installation is Steam-based.</param>
        /// <returns>True if the key exists or was created.</returns>
        public static bool ExistKey(string keyName, bool isSteam = false)
        {
            string subKeyPath = isSteam ? @"SOFTWARE\Atari\TDU2\Steam" : @"SOFTWARE\Atari\TDU2";

            try
            {
                // Öffne HKEY_LOCAL_MACHINE im 32-Bit-View, damit WOW6432Node automatisch berücksichtigt wird
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                                       .OpenSubKey(subKeyPath, writable: true)
                       ?? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                     .CreateSubKey(subKeyPath, writable: true))
                {
                    if (baseKey == null)
                    {
                        Log.Log.Error($"Could not open or create registry key: {subKeyPath}");
                        return false;
                    }

                    // Existiert der KeyName noch nicht, anlegen
                    if (baseKey.GetValue(keyName) == null)
                    {
                        baseKey.SetValue(keyName, string.Empty);
                    }
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Log.Error($"Access denied: {ex.Message}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Log.Log.Error($"Permission error: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                Log.Log.Error($"Invalid argument: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Log.Error($"An error occurred: {ex.Message}", ex);
            }

            return false;
        }
    }
}