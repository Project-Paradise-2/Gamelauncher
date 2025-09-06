using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.Net;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeView"/> class.
        /// Sets up the news display and server state monitoring.
        /// </summary>
        public HomeView()
        {
            InitializeComponent();
            Instance = this;
            while (MainViewModel.Instance != null)
            {
                MainViewModel.HomeView = this;
                break;
            }

            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            try
            {
                LoadNewsFromServer();
            }
            catch (WebException)
            {
                try
                {
                    LoadNewsFromServer();
                }
                catch (WebException wx)
                {
                    this.NewsMessage.Text = "Failed to get the latest news:\n" + wx.Message;
                    Log.Error("Failed to get the latest news: ", wx);
                }
                catch (Exception ex)
                {
                    this.NewsMessage.Text = "An unexpected error occurred:\n" + ex.Message;
                    Log.Error("An unexpected error occurred: ", ex);
                }
            }

            RefreshState(null, null);
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
            LastNews.Content = Lang.GetText(7);
            RunGameButton.Content = Lang.GetText(0);
            if (Database.Database.p2Database.Usersettings.Onlinemode)
                RunGameButton.Foreground = Brushes.DarkGreen;
            else
                RunGameButton.Foreground = Brushes.DarkRed;
        }

        /// <summary>
        /// Sets the language for UI elements based on user settings.
        /// </summary>
        private void SetLang()
        {
            LastNews.Content = Lang.GetText(7);
            RunGameButton.Content = Lang.GetText(0);
            if (Database.Database.p2Database.Usersettings.Onlinemode)
                RunGameButton.Foreground = Brushes.DarkGreen;
            else
                RunGameButton.Foreground = Brushes.DarkRed;
        }

        /// <summary>
        /// Refreshes the server state information and updates the UI.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The mouse button event arguments.</param>
        private void RefreshState(object sender, MouseButtonEventArgs e)
        {
            var info = HomeViewModel.CheckState();
            if (info != null)
            {
                this.ServerStateText.Content = Lang.GetText(111) + info.playercount;
                this.ServerStateLamp.Background = Brushes.Green;
            }
            else
            {
                this.ServerStateText.Content = Lang.GetText(112);
                this.ServerStateLamp.Background = Brushes.Red;
            }
        }

        /// <summary>
        /// Starts the game using the background worker.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The routed event arguments.</param>
        private void StartGame(object sender, RoutedEventArgs e)
        {
            BackgroundWorker.RunGame();
        }

        /// <summary>
        /// Loads news data from the server and updates the UI.
        /// </summary>
        private void LoadNewsFromServer()
        {
            using (WebConnection wc = new WebConnection())
            {
                wc.Timeout = 10;
                string response = System.Text.Encoding.UTF8.GetString(wc.DownloadData("https://cdn.project-paradise2.de/Requests/Launchernews.php"));

                string[] news = response.Split('|');
                this.NewsMessage.Text = news[0];

                UpdateNewsBoxStyle(news);
            }
        }

        /// <summary>
        /// Updates the news box style based on the server response.
        /// </summary>
        /// <param name="news">An array containing news data.</param>
        private void UpdateNewsBoxStyle(string[] news)
        {
            switch (news[1])
            {
                case "Black":
                    NewsBox.Foreground = Brushes.Black;
                    break;
                case "White":
                    NewsBox.Foreground = Brushes.White;
                    break;
                case "Green":
                    NewsBox.Foreground = Brushes.Green;
                    break;
                case "Red":
                    NewsBox.Foreground = Brushes.Red;
                    break;
                case "Orange":
                    NewsBox.Foreground = Brushes.Orange;
                    break;
                case "Yellow":
                    NewsBox.Foreground = Brushes.Yellow;
                    break;
                case "Blue":
                    NewsBox.Foreground = Brushes.Blue;
                    break;
                case "Grey":
                    NewsBox.Foreground = Brushes.Gray;
                    break;
                case "Cyan":
                    NewsBox.Foreground = Brushes.Cyan;
                    break;
                case "Magenta":
                    NewsBox.Foreground = Brushes.Magenta;
                    break;
            }

            switch (news[2])
            {
                case "smal":
                    NewsBox.FontSize = 12;
                    break;
                case "medium":
                    NewsBox.FontSize = 14;
                    break;
                case "big":
                    NewsBox.FontSize = 16;
                    break;
                case "verybig":
                    NewsBox.FontSize = 20;
                    break;
            }
        }
    }
}