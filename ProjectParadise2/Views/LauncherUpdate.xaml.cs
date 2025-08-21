using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaktionslogik für LauncherUpdate.xaml
    /// </summary>
    public partial class LauncherUpdate : UserControl
    {
        public LauncherUpdate()
        {
            InitializeComponent();
            UpdateBTN.Visibility = Visibility.Hidden;
            LoadVersionFromServer();
            if (Constans.launcherbuild == Launcherbuild.Packed)
            {
                GetUpdater("packed");
            }
            if (Constans.launcherbuild == Launcherbuild.Unpacked)
            {
                GetUpdater("unpacked");
            }
        }

        public void GetUpdater(string Type)
        {

            try
            {
                WebClient client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileComplete);
                Uri uri = new Uri("https://cdn.project-paradise2.de/launcherfiles/new/Updater/" + Type + ".exe");

                if (File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/updater.exe"))
                    File.Delete(Database.Database.p2Database.Usersettings.Gamedirectory + "/updater.exe");

                client.DownloadFileAsync(uri, Database.Database.p2Database.Usersettings.Gamedirectory + "/updater.exe");
            }
            catch (Exception)
            {
                this.ChangeLog.Text = "Failed load Updater, Perform Manual Update";
            }
        }

        private void DownloadFileComplete(object sender, AsyncCompletedEventArgs e)
        {
            UpdateBTN.Visibility = Visibility.Visible;
            string response = ProjectParadise2.Core.BackgroundWorker.LauncherNews;
            string[] news = response.Split('|');
            this.ChangeLog.Text = news[1];
            this.Version.Content = "Local Version: " + Constans.LauncherVersion + "  Online: " + news[0] + " (ready for update)";
        }

        private void RunUpdate(object sender, RoutedEventArgs e)
        {
            try
            {
                string appPath = Database.Database.p2Database.Usersettings.Gamedirectory + "/updater.exe";
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    //UseShellExecute = true,
                    //Verb = "runas"
                };

                Process.Start(processInfo);
            }
            catch (Exception)
            {
            }
            Core.BackgroundWorker.CloseLauncher();
        }


        private void LoadVersionFromServer()
        {
            string response = ProjectParadise2.Core.BackgroundWorker.LauncherNews;
            string[] news = response.Split('|');
            this.ChangeLog.Text = news[1];
            this.Version.Content = "Local Version: " + Constans.LauncherVersion + "  Online: " + news[0] + " (wait for Updater)";
        }

        private void SkipUpdate(object sender, RoutedEventArgs e)
        {
            MainViewModel.GetMainview();
        }
    }
}