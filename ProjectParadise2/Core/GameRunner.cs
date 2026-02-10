using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Views;
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
        private static Process monitoredProcess;
        private static Thread monitoringThread;
        private static bool isMonitoringActive = false;
        public static bool CanRunTheGame { get; set; } = false;

        /// <summary>
        /// Starts the game based on user settings.
        /// </summary>
        /// <returns>Returns true if the game started successfully, otherwise false.</returns>
        public static bool RunGame(GameProfile profile)
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
                if (File.Exists(profile.Gamepath))
                {
                    AntiCheat.StartMonitoring();

                    if (AntiCheat.CanRungame() == false)
                    {
                        Log.Warning("Cheat Tool detected, aborting game start.");
                        MessageBox.Show("Failed run Game: 0x14",
                                        "Project Paradise 2 - Gamestart",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                        return false;
                    }

                    if (profile.Gametype == Gametype.TDU2)
                    {
                        if (!File.Exists(Path.Combine(profile.Basedir, "key.txt")))
                        {
                            string Key = BackgroundWorker.GetKey();
                            Log.Warning("Missing Key dedect Generate it: " + Key + " ");
                            File.WriteAllText(Path.Combine(profile.Basedir, "key.txt"), Key);
                        }
                    }

                    profile.SetLargeAddressAware(profile.Gamepath, profile.LAAEnabled);

                    if (profile.Gametype == Gametype.TDU2 || profile.Gametype == Gametype.TDU2Dev)
                    {
                        if (profile.OnlineMode)
                        {
                            DiscordIntegration.UpdateRpc(DiscordIntegration.OnlineMode);
                        }
                        else
                        {
                            DiscordIntegration.UpdateRpc(DiscordIntegration.OfflineMode);
                        }

                        start = new Mutex(false, profile.GetMutex(), out bool createdNew);

                        if (!createdNew)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        DiscordIntegration.UpdateRpc(DiscordIntegration.TDU);
                    }

                    ProcessStartInfo processStartInfo = new ProcessStartInfo(profile.Gamepath, profile.GetLaunchArguments());
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(profile.Gamepath);
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
                    Log.Info("Game (" + profile.Profilename + ") started, -> " + profile.Gamepath + " args: " + profile.Arguments);
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
                MessageBox.Show("Failed run Game: 0x7F",
                "Project Paradise 2 - Gamestart",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
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

        static bool Decoradet = false;

        /// <summary>
        /// Monitors the started process and responds to the process exit.
        /// </summary>
        private static void MonitorProcess()
        {
            Decoradet = false;
            try
            {
                AntiCheat.SetProcess(monitoredProcess);
                while (isMonitoringActive)
                {
                    if (monitoredProcess != null && monitoredProcess.HasExited)
                    {
                        isMonitoringActive = false;
                        HandleProcessExit();
                    }
                    Thread.Sleep(4000);
                    if (!Decoradet)
                    {
                        AntiCheat.Decorate();
                        Decoradet = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while monitoring the process: {ex.Message}: " + ex);
                MessageBox.Show("Failed run Game: 0x6D",
                    "Project Paradise 2 - Gamestart",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
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
            }
            CanRunTheGame = true;
            try
            {
                HomeView.Instance.ShowRunGameButton();
            }
            catch (Exception ex)
            {
                Log.Error("Failed to show the run game button ", ex);
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
                    MessageBox.Show("Failed run Game: 0x9D",
                        "Project Paradise 2 - Gamestart",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
    }
}