using ProjectParadise2.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public static HomeView Instance { get; private set; }
        private List<NewsEntry> _newsEntries = new List<NewsEntry>();
        private int _currentIndex = 0;
        public bool IsLoaded { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeView"/> class.
        /// Sets up the news display and server state monitoring.
        /// </summary>
        public HomeView()
        {
            InitializeComponent();
            Instance = this;
            BackgroundWorker.OnLangset += SetLang;
            SetLang(null, null);
            Playbutton.Visibility = Visibility.Hidden;
            if (!NatDetector.CanRun)
                while (MainViewModel.Instance != null)
                {
                    MainViewModel.HomeView = this;
                    break;
                }
            _newsEntries = BackgroundWorker.GetLauncherNews();
            GameProfiles.SelectedIndex = 0;
            RefreshState(null, null);

        }

        public void AddProfiles(List<GameProfile> profiles)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<List<GameProfile>>(AddProfiles), profiles);
                return;
            }
            GameProfiles.Items.Clear();
            foreach (var profile in profiles)
            {
                GameProfiles.Items.Add(new ComboBoxItem() { Content = profile.Profilename });
            }
            this.GameProfiles.SelectedIndex = Database.Database.p2Database.Usersettings.SelectedProfile;
            this.GameProfiles.SelectedItem = Database.Database.p2Database.Usersettings.SelectedProfile;
        }

        private void NextNews(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < _newsEntries.Count - 1)
            {
                _currentIndex++;
                ShowNews();
            }
        }

        private void PrevNews(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                ShowNews();
            }
        }

        private void ShowNews()
        {
            if (_newsEntries.Count == 0)
            {
                NewsTitle.Text = "No News";
                NewsText.Text = "No news entries available.";
                NavPanel.Visibility = Visibility.Collapsed;
                return;
            }

            var current = _newsEntries[_currentIndex];
            NewsTitle.Text = current.Title + " - " + current.Date.ToString("d");
            NewsText.Text = current.Content;

            // Buttons nur anzeigen, wenn mehr als 1 Eintrag existiert
            NavPanel.Visibility = _newsEntries.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        public void ShowRunGameButton()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(ShowRunGameButton);
                return;
            }
            Playbutton.Visibility = Visibility.Visible;
            Playbutton.IsEnabled = true;
        }

        public void HideRunGameButton()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(HideRunGameButton);
                return;
            }
            Playbutton.Visibility = Visibility.Hidden;
            Playbutton.IsEnabled = false;
        }

        /// <summary>
        /// Sets the language for UI elements based on user settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            this.Playbutton.Content = Lang.GetText(0);
        }

        /// <summary>
        /// Refreshes the server state information and updates the UI.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The mouse button event arguments.</param>
        private void RefreshState(object sender, MouseButtonEventArgs e)
        {
            if (BackgroundWorker.NetworkTestsDone)
            {
                Playbutton.Visibility = Visibility.Visible;
            }

            var info = HomeViewModel.CheckState();
            if (info != null)
            {
                this.ServerStateText.Content = Lang.GetText(115) + info.playercount;
                this.ServerStateLamp.Background = Brushes.Green;
            }
            else
            {
                this.ServerStateText.Content = Lang.GetText(116);
                this.ServerStateLamp.Background = Brushes.Red;
            }

            BackgroundWorker.RefreshProfiles();
            this.GameProfiles.SelectedIndex = Database.Database.p2Database.Usersettings.SelectedProfile;
            this.GameProfiles.SelectedItem = Database.Database.p2Database.Usersettings.SelectedProfile;
            ShowNews();
        }

        /// <summary>
        /// Starts the game using the background worker.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The routed event arguments.</param>
        private void StartGame(object sender, RoutedEventArgs e)
        {
            HideRunGameButton();
            BackgroundWorker.RunGame();
        }

        private void DefineProfile(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                IsLoaded = true;
                return;
            }
            int id = GameProfiles.SelectedIndex;
            Database.Database.p2Database.Usersettings.SelectedProfile = id;
            Database.Database.Write();
        }
    }
}