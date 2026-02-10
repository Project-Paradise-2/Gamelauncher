using Microsoft.Win32;
using Newtonsoft.Json;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaktionslogik für CreateProfileView.xaml
    /// </summary>
    public partial class CreateProfileView : UserControl
    {
        private string GamePath;
        private string GameProfilename;
        private string StartArgs;

        private GameProfile currentProfile;

        public CreateProfileView()
        {
            InitializeComponent();
            BackgroundWorker.OnLangset += SetLang;
            ModBrowser.Visibility = Visibility.Hidden;
            SetConnection.Visibility = Visibility.Hidden;
            DamageEnabled.Visibility = Visibility.Hidden;
            DirtEnabled.Visibility = Visibility.Hidden;
            DirtText.Visibility = Visibility.Hidden;
            DamageText.Visibility = Visibility.Hidden;
            GameUpdate.Visibility = Visibility.Hidden;
            SetConnection.Visibility = Visibility.Hidden;
            ArgumentText.Visibility = Visibility.Hidden;
            Arguments.Visibility = Visibility.Hidden;
            MagicRam.Visibility = Visibility.Hidden;
            Cores.Visibility = Visibility.Hidden;
            Priority.Visibility = Visibility.Hidden;
            OnlineEnabled.Visibility = Visibility.Hidden;
            MagicRamText.Visibility = Visibility.Hidden;
            OnlineText.Visibility = Visibility.Hidden;
            MagicCoresText.Visibility = Visibility.Hidden;
            MagicPrioText.Visibility = Visibility.Hidden;
            InitializeSettingsRefreshThread();
        }

        private void InitializeSettingsRefreshThread()
        {
            Thread refreshThread = new Thread(Refresh);
            refreshThread.Start();
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            ProfileName.Text = Lang.GetText(21);
            ArgumentText.Text = Lang.GetText(22);
            MagicRamText.Text = Lang.GetText(23);
            MagicCoresText.Text = Lang.GetText(24);
            MagicPrioText.Text = Lang.GetText(25);
            DirtText.Text = Lang.GetText(26);
            DamageText.Text = Lang.GetText(27);
            OnlineText.Text = Lang.GetText(28);
            MagicRam.ToolTip = Lang.GetTooltipText(6);
            Cores.ToolTip = Lang.GetTooltipText(7);
            Priority.ToolTip = Lang.GetTooltipText(8);
            DirtEnabled.ToolTip = Lang.GetTooltipText(9);
            DamageEnabled.ToolTip = Lang.GetTooltipText(10);
            OnlineEnabled.ToolTip = Lang.GetTooltipText(11);
            GameSelect.Content = Lang.GetText(29);
            OpenGame.Content = Lang.GetText(30);
            GameUpdate.Content = Lang.GetText(33);
            SetConnection.Content = Lang.GetText(31);
            ModBrowser.Content = Lang.GetText(32);
            Save.Content = Lang.GetText(34);
            GameSelect.ToolTip = Lang.GetTooltipText(12);
            OpenGame.ToolTip = Lang.GetTooltipText(13);
            GameUpdate.ToolTip = Lang.GetTooltipText(14);
            SetConnection.ToolTip = Lang.GetTooltipText(15);
            ModBrowser.ToolTip = Lang.GetTooltipText(16);
            Save.ToolTip = Lang.GetTooltipText(17);
        }

        public void Refresh()
        {
            Thread.Sleep(100); // Short delay to ensure UI elements are ready for update
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(Refresh)); // Ensures the method runs on the UI thread
                return;
            }

            if (currentProfile == null)
            {
                return;
            }

            try
            {
                ShowElements(currentProfile.Gametype, currentProfile.Build);
                CreateEdit.Text = Lang.GetText(20) + " (" + currentProfile.Profilename + ")";
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(currentProfile.Gamepath);
                ProductVersion.Text = Lang.GetText(35) + fvi.ProductVersion.ToString();
                FileVersion.Text = string.Format(Lang.GetText(36), fvi.FileVersion.ToString());
                Gamedir.Text = string.Format(Lang.GetText(37), System.IO.Path.GetFullPath(GamePath).Replace(@"\TestDrive2.exe", "").Replace(@"\TestDriveUnlimited.exe", ""));
                GameSize.Text = string.Format(Lang.GetText(38), FormatFileSize(GetDirectorySize(System.IO.Path.GetDirectoryName(GamePath), true)));
                if (GamePath.EndsWith("TestDriveUnlimited.exe"))
                {
                    GameType.Text = string.Format(Lang.GetText(39), "TDU1", "Packed", "Standalone");
                    ProductVersion.Inlines.Clear();
                    ProductVersion.Inlines.Add(new Run(Lang.GetText(35)) { Foreground = Brushes.White });
                    Brush versionColor = fvi.ProductVersion.ToString().Equals("MC 1.66 A")
                        ? Brushes.Green
                        : Brushes.Red;
                    ProductVersion.Inlines.Add(new Run(fvi.ProductVersion.ToString()) { Foreground = versionColor });
                    if (!fvi.ProductVersion.ToString().Equals("MC 1.66 A"))
                    {
                        ProductVersion.Inlines.Add(new Run(Lang.GetText(40))
                        {
                            Foreground = Brushes.White
                        });
                    }
                }
                else if (GamePath.EndsWith("TestDrive2.exe"))
                {
                    Database.Database.p2Database.Usersettings.Gamedirectory = System.IO.Path.GetFullPath(GamePath).Replace(@"\TestDrive2.exe", "");
                    Database.Database.p2Database.Usersettings.Packedgame = currentProfile.Build == Build.Packed;
                    Database.Database.Write();
                    if (currentProfile.SteamBuild)
                    {
                        GameType.Text = string.Format(Lang.GetText(39), "TDU2", currentProfile.Build, "Steamspiel");
                    }
                    else
                    {
                        GameType.Text = string.Format(Lang.GetText(39), "TDU2", currentProfile.Build, "Standalone");
                    }

                    ProductVersion.Inlines.Clear();
                    ProductVersion.Inlines.Add(new Run(Lang.GetText(35)) { Foreground = Brushes.White });
                    Brush versionColor = fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034")
                        ? Brushes.Green
                        : Brushes.Red;

                    ProductVersion.Inlines.Add(new Run(fvi.ProductVersion.ToString()) { Foreground = versionColor });
                    if (!fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034"))
                    {
                        ProductVersion.Inlines.Add(new Run(Lang.GetText(40))
                        {
                            Foreground = Brushes.White
                        });
                    }
                    this.DirtEnabled.IsChecked = currentProfile.VehicleDirt;
                    this.DamageEnabled.IsChecked = currentProfile.VehicleDamage;
                    this.OnlineEnabled.IsChecked = currentProfile.OnlineMode;
                }
                else if (GamePath.EndsWith("TestDrive2Dev.exe"))
                {
                    Database.Database.p2Database.Usersettings.Gamedirectory = System.IO.Path.GetFullPath(GamePath).Replace(@"\TestDrive2.exe", "");
                    Database.Database.p2Database.Usersettings.Packedgame = currentProfile.Build == Build.Packed;
                    Database.Database.Write();
                    if (currentProfile.SteamBuild)
                    {
                        GameType.Text = string.Format(Lang.GetText(39), "TDU2", currentProfile.Build, "Steamspiel");
                    }
                    else
                    {
                        GameType.Text = string.Format(Lang.GetText(39), "TDU2", currentProfile.Build, "Standalone");
                    }
                    ProductVersion.Inlines.Clear();
                    ProductVersion.Inlines.Add(new Run(Lang.GetText(35)) { Foreground = Brushes.White });
                    Brush versionColor = fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034")
                        ? Brushes.Green
                        : Brushes.Red;

                    ProductVersion.Inlines.Add(new Run(fvi.ProductVersion.ToString()) { Foreground = versionColor });
                    if (!fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034"))
                    {
                        ProductVersion.Inlines.Add(new Run(Lang.GetText(40))
                        {
                            Foreground = Brushes.White
                        });
                    }
                    this.DirtEnabled.IsChecked = currentProfile.VehicleDirt;
                    this.DamageEnabled.IsChecked = currentProfile.VehicleDamage;
                    this.OnlineEnabled.IsChecked = currentProfile.OnlineMode;
                }
                this.MagicRam.IsChecked = currentProfile.LAAEnabled;
                this.Cores.IsChecked = currentProfile.UseMoreCores;
                this.Priority.IsChecked = currentProfile.HighPrio;
                this.Arguments.Text = currentProfile.Arguments;
                this.Profilename.Text = currentProfile.Profilename;
            }
            catch (Exception ex)
            {
                OpenGame.Visibility = Visibility.Hidden;
                DirtEnabled.Visibility = Visibility.Hidden;
                DamageEnabled.Visibility = Visibility.Hidden;
                DirtText.Visibility = Visibility.Hidden;
                DamageText.Visibility = Visibility.Hidden;
                GameUpdate.Visibility = Visibility.Hidden;
                OnlineEnabled.Visibility = Visibility.Hidden;
                OnlineText.Visibility = Visibility.Hidden;
                SetConnection.Visibility = Visibility.Hidden;
                GameUpdate.Visibility = Visibility.Hidden;
                ModBrowser.Visibility = Visibility.Hidden;
                GameUpdate.Visibility = Visibility.Hidden;
                ModBrowser.Visibility = Visibility.Hidden;
                ArgumentText.Visibility = Visibility.Hidden;
                Arguments.Visibility = Visibility.Hidden;
                MagicRamText.Visibility = Visibility.Hidden;
                MagicRam.Visibility = Visibility.Hidden;
                MagicCoresText.Visibility = Visibility.Hidden;
                Cores.Visibility = Visibility.Hidden;
                MagicPrioText.Visibility = Visibility.Hidden;
                Priority.Visibility = Visibility.Hidden;
                Gamedir.Foreground = Brushes.Red;
                Gamedir.Text = "No Valid Gamepath selected";
                Log.Error("Error refreshing profile view", ex);
            }
        }

        public void ShowElements(Gametype gametype, Build build)
        {
            ArgumentText.Text = Lang.GetText(22);
            CreateEdit.Text = Lang.GetText(19);
            ProfileName.Text = Lang.GetText(21);
            ArgumentText.Text = Lang.GetText(22);
            MagicRamText.Text = Lang.GetText(23);
            MagicCoresText.Text = Lang.GetText(24);
            MagicPrioText.Text = Lang.GetText(25);
            DirtText.Text = Lang.GetText(26);
            DamageText.Text = Lang.GetText(27);
            OnlineText.Text = Lang.GetText(28);
            MagicRam.ToolTip = Lang.GetTooltipText(6);
            Cores.ToolTip = Lang.GetTooltipText(7);
            Priority.ToolTip = Lang.GetTooltipText(8);
            DirtEnabled.ToolTip = Lang.GetTooltipText(9);
            DamageEnabled.ToolTip = Lang.GetTooltipText(10);
            OnlineEnabled.ToolTip = Lang.GetTooltipText(11);
            GameSelect.Content = Lang.GetText(29);
            OpenGame.Content = Lang.GetText(30);
            GameUpdate.Content = Lang.GetText(33);
            SetConnection.Content = Lang.GetText(31);
            ModBrowser.Content = Lang.GetText(32);
            Save.Content = Lang.GetText(34);
            GameSelect.ToolTip = Lang.GetTooltipText(12);
            OpenGame.ToolTip = Lang.GetTooltipText(13);
            GameUpdate.ToolTip = Lang.GetTooltipText(14);
            SetConnection.ToolTip = Lang.GetTooltipText(15);
            ModBrowser.ToolTip = Lang.GetTooltipText(16);
            Save.ToolTip = Lang.GetTooltipText(17);

            ProfileName.Visibility = Visibility.Hidden;
            Profilename.Visibility = Visibility.Hidden;
            GameUpdate.Visibility = Visibility.Hidden;
            ModBrowser.Visibility = Visibility.Hidden;
            ArgumentText.Visibility = Visibility.Visible;
            Arguments.Visibility = Visibility.Visible;
            MagicRamText.Visibility = Visibility.Visible;
            MagicRam.Visibility = Visibility.Visible;
            MagicCoresText.Visibility = Visibility.Visible;
            Cores.Visibility = Visibility.Visible;
            MagicPrioText.Visibility = Visibility.Visible;
            Priority.Visibility = Visibility.Visible;

            switch (gametype)
            {
                case Gametype.TDU1:
                    DirtEnabled.Visibility = Visibility.Hidden;
                    DamageEnabled.Visibility = Visibility.Hidden;
                    DirtText.Visibility = Visibility.Hidden;
                    DamageText.Visibility = Visibility.Hidden;
                    GameUpdate.Visibility = Visibility.Hidden;
                    OnlineEnabled.Visibility = Visibility.Hidden;
                    OnlineText.Visibility = Visibility.Hidden;
                    SetConnection.Visibility = Visibility.Hidden;
                    GameUpdate.Visibility = Visibility.Hidden;
                    ModBrowser.Visibility = Visibility.Hidden;
                    break;
                case Gametype.TDU2:
                    DirtEnabled.Visibility = Visibility.Visible;
                    DamageEnabled.Visibility = Visibility.Visible;
                    DirtText.Visibility = Visibility.Visible;
                    DamageText.Visibility = Visibility.Visible;
                    GameUpdate.Visibility = Visibility.Visible;
                    OnlineEnabled.Visibility = Visibility.Visible;
                    OnlineText.Visibility = Visibility.Visible;
                    SetConnection.Visibility = Visibility.Visible;
                    if (build == Build.Packed)
                    {
                        GameUpdate.Visibility = Visibility.Visible;
                        ModBrowser.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        GameUpdate.Visibility = Visibility.Hidden;
                        ModBrowser.Visibility = Visibility.Hidden;
                    }
                    break;
                case Gametype.TDU2Dev:
                    DirtEnabled.Visibility = Visibility.Visible;
                    DamageEnabled.Visibility = Visibility.Visible;
                    DirtText.Visibility = Visibility.Visible;
                    DamageText.Visibility = Visibility.Visible;
                    GameUpdate.Visibility = Visibility.Visible;
                    OnlineEnabled.Visibility = Visibility.Visible;
                    OnlineText.Visibility = Visibility.Visible;
                    SetConnection.Visibility = Visibility.Visible;
                    if (build == Build.Packed)
                    {
                        GameUpdate.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        GameUpdate.Visibility = Visibility.Hidden;
                    }
                    break;
            }
        }

        public void SetInfos(GameProfile gameProfile)
        {
            try
            {
                currentProfile = gameProfile;
                for (int i = 0; i < BackgroundWorker.GameProfiles.Count; i++)
                {
                    if (BackgroundWorker.GameProfiles[i].Profilename == gameProfile.Profilename)
                    {
                        Database.Database.p2Database.Usersettings.SelectedProfile = i;
                        break;
                    }
                }

                CreateEdit.Text = Lang.GetText(20) + " (" + currentProfile.Profilename + ")";
                GameProfilename = gameProfile.Profilename;
                GamePath = gameProfile.Gamepath;
                StartArgs = gameProfile.Arguments;
                Log.Info("Successfully set profile: " + gameProfile.Profilename);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to set profile: " + gameProfile?.Profilename ?? "Unknown Profile", ex);
            }
        }

        #region Set Profile Settings
        private void ProfileNameUpdate(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox outerTextBox)
            {
                var innerTextBox = FindChild<TextBox>(outerTextBox, "Input");
                if (innerTextBox != null)
                {
                    string inputText = innerTextBox.Text;
                    GameProfilename = inputText.Replace(" ", "-");
                }
            }
        }

        private void StartArgsUpdate(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox outerTextBox)
            {
                var innerTextBox = FindChild<TextBox>(outerTextBox, "Input");
                if (innerTextBox != null)
                {
                    string inputText = innerTextBox.Text;
                    StartArgs = inputText;
                }
            }
        }

        private void OnRamEdit(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            currentProfile.LAAEnabled = (bool)MagicRam.IsChecked;
        }

        private void MagicCoresUpdate(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            currentProfile.UseMoreCores = (bool)Cores.IsChecked;
        }

        private void MagicPrioUpdate(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            currentProfile.HighPrio = (bool)Priority.IsChecked;
        }

        private void VehicleDirtUpdate(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            currentProfile.VehicleDirt = (bool)DirtEnabled.IsChecked;
        }

        private void VehicleDamageUpdate(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            currentProfile.VehicleDamage = (bool)DamageEnabled.IsChecked;
        }

        private void OnlineBuildUpdate(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            currentProfile.OnlineMode = (bool)OnlineEnabled.IsChecked;
        }

        #endregion

        #region Buttons
        private void OnSelectGame(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GameProfilename))
            {
                MessageBox.Show(string.Format(Lang.GetText(21) + " ?????", GamePath), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            ModBrowser.Visibility = Visibility.Hidden;
            SetConnection.Visibility = Visibility.Hidden;
            DamageEnabled.Visibility = Visibility.Hidden;
            DirtEnabled.Visibility = Visibility.Hidden;
            DirtText.Visibility = Visibility.Hidden;
            DamageText.Visibility = Visibility.Hidden;
            GameUpdate.Visibility = Visibility.Hidden;
            SetConnection.Visibility = Visibility.Hidden;
            ArgumentText.Visibility = Visibility.Hidden;
            Arguments.Visibility = Visibility.Hidden;
            MagicRam.Visibility = Visibility.Hidden;
            Cores.Visibility = Visibility.Hidden;
            Priority.Visibility = Visibility.Hidden;
            OnlineEnabled.Visibility = Visibility.Hidden;
            MagicRamText.Visibility = Visibility.Hidden;
            OnlineText.Visibility = Visibility.Hidden;
            MagicCoresText.Visibility = Visibility.Hidden;
            MagicPrioText.Visibility = Visibility.Hidden;
            GameSelect.Content = Lang.GetText(29);
            OpenGame.Content = Lang.GetText(30);
            GameSelect.ToolTip = Lang.GetTooltipText(12);
            OpenGame.ToolTip = Lang.GetTooltipText(13);
            Save.Content = Lang.GetText(34);
            Save.ToolTip = Lang.GetTooltipText(17);

            OpenFileDialog open = new OpenFileDialog
            {
                Title = Lang.GetText(41),
                InitialDirectory = Regestry.GetPath(),
                DefaultExt = ".exe",
                Filter = "TestDrive Games|TestDrive2.exe;TestDriveUnlimited.exe;TestDrive2Dev.exe;TestDrive2Dev.exe|All Files (*.*)|*.*"

            };

            if (open.ShowDialog() == true)
            {
                GamePath = open.FileName;
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(GamePath);
                ProductVersion.Text = Lang.GetText(35) + fvi.ProductVersion.ToString();
                FileVersion.Text = string.Format(Lang.GetText(36), fvi.FileVersion.ToString());
                Gamedir.Text = string.Format(Lang.GetText(37), System.IO.Path.GetFullPath(GamePath).Replace(@"\TestDrive2.exe", "").Replace(@"\TestDriveUnlimited.exe", ""));
                GameSize.Text = string.Format(Lang.GetText(38), FormatFileSize(GetDirectorySize(System.IO.Path.GetDirectoryName(GamePath), true)));
                if (GamePath.EndsWith("TestDriveUnlimited.exe"))
                {
                    GameType.Text = string.Format(Lang.GetText(39), "TDU1", "Packed", "Standalone");
                    ProductVersion.Inlines.Clear();
                    ProductVersion.Inlines.Add(new Run(Lang.GetText(35)) { Foreground = Brushes.White });
                    Brush versionColor = fvi.ProductVersion.ToString().Equals("MC 1.66 A")
                        ? Brushes.Green
                        : Brushes.Red;
                    ProductVersion.Inlines.Add(new Run(fvi.ProductVersion.ToString()) { Foreground = versionColor });
                    if (!fvi.ProductVersion.ToString().Equals("MC 1.66 A"))
                    {
                        ProductVersion.Inlines.Add(new Run(Lang.GetText(40))
                        {
                            Foreground = Brushes.White
                        });
                    }
                }
                else if (GamePath.EndsWith("TestDrive2.exe"))
                {
                    GameType.Text = string.Format(Lang.GetText(39), "TDU2", " ???", " ???");
                    ProductVersion.Inlines.Clear();
                    ProductVersion.Inlines.Add(new Run(Lang.GetText(35)) { Foreground = Brushes.White });
                    Brush versionColor = fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034")
                        ? Brushes.Green
                        : Brushes.Red;

                    ProductVersion.Inlines.Add(new Run(fvi.ProductVersion.ToString()) { Foreground = versionColor });
                    if (!fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034"))
                    {
                        ProductVersion.Inlines.Add(new Run(Lang.GetText(40))
                        {
                            Foreground = Brushes.White
                        });
                    }
                    DamageEnabled.Visibility = Visibility.Visible;
                    DirtEnabled.Visibility = Visibility.Visible;
                    DirtText.Visibility = Visibility.Visible;
                    DamageText.Visibility = Visibility.Visible;
                    OnlineEnabled.Visibility = Visibility.Visible;
                    OnlineText.Visibility = Visibility.Visible;
                }
                else if (GamePath.EndsWith("TestDrive2Dev.exe"))
                {
                    GameType.Text = string.Format(Lang.GetText(39), "TDU2", " ???", " ???");
                    ProductVersion.Inlines.Clear();
                    ProductVersion.Inlines.Add(new Run(Lang.GetText(35)) { Foreground = Brushes.White });
                    Brush versionColor = fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034")
                        ? Brushes.Green
                        : Brushes.Red;

                    ProductVersion.Inlines.Add(new Run(fvi.ProductVersion.ToString()) { Foreground = versionColor });
                    if (!fvi.ProductVersion.ToString().Equals("TDU2 DLC2 v034"))
                    {
                        ProductVersion.Inlines.Add(new Run(Lang.GetText(40))
                        {
                            Foreground = Brushes.White
                        });
                    }
                    DamageEnabled.Visibility = Visibility.Visible;
                    DirtEnabled.Visibility = Visibility.Visible;
                    DirtText.Visibility = Visibility.Visible;
                    DamageText.Visibility = Visibility.Visible;
                    OnlineEnabled.Visibility = Visibility.Visible;
                    OnlineText.Visibility = Visibility.Visible;
                }
                ArgumentText.Visibility = Visibility.Visible;
                Arguments.Visibility = Visibility.Visible;
                MagicRam.Visibility = Visibility.Visible;
                MagicRamText.Visibility = Visibility.Visible;
                Cores.Visibility = Visibility.Visible;
                MagicCoresText.Visibility = Visibility.Visible;
                Priority.Visibility = Visibility.Visible;
                MagicPrioText.Visibility = Visibility.Visible;
            }
        }

        private void OnGameUpdate(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.Gamedirectory = System.IO.Path.GetFullPath(GamePath).Replace(@"\TestDrive2.exe", "");
            Database.Database.Write();
            MainViewModel.GetUpdateView();
        }

        private void OnSetConnection(object sender, RoutedEventArgs e)
        {
            if (currentProfile == null)
                return;
            if (Directory.Exists(currentProfile.Basedir))
            {
                if (File.Exists(currentProfile.Basedir + "/GamePC.cpr"))
                    File.Delete(currentProfile.Basedir + "/GamePC.cpr");

                File.WriteAllBytes(currentProfile.Basedir + "/GamePC.cpr", Properties.Resources.GamePC);
                MessageBox.Show(Lang.GetText(42), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(Lang.GetText(43), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveProfile(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(GameProfilename) || !string.IsNullOrEmpty(GamePath) || !string.IsNullOrEmpty(StartArgs))
            {
                try
                {
                    GameProfile profile = new GameProfile(GameProfilename, GamePath, StartArgs);

                    profile.LAAEnabled = (bool)MagicRam.IsChecked;
                    profile.UseMoreCores = (bool)Cores.IsChecked;
                    profile.HighPrio = (bool)Priority.IsChecked;
                    profile.VehicleDirt = (bool)DirtEnabled.IsChecked;
                    profile.VehicleDamage = (bool)DamageEnabled.IsChecked;
                    profile.OnlineMode = (bool)OnlineEnabled.IsChecked;


                    profile.EditExe(0x00AE0494L, profile.VehicleDirt, profile.Gamepath);
                    profile.EditExe(0x00AE0484L, profile.VehicleDamage, profile.Gamepath);

                    string jsonData = JsonConvert.SerializeObject(profile, Formatting.Indented);

                    if (File.Exists(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile"))
                    {
                        File.Delete(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile");
                    }

                    File.WriteAllText(Constans.DokumentsFolder + "/GameProfiles/" + profile.Profilename + ".profile", jsonData);
                    MessageBox.Show(string.Format(Lang.GetText(45), GameProfilename), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Lang.GetText(46), ex.Message), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(string.Format(Lang.GetText(44), GamePath), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            BackgroundWorker.RefreshProfiles();
            MainViewModel.OpenLauncherProfiles();
        }

        private void OpenMods(object sender, RoutedEventArgs e)
        {
            Database.Database.p2Database.Usersettings.Gamedirectory = System.IO.Path.GetFullPath(GamePath).Replace(@"\TestDrive2.exe", "").Replace(@"\TestDrive2Dev.exe", "").Replace(@"\TestDriveUnlimited.exe", "");
            Database.Database.Write();
            MainViewModel.OpenModbrowser();
        }

        private void OnOpenGamefolder(object sender, RoutedEventArgs e)
        {

            if (currentProfile == null)
                return;

            OpenDirectory(currentProfile.Gamepath.Replace(@"\TestDrive2.exe", "").Replace(@"\TestDrive2Dev.exe", "").Replace(@"\TestDriveUnlimited.exe", ""));
        }

        /// <summary>
        /// Helper method to open a directory in Windows Explorer.
        /// </summary>
        /// <param name="folderPath">The folder path to open.</param>
        /// <param name="errorMessage">The error message to display if the folder does not exist.</param>
        private void OpenDirectory(string folderPath)
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
                MessageBox.Show(Lang.GetText(44) + "\n" + folderPath.Replace("/", @"\"), "Project Paradise 2", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild && ((FrameworkElement)child).Name == childName)
                {
                    return typedChild;
                }

                var result = FindChild<T>(child, childName);
                if (result != null)
                    return result;
            }
            return null;
        }

        static long GetDirectorySize(string folderPath, bool includeSubDirs)
        {
            var option = includeSubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateFiles(folderPath, "*", option)
                            .Sum(file => new FileInfo(file).Length);
        }

        public string FormatFileSize(long fileSize)
        {
            string[] sizeUnits = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB" };
            double size = fileSize;
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return string.Format("{0:0.##} {1}", size, sizeUnits[unitIndex]);
        }
    }
}