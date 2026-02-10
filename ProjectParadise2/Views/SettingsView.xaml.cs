using ProjectParadise2.Core;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
            Instance = this;
            InitializeSettingsRefreshThread();
        }

        /// <summary>
        /// Initializes and starts the refresh thread to update UI elements based on the current settings.
        /// </summary>
        private void InitializeSettingsRefreshThread()
        {
            Thread refreshThread = new Thread(Refresh);
            refreshThread.IsBackground = true; // Ensures the thread does not block application exit
            refreshThread.Start();
        }

        /// <summary>
        /// Refreshes the settings UI by updating toggle states and game information.
        /// This method ensures that the UI is updated on the main thread.
        /// </summary>
        public void Refresh()
        {
            Thread.Sleep(10); // Short delay to ensure UI elements are ready for update

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(Refresh)); // Ensures the method runs on the UI thread
                return;
            }

            this.DiscordToggle.IsChecked = Database.Database.p2Database.Usersettings.DiscordRPC;
            this.UpdateToggle.IsChecked = Database.Database.p2Database.Usersettings.Autoupdatecheck;
            this.UPNPToggle.IsChecked = Database.Database.p2Database.Usersettings.UpnpWorker;
            this.LauncherLang.SelectedIndex = Database.Database.p2Database.Usersettings.LangId;
            this.LauncherLang.SelectedItem = Database.Database.p2Database.Usersettings.LangId;
            var state = Regestry.GetAudiosetting();

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
            if (Database.Database.p2Database.Usersettings.UpnpWorker == false)
                _ = NatDetector.RemoveUpnp();
            Save();
        }

        /// <summary>
        /// Saves the current state of the user's settings to the database and refreshes the UI.
        /// </summary>
        private void Save()
        {
            Database.Database.Write();
            Refresh(); // Refresh UI with the updated settings
        }

        /// <summary>
        /// Event to fire when the language selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            DiscordTxt.Text = Lang.GetText(13);
            UpdateTxt.Text = Lang.GetText(14);
            Upnp.Text = Lang.GetText(15);
            LangTxt.Text = Lang.GetText(16);
            AudioTxt.Text = Lang.GetText(17);

            DiscordToggle.ToolTip = Lang.GetTooltipText(0);
            UpdateToggle.ToolTip = Lang.GetTooltipText(1);
            UPNPToggle.ToolTip = Lang.GetTooltipText(2);
            LauncherLang.ToolTip = Lang.GetTooltipText(3);
            Audiomode.ToolTip = Lang.GetTooltipText(4);
        }
    }
}