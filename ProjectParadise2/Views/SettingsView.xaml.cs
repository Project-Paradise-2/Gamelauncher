using Microsoft.Win32;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Classes;
using ProjectParadise2.Core.Log;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// This class provides functionality to interact with the settings view, allowing the user to configure game paths, updates, and connection settings.
    /// </summary>
    public partial class SettingsView : UserControl
    {
        /// <summary>
        /// Singleton instance of SettingsView.
        /// </summary>
        public static SettingsView Instance { get; private set; }

        public SettingsView()
        {
            InitializeComponent();
            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            Instance = this;
            InitializeSettingsRefreshThread();
        }

        /// <summary>
        /// Initializes and starts the refresh thread to update UI elements based on the current settings.
        /// </summary>
        private void InitializeSettingsRefreshThread()
        {
            Thread refreshThread = new Thread(Refresh);
            refreshThread.Start();
        }

        /// <summary>
        /// Refreshes the settings UI by updating toggle states and game information.
        /// This method ensures that the UI is updated on the main thread.
        /// </summary>
        public void Refresh()
        {
            Thread.Sleep(450); // Short delay to ensure UI elements are ready for update

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(Refresh)); // Ensures the method runs on the UI thread
                return;
            }

            // Update toggles based on the current user settings from the database
            this.OnlineToggle.IsChecked = Database.Database.p2Database.Usersettings.Onlinemode;
            this.DiscordToggle.IsChecked = Database.Database.p2Database.Usersettings.DiscordRPC;
            this.UpdateToggle.IsChecked = Database.Database.p2Database.Usersettings.Autoupdatecheck;
            this.UPNPToggle.IsChecked = Database.Database.p2Database.Usersettings.UpnpWorker;
            //this.SteamToggle.IsChecked = Database.Database.p2Database.Usersettings.IsSteambuild;
            this.LauncherLang.SelectedIndex = Database.Database.p2Database.Usersettings.LangId;
            this.LauncherLang.SelectedItem = Database.Database.p2Database.Usersettings.LangId;
            this.MagicRam.IsChecked = Database.Database.p2Database.Usersettings.LAAEnabled;
            this.Cores.IsChecked = Database.Database.p2Database.Usersettings.UseMoreCores;
            this.Prio.IsChecked = Database.Database.p2Database.Usersettings.HighPrio;
            var state = Regestry.GetAudiosetting();
            DamageEnabled.IsChecked = Database.Database.p2Database.Usersettings.VehicleDamage;
            DirtEnabled.IsChecked = Database.Database.p2Database.Usersettings.VehicleDirt;

            if (state.Equals(AudioMode.DirectSound))
            {
                this.Audiomode.SelectedIndex = 0;
                this.Audiomode.SelectedItem = 0;
            }
            else
            {
                this.Audiomode.SelectedIndex = 1;
                this.Audiomode.SelectedItem = 1;
            }

            // Check if the game path is set and update game information accordingly
            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
            {
                UpdateGameInfo();
            }
            else
            {
                DisplayNoGameConfiguredWarning();
            }
        }

        /// <summary>
        /// Updates the game information in the UI, including version and file details.
        /// </summary>
        private void UpdateGameInfo()
        {
            if (string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
            {
                DisplayNoGameConfiguredWarning();
                return;
            }

            try
            {
                FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(Database.Database.p2Database.Usersettings.ExePath);
                var Product = fileInfo.ProductName.ToString();
                var Version = fileInfo.ProductVersion.ToString();
                var InternalName = fileInfo.InternalName.ToString();
                var FileVersion = fileInfo.FileVersion.ToString();

                GameInfo.Text = Lang.GetText(97);
                GameInfo1.Text = Lang.GetText(98) + Database.Database.p2Database.Usersettings.Gamedirectory;
                GameInfo2.Text = Lang.GetText(99) + InternalName + " " + FileVersion.Replace("TDU2", "");
                GameInfo2.Foreground = Brushes.White;

                // Check if the game version matches the expected one
                if (Version.Equals("TDU2 DLC2 v034"))
                {
                    GameInfo3.Foreground = Brushes.Green;
                }
                else
                {
                    GameInfo3.Foreground = Brushes.Red;
                }
                GameInfo3.Text = Lang.GetText(100) + Product + " " + Version.Replace("TDU2", "");
            }
            catch (Exception ex)
            {
                Log.Print("Failed Read Gameinfos: " + ex.Message);
                DisplayNoGameConfiguredWarning();
                return;
            }
        }

        /// <summary>
        /// Displays a warning if no game is selected or configured.
        /// </summary>
        private void DisplayNoGameConfiguredWarning()
        {
            GameInfo2.Foreground = Brushes.Red;
            GameInfo.Text = Lang.GetText(101);
            GameInfo2.Text = Lang.GetText(102);
        }

        /// <summary>
        /// Allows the user to select a game executable file and validates the selection.
        /// </summary>
        private void OnGameSelect(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Title = Lang.GetText(103),
                InitialDirectory = Regestry.GetPath(),
                DefaultExt = ".exe",
                Filter = "TestDrive2.exe|TestDrive2.exe"
            };

            if (open.ShowDialog() == true)
            {
                FileVersionInfo file = FileVersionInfo.GetVersionInfo(open.FileName);
                var Product = file.ProductName.ToString();
                var Version = file.ProductVersion.ToString();
                var FileVersion = file.FileVersion.ToString();

                if (!Product.Contains("Test Drive Unlimited 2") || !FileVersion.Contains("TDU2"))
                {
                    // Show message and prompt to select correct file
                    MessageBoxResult state = MessageBox.Show(Lang.GetText(104), "Project Paradise 2 - Test Drive Unlimited 2 Game Selector", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (state == MessageBoxResult.OK)
                    {
                        OnGameSelect(sender, e); // Retry if the selected file is incorrect
                    }
                }

                CheckGame(open.FileName); // Proceed to check and update game settings
            }
        }

        /// <summary>
        /// Validates the selected game file and updates the settings based on whether it's a Steam installation.
        /// </summary>
        private void CheckGame(string fileName)
        {
            string serial = "xxxx-xxxx-xxxx-xxxx";
            FileVersionInfo file = FileVersionInfo.GetVersionInfo(fileName);
            var basedir = fileName.Replace(@"\TestDrive2.exe", "");
            bool isSteam = File.Exists(basedir + @"\steam_api.dll");

            bool unpackedDetected =
                File.Exists(Path.Combine(basedir, "Euro", "Bnk", "database", "db_data.cpr")) ||
                Directory.Exists(Path.Combine(basedir, "Unknown_bin")) ||
                File.Exists(Path.Combine(basedir, "TDU2_Unpacked_Uninstall.exe"));

            if (unpackedDetected)
            {
                MessageBoxResult state = MessageBox.Show(
                    Lang.GetText(123),
                    "Project Paradise 2 - Test Drive Unlimited 2 Game Selector",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (state == MessageBoxResult.Yes)
                {
                    Database.Database.p2Database.Usersettings.Packedgame = false;

                    MessageBoxResult warning = MessageBox.Show(
                            "Warning: Use of unpacked games is at your own risk. We do not provide any support/help for issues.",
                            "Project Paradise 2 - Test Drive Unlimited 2 Game Selector",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                }
                else
                {
                    Database.Database.p2Database.Usersettings.Packedgame = true;

                    MessageBoxResult warning = MessageBox.Show(
                            "Warning: If this information is incorrect, it may damage your game and could require a reinstallation.",
                            "Project Paradise 2 - Test Drive Unlimited 2 Game Selector",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                }
            }

            if (isSteam)
            {
                string message = Lang.GetText(105);
                var result = MessageBox.Show(message, "Project Paradise 2 - Test Drive Unlimited 2 Game Selector", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    isSteam = false;
                    File.Move(basedir + @"\steam_api.dll", basedir + @"\steam_api.dll.backup");
                }
            }

            while (Regestry.UpdateKey(isSteam)) break;
            Regestry.UpdatePath(basedir + @"\TestDrive2.exe", basedir, System.Environment.CurrentDirectory + @"\Uplauncher.exe", isSteam);

            if (File.Exists(basedir + @"\key.txt"))
                serial = File.ReadAllText(basedir + @"\key.txt");

            var currentLanguage = CultureInfo.CurrentCulture;
            var parsed = currentLanguage.Name.Split("-");
            Regestry.UpdateKey("AudioLib", "DirectSound", isSteam);
            Regestry.UpdateKey("GameProductVersion", file.ProductVersion, isSteam);
            Regestry.UpdateKey("GUID", Guid.NewGuid().ToString(), isSteam);
            Regestry.UpdateKey("Serial", serial, isSteam);
            Regestry.UpdateKey("NetworkNatType", "Strict:Blocked", isSteam);
            Regestry.UpdateKey("languagePack", Utils.GetRegionFromCountryCode(new RegionInfo(currentLanguage.Name).TwoLetterISORegionName), isSteam);
            Regestry.UpdateKey("language", parsed[0], isSteam);
            Regestry.UpdateKey("Project-Paradise2", Constans.LauncherVersion, isSteam);

            Database.Database.p2Database.Usersettings.ExePath = fileName.ToString();
            Database.Database.p2Database.Usersettings.Gamedirectory = basedir.ToString();
            Database.Database.p2Database.Usersettings.FileVersion = file.FileVersion.ToString();
            Database.Database.p2Database.Usersettings.ProductVersion = file.ProductVersion.ToString();
            Database.Database.p2Database.Usersettings.IsSteambuild = isSteam;

            if (isSteam)
                InstallWrapper(basedir);

            Refresh(); // Refresh UI with the updated settings
            Database.Database.Write();

            if (file.ProductVersion != "TDU2 DLC2 v034")
            {
                MessageBox.Show(Lang.GetText(106), "Project Paradise 2 - Test Drive Unlimited 2 Game Selector", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            Regestry.UpdateKey("NetworkNatType", BackgroundWorker.MyNatType, Database.Database.p2Database.Usersettings.IsSteambuild);
        }

        /// <summary>
        /// Placeholder method for Steam wrapper installation (future functionality).
        /// </summary>
        private void InstallWrapper(string basedir)
        {
            // Install Steam wrapper or additional files if necessary.
        }

        /// <summary>
        /// Sets the server connection to the custom connection.
        /// If no game directory has been selected, displays an error message indicating
        /// that the server connection cannot be changed.
        /// </summary>
        private void OnConnectionSet(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Database.Database.p2Database.Usersettings.Gamedirectory))
            {
                if (File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/GamePC.cpr"))
                    File.Delete(Database.Database.p2Database.Usersettings.Gamedirectory + "/GamePC.cpr");

                File.WriteAllBytes(Database.Database.p2Database.Usersettings.Gamedirectory + "/GamePC.cpr", Properties.Resources.GamePC);
                MessageBox.Show("The connection file has been replaced.", "Project Paradise 2 - Server Connection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(Lang.GetText(109), "Project Paradise 2 - Server Connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks and performs an update for the selected game.
        /// If no game directory has been selected, displays an error message indicating
        /// that the update cannot be performed.
        /// </summary>
        private void OnGameUpdate(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Database.Database.p2Database.Usersettings.Gamedirectory))
            {
                if (Database.Database.p2Database.Usersettings.Packedgame == false)
                {
                    MessageBox.Show(Lang.GetText(124), "Project Paradise 2 - Test Drive Unlimited 2 Game Selector", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                    MainViewModel.GetUpdateView();
            }
            else
            {
                MessageBox.Show(Lang.GetText(110), "Project Paradise 2 - Game Update", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Helper method to open a directory in Windows Explorer.
        /// Displays an error message if the directory does not exist.
        /// </summary>
        /// <param name="folderPath">The folder path to open.</param>
        /// <param name="errorMessage">The error message to display if the folder does not exist.</param>
        private void OpenDirectory(string folderPath, string errorMessage)
        {
            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    Arguments = folderPath.Replace("/", @"\"),
                    FileName = "explorer.exe"
                });
            }
            else
            {
                MessageBox.Show(errorMessage, "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles changes in the audio mode selection and updates the registry with the selected audio mode.
        /// </summary>
        /// <param name="sender">The source of the event, typically a ComboBox.</param>
        /// <param name="e">Event arguments containing details about the selection change.</param>
        private void UpdateAudio(object sender, SelectionChangedEventArgs e)
        {
            var selected = (AudioMode)Audiomode.SelectedIndex;
            Regestry.SetAudiomode(selected);
        }

        /// <summary>
        /// Updates the online mode setting based on the toggle switch's state
        /// and saves the change to the database.
        /// </summary>
        /// <param name="sender">The source of the event, typically a ToggleButton.</param>
        /// <param name="e">Event arguments for the routed event.</param>
        private void OnOnlineModeupdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.Onlinemode = (bool)OnlineToggle.IsChecked;
            Save();
            BackgroundWorker.SetLang();
        }

        /// <summary>
        /// Updates the Discord RPC setting based on the toggle switch's state
        /// and saves the change to the database.
        /// </summary>
        /// <param name="sender">The source of the event, typically a ToggleButton.</param>
        /// <param name="e">Event arguments for the routed event.</param>
        private void OnDiscordUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.DiscordRPC = (bool)DiscordToggle.IsChecked;
            Save();
        }

        /// <summary>
        /// Updates the automatic update check setting based on the toggle switch's state
        /// and saves the change to the database.
        /// </summary>
        /// <param name="sender">The source of the event, typically a ToggleButton.</param>
        /// <param name="e">Event arguments for the routed event.</param>
        private void OnUpdateupdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.Autoupdatecheck = (bool)UpdateToggle.IsChecked;
            Save();
        }

        /// <summary>
        /// Updates the UPnP worker setting based on the toggle switch's state
        /// and saves the change to the database.
        /// </summary>
        /// <param name="sender">The source of the event, typically a ToggleButton.</param>
        /// <param name="e">Event arguments for the routed event.</param>
        private void OnUpnpupdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.UpnpWorker = (bool)UPNPToggle.IsChecked;
            Save();
        }

        ///// <summary>
        ///// Updates the Steam build setting based on the toggle switch's state
        ///// and saves the change to the database.
        ///// </summary>
        ///// <param name="sender">The source of the event, typically a ToggleButton.</param>
        ///// <param name="e">Event arguments for the routed event.</param>
        //private void OnSteamupdate(object sender, RoutedEventArgs e)
        //{
        //    Database.Database.p2Database.Usersettings.IsSteambuild = (bool)SteamToggle.IsChecked;
        //    Save();

        //}

        /// <summary>
        /// Saves the current state of the user's settings to the database and refreshes the UI.
        /// </summary>
        private void Save()
        {
            Database.Database.Write();
            Refresh(); // Refresh UI with the updated settings
        }

        private void SetLang(object sender, SelectionChangedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.LangId = this.LauncherLang.SelectedIndex;
            Save();
            BackgroundWorker.SetLang();
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            GameSelect.Content = Lang.GetText(31);
            GameUpdate.Content = Lang.GetText(34);
            SetConnection.Content = Lang.GetText(32);
            OnlineTxt.Text = Lang.GetText(114);
            DiscordTxt.Text = Lang.GetText(116);
            UpdateTxt.Text = Lang.GetText(117);
            //SteamTxt.Text = Lang.GetText(119);
            WarningTxt.Text = " *(" + Lang.GetText(120) + ")";
            LangTxt.Text = Lang.GetText(121);
            AudioTxt.Text = Lang.GetText(122);
            OnlineToggle.ToolTip = Lang.GetTooltipText(11);
            DiscordToggle.ToolTip = Lang.GetTooltipText(13);
            UpdateToggle.ToolTip = Lang.GetTooltipText(14);
            UPNPToggle.ToolTip = Lang.GetTooltipText(15);
            //SteamToggle.ToolTip = Lang.GetTooltipText(16);
            GameSelect.ToolTip = Lang.GetTooltipText(17);
            GameUpdate.ToolTip = Lang.GetTooltipText(18);
            SetConnection.ToolTip = Lang.GetTooltipText(19);
            Audiomode.ToolTip = Lang.GetTooltipText(20);
            MagicRam.ToolTip = Lang.GetTooltipText(21);
            Cores.ToolTip = Lang.GetTooltipText(22);
            Prio.ToolTip = Lang.GetTooltipText(23);
            DamageEnabled.ToolTip = Lang.GetTooltipText(26);
            DirtEnabled.ToolTip = Lang.GetTooltipText(27);

            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
            {
                UpdateGameInfo();
            }
            else
            {
                DisplayNoGameConfiguredWarning();
            }
        }

        private void SetLang()
        {
            GameSelect.Content = Lang.GetText(31);
            GameUpdate.Content = Lang.GetText(34);
            SetConnection.Content = Lang.GetText(32);
            OnlineTxt.Text = Lang.GetText(114);
            DiscordTxt.Text = Lang.GetText(116);
            UpdateTxt.Text = Lang.GetText(117);
            //SteamTxt.Text = Lang.GetText(119);
            WarningTxt.Text = " *(" + Lang.GetText(120) + ")";
            LangTxt.Text = Lang.GetText(121);
            AudioTxt.Text = Lang.GetText(122);
            OnlineToggle.ToolTip = Lang.GetTooltipText(11);
            DiscordToggle.ToolTip = Lang.GetTooltipText(13);
            UpdateToggle.ToolTip = Lang.GetTooltipText(14);
            UPNPToggle.ToolTip = Lang.GetTooltipText(15);
            //SteamToggle.ToolTip = Lang.GetTooltipText(16);
            GameSelect.ToolTip = Lang.GetTooltipText(17);
            GameUpdate.ToolTip = Lang.GetTooltipText(18);
            SetConnection.ToolTip = Lang.GetTooltipText(19);
            Audiomode.ToolTip = Lang.GetTooltipText(20);
            MagicRam.ToolTip = Lang.GetTooltipText(21);
            Cores.ToolTip = Lang.GetTooltipText(22);
            Prio.ToolTip = Lang.GetTooltipText(23);

            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
            {
                UpdateGameInfo();
            }
            else
            {
                DisplayNoGameConfiguredWarning();
            }
        }

        private void OnRamUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.LAAEnabled = (bool)MagicRam.IsChecked;
            Save();
        }

        private void OnCoreUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.UseMoreCores = (bool)Cores.IsChecked;
            Save();
        }

        private void OnPrioUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.HighPrio = (bool)Prio.IsChecked;
            Save();
        }

        private void OnDamageUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.VehicleDamage = (bool)DamageEnabled.IsChecked;
            EditExe(0x00AE0484L, (bool)DamageEnabled.IsChecked);
            Save();
        }

        private void OnDirtUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.VehicleDirt = (bool)DirtEnabled.IsChecked;
            EditExe(0x00AE0494L, (bool)DirtEnabled.IsChecked);
            Save();
        }

        private static bool EditExe(long offset, bool Value)
        {
            byte data = 0;

            if (Value)
            {
                data = 0x44;
            }
            else
            {
                data = 0x42;
            }

            try
            {
                string filePath = Path.Combine(Database.Database.p2Database.Usersettings.Gamedirectory, "TestDrive2.exe");

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                    fs.WriteByte(data);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler: " + ex.Message);
                return false;
            }
        }
    }
}