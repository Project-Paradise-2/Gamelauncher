using ProjectParadise2.Core.Log;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
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
            UpdateProgress(0, 100);
            var worker = new Thread(StartUpdate);
            worker.IsBackground = true;
            worker.Start();
        }

        /// <summary>
        /// Load the Launcher Updater based on the current build type (Packed or Unpacked).
        /// </summary>
        private void StartUpdate()
        {
            Log.Info("Start loading Updater as Type: " + Constans.launcherbuild);

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
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
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

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, DownloadProgressChangedEventArgs>(DownloadProgressCallback), sender, e);
                return;
            }
            UpdateProgress(e.ProgressPercentage, 100);
        }

        private void DownloadFileComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, AsyncCompletedEventArgs>(DownloadFileComplete), sender, e);
                return;
            }

            Progress.Visibility = Visibility.Hidden;
            ProgressText.Visibility = Visibility.Hidden;
            UpdateBTN.Visibility = Visibility.Visible;
            string response = ProjectParadise2.Core.BackgroundWorker.LauncherNews;
            string[] news = response.Split('|');
            this.ChangeLog.Text = "Launcher Update:\n\n" + news[1];
            this.Version.Content = "Local Version: " + Constans.LauncherVersion + "  Online: " + news[0] + " (ready for update)";
        }

        private void RunUpdate(object sender, RoutedEventArgs e)
        {
            SkippBTN.Visibility = Visibility.Hidden;
            Thread.Sleep(2000);
            try
            {
                string appPath = Database.Database.p2Database.Usersettings.Gamedirectory + "/updater.exe";
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    //UseShellExecute = true,
                    Verb = "runas"
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
            this.ChangeLog.Text = "Launcher Update:\n\n" + news[1];
            this.Version.Content = "Local Version: " + Constans.LauncherVersion + "  Online: " + news[0] + " (wait for Updater)";
        }

        private void SkipUpdate(object sender, RoutedEventArgs e)
        {
            MainViewModel.GetMainview();
        }

        public void UpdateProgress(int totalSteps, int currentStep)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<int, int>(UpdateProgress), totalSteps, currentStep);
                return;
            }
            Progress.Value = totalSteps;
            Progress.Maximum = currentStep;
        }
    }
}