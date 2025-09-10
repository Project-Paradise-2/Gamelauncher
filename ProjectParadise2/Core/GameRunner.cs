using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace ProjectParadise2
{
    internal class GameRunner
    {
        private static Mutex start;
        public static string[] Runtype = { "44938b8f", "957e4cc3" }; // On/Off
        private static Process monitoredProcess;
        private static Thread monitoringThread;
        private static bool isMonitoringActive = false;
        public static bool CanRunTheGame { get; set; } = false;

        /// <summary>
        /// Starts the game based on user settings.
        /// </summary>
        /// <returns>Returns true if the game started successfully, otherwise false.</returns>
        public static bool RunGame()
        {
            if (Database.Database.p2Database.Usersettings.BackupType == Database.Data.BackUptype.OnStart)
            {
                var backup = new Thread(Savebackup.CreateBackup);
                backup.IsBackground = true;
                backup.Start();
            }
            Application.Current.Exit += new ExitEventHandler(KillGameFast);
            DiscordIntegration.SetRpcTime();
            try
            {
                int mode = 1;
                if (string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
                {
                    return false;
                }

                if (Database.Database.p2Database.Usersettings.Onlinemode)
                {
                    mode = 0;
                }

                if (!CommandLineArg.OnlineMode)
                {
                    mode = 1;
                    Log.Warning("Force Offline mode, flag is active");
                }

                if (mode == 0)
                {
                    DiscordIntegration.UpdateRpc(DiscordIntegration.OnlineMode);
                }
                else
                {
                    DiscordIntegration.UpdateRpc(DiscordIntegration.OfflineMode);
                }

                string path = Path.GetFullPath(Database.Database.p2Database.Usersettings.ExePath);
                if (File.Exists(Path.Combine(Database.Database.p2Database.Usersettings.Gamedirectory, "TestDrive2.exe")))
                {
                    SetLargeAddressAware(Path.Combine(Database.Database.p2Database.Usersettings.Gamedirectory, "TestDrive2.exe"), Database.Database.p2Database.Usersettings.LAAEnabled);

                    if (!File.Exists(Path.Combine(Database.Database.p2Database.Usersettings.Gamedirectory, "key.txt")))
                    {
                        string Key = BackgroundWorker.GetKey();
                        Log.Warning("Missing Key dedect Generate it: " + Key + " ");
                        File.WriteAllText(Path.Combine(Database.Database.p2Database.Usersettings.Gamedirectory, "key.txt"), Key);
                    }

                    start = new Mutex(false, Runtype[mode], out bool createdNew);

                    if (!createdNew)
                    {
                        return false;
                    }

                    string arguments = Runtype[mode];
                    var highPrio = Database.Database.p2Database.Usersettings.HighPrio;
                    var moreCores = Database.Database.p2Database.Usersettings.UseMoreCores;

                    // Falls HighPrio aktiviert ist, füge "-high" hinzu
                    if (highPrio)
                    {
                        arguments += " -high";
                    }

                    // Falls MoreCores aktiviert ist, füge "-USEALLAVAILABLECORES" hinzu
                    if (moreCores)
                    {
                        arguments += " -USEALLAVAILABLECORES";
                    }

                    ProcessStartInfo processStartInfo = new ProcessStartInfo(path, Runtype[mode] + arguments);
                    var proc = new Process();
                    proc.StartInfo = processStartInfo;
                    isMonitoringActive = true;
                    proc.Start();
                    Thread.Sleep(500);
                    monitoredProcess = proc;
                    monitoringThread = new Thread(MonitorProcess);
                    monitoringThread.Start();
                    MinimizeApplicationWithHint();
                    StopMutex();
                    Log.Info("Game started with mode: " + (mode == 0 ? "Online" : "Offline") + " args: " + arguments);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while starting the game: {ex.Message}: " + ex);
                return false;
            }
        }

        /// <summary>
        /// Called when the application closes and executes the KillGame method.
        /// </summary>
        private static void KillGameFast(object sender, ExitEventArgs e)
        {
            KillGame();
        }

        /// <summary>
        /// Monitors the started process and responds to the process exit.
        /// </summary>
        private static void MonitorProcess()
        {
            try
            {
                while (isMonitoringActive)
                {
                    if (monitoredProcess != null && monitoredProcess.HasExited)
                    {
                        isMonitoringActive = false;
                        HandleProcessExit();
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while monitoring the process: {ex.Message}: " + ex);
            }
        }

        /// <summary>
        /// Handles the exit of the monitored process and restores the application.
        /// </summary>
        private static void HandleProcessExit()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RestoreApplication();
            });
        }

        /// <summary>
        /// Stops the mutex after the monitored process has exited.
        /// </summary>
        private static void StopMutex()
        {
            Thread.Sleep(5000);
            while (monitoredProcess != null && !monitoredProcess.HasExited && monitoredProcess.StartTime != DateTime.MinValue)
            {
                Thread.Sleep(1000);
            }
            if (start != null)
            {
                start.Close();
                CanRunTheGame = true;
            }
        }

        /// <summary>
        /// Kills the game and closes the mutex.
        /// </summary>
        public static void KillGame()
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow.Close();
                });
            }
            if (monitoredProcess != null)
            {
                try
                {
                    if (!monitoredProcess.HasExited)
                    {
                        monitoredProcess.Kill();
                        monitoredProcess.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"An error occurred while killing the game process: {ex.Message}: " + ex);
                }
            }
            if (start != null)
            {
                start.Close();
                CanRunTheGame = true;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                App.Current.Shutdown();
            });
        }

        /// <summary>
        /// Minimizes the application if the 'HideOnStart' argument is set.
        /// </summary>
        private static void MinimizeApplicationWithHint()
        {
            if (CommandLineArg.HideOnStart)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.WindowState = WindowState.Minimized;
                        mainWindow.ShowInTaskbar = false;
                    }
                });
            }
            else
                Log.Warning("Stop hiding launcher, flag is active");
        }

        /// <summary>
        /// Restores the application if it was minimized.
        /// </summary>
        private static void RestoreApplication()
        {
            DiscordIntegration.UpdateRpc(DiscordIntegration.Closed);

            if (Database.Database.p2Database.Usersettings.BackupType == Database.Data.BackUptype.OnClose)
            {
                var backup = new Thread(Savebackup.CreateBackup);
                backup.IsBackground = true;
                backup.Start();
            }

            if (CommandLineArg.HideOnStart)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = Application.Current.MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.WindowState = WindowState.Normal;
                        mainWindow.ShowInTaskbar = true;
                        mainWindow.Activate();
                    }
                });
            }
        }

        /// <summary>
        /// Sets or unsets the Large Address Aware (LAA) flag in the specified executable file.
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public static bool SetLargeAddressAware(string exePath, bool enable)
        {
            if (!File.Exists(exePath))
            {
                Log.Error("Error: File not found!");
                return false;
            }

            try
            {
                using (FileStream fs = new FileStream(exePath, FileMode.Open, FileAccess.ReadWrite))
                using (BinaryReader br = new BinaryReader(fs))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    fs.Seek(0x3C, SeekOrigin.Begin);
                    int peHeaderOffset = br.ReadInt32();

                    fs.Seek(peHeaderOffset + 0x18, SeekOrigin.Begin);
                    short magic = br.ReadInt16();

                    if (magic != 0x10B && magic != 0x20B)  // Check if it's a valid PE file (32-bit or 64-bit)
                    {
                        Log.Error("Error: Not a valid PE file.");
                        return false;
                    }

                    fs.Seek(peHeaderOffset + 0x16, SeekOrigin.Begin);
                    ushort characteristics = br.ReadUInt16();

                    if (enable)
                    {
                        characteristics |= 0x0020;  // Enable LAA flag
                    }
                    else
                    {
                        characteristics &= unchecked((ushort)~0x0020);  // Disable LAA flag
                    }

                    fs.Seek(peHeaderOffset + 0x16, SeekOrigin.Begin);
                    bw.Write(characteristics);
                }

                Log.Info("LAA flag has been successfully " + (enable ? "enabled" : "disabled") + "!");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error modifying EXE: " + ex.Message);
                return false;
            }
        }
    }
}