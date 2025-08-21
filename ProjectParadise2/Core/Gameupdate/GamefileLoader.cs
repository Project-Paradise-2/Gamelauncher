using ProjectParadise2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace ProjectParadise2.Core.Gameupdate
{
    internal class GamefileLoader
    {
        private static Stopwatch stopWatch;
        public static int NeedFiles = 0;
        public static int HasFiles = 0;
        public static bool NeedUpdate = false;

        #region Download Info Holder
        public static List<string> UpdateFile = new List<string>();
        public static List<string> InstallDir = new List<string>();
        public static List<string> UpdateName = new List<string>();
        #endregion

        /// <summary>
        /// Adds a new file to the download queue.
        /// </summary>
        /// <param name="File">The URL of the file to download.</param>
        /// <param name="Filename">The name of the file to be downloaded.</param>
        /// <param name="FileDestination">The destination directory for the file.</param>
        public static void AddFile(string File, string Filename, string FileDestination)
        {
            NeedUpdate = true;
            UpdateFile.Add(File);
            InstallDir.Add(FileDestination);
            UpdateName.Add(Filename);
        }

        /// <summary>
        /// Creates any missing directories required for the update process.
        /// </summary>
        public static void CreateDirs()
        {
            NeedFiles = UpdateFile.Count;

            if (NeedFiles > 70)
            {
                UpdateView.Instance.PrintMessage(Lang.GetText(59), 4);
                UpdateView.Instance.Updatestatus(Lang.GetText(60));
                NeedUpdate = false;
                UpdateFile.Clear();
                Log.Log.Print("[UPDATER]Stop Gameupdate, To many Missing File. Looks to an not Correct installed Game.. Sorry Mate cant share the Game");
                return;
            }

            stopWatch = new Stopwatch();
            stopWatch.Start();

            // Verifizierungsschritt (Stage 2)
            for (int i = 0; i < Filecheck.Live.Directorys.Length; i++)
            {
                if (!Directory.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + Filecheck.Live.Directorys[i]))
                {
                    Thread.Sleep(2);
                    Directory.CreateDirectory(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + Filecheck.Live.Directorys[i]);
                    UpdateView.Instance.PrintMessage(Lang.GetText(61) + Filecheck.Live.Directorys[i], 1);
                }
            }

            // Update den Status und die ProgressBar für die Verifikation
            UpdateView.Instance.Updatestatus(Lang.GetText(62));

            // Hier wird die Fortschrittsanzeige für die Verifizierung korrekt gesetzt.
            UpdateView.Instance.UpdateProgress(0, NeedFiles, Lang.GetText(63));

            // Wenn neue Dateien heruntergeladen werden müssen, starte den Downloadprozess
            if (NeedUpdate.Equals(true))
            {
                UpdateView.Instance.WriteCurrentDownload(0, 0, "", "", "");
                UpdateView.Instance.UpdateProgress(0, 0, Lang.GetText(64));
                GetFiles();
            }
        }

        /// <summary>
        /// Initiates the download of files in the update queue.
        /// </summary>
        public static void GetFiles()
        {
            if (HasFiles != NeedFiles)
            {
                try
                {
                    // Berechnung des Gesamtfortschritts basierend auf der Anzahl der heruntergeladenen Dateien
                    int percentage = (int)((double)(HasFiles + 1) / NeedFiles * 100);

                    // Update der Gesamtfortschrittsanzeige
                    UpdateView.Instance.UpdateProgress(NeedFiles, HasFiles + 1, string.Format(Lang.GetText(65), UpdateName[HasFiles], percentage));

                    WebClient client = new WebClient();
                    Thread.Sleep(25);  // Kurze Pause zur Verbesserung der Performance
                    Uri uri = new Uri(Constans.Cdn + "/update/" + InstallDir[HasFiles]);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileComplete);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                    client.DownloadFileAsync(uri, (Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles] + ".zip"));
                }
                catch (Exception ex)
                {
                    Log.Log.Print($"[UPDATER] Failed to download file: ({InstallDir[HasFiles]}).", ex);
                }
            }
            else
            {
                // Wenn alle Dateien heruntergeladen sind, stoppe den Timer und gebe eine Erfolgsnachricht aus
                stopWatch.Stop();
                TimeSpan elapsed = stopWatch.Elapsed;
                string formattedTime = string.Empty;

                if (elapsed.Hours > 0)
                {
                    formattedTime += string.Format("{0}h:", elapsed.Hours);
                }

                if (elapsed.Minutes > 0 || elapsed.Hours > 0)
                {
                    formattedTime += string.Format("{0:D2}m:", elapsed.Minutes);
                }

                formattedTime += string.Format("{0:D2}s.{1:D3}ms", elapsed.Seconds, elapsed.Milliseconds);

                UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(66), formattedTime), 5);
                UpdateView.Instance.UpdateProgress(0, 0, Lang.GetText(67));
                UpdateView.Instance.WriteCurrentDownload(0, 0, "", "", "");

                // Aktualisiere die Versionsnummer, wenn nötig
                if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.ExePath))
                {
                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(Database.Database.p2Database.Usersettings.ExePath);
                    Database.Database.p2Database.Usersettings.ProductVersion = myFileVersionInfo.ProductVersion.ToString();
                    Database.Database.p2Database.Usersettings.FileVersion = myFileVersionInfo.FileVersion.ToString();
                    Database.Database.Write();
                }
            }
        }

        /// <summary>
        /// Extracts the downloaded ZIP file and deletes it afterward.
        /// </summary>
        public static void InstallFile()
        {
            try
            {
                UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(69), UpdateName[HasFiles], InstallDir[HasFiles]), 1);
                Thread.Sleep(10);
                using (ZipArchive archive = ZipFile.OpenRead(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles] + ".zip"))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles].Replace(".zip", string.Empty), true);
                    }
                }
                Thread.Sleep(10);
                File.Delete((Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles]) + ".zip");
                // Update mit Nachricht
                UpdateView.Instance.PrintMessage(Lang.GetText(70) + InstallDir[HasFiles], 1);
            }
            catch (Exception ex)
            {
                Log.Log.Print($"[UPDATER] Failed to extract file: ({InstallDir[HasFiles]}).", ex);
                UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(71), InstallDir[HasFiles], ex.Message), 0);
            }
        }

        /// <summary>
        /// Updates the progress of the download and calculates the current download speed.
        /// </summary>
        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            DateTime now = DateTime.Now;

            // Berechnung des Fortschritts für den aktuellen Dateidownload (in Prozent)
            int downloadProgress = (int)((double)e.BytesReceived / e.TotalBytesToReceive * 100);

            if ((now - LastUpdatedState).TotalMilliseconds >= 1000)  // Update nur jede Sekunde
            {
                double elapsedSeconds = (now - LastUpdatedState).TotalSeconds;
                long bytesPerSecond = (long)((e.BytesReceived - lastTotalBytesReceived) / elapsedSeconds);


                // Update der Download-Ansicht mit Fortschritt der aktuellen Datei
                UpdateView.Instance.WriteCurrentDownload(downloadProgress, 100, string.Format(Lang.GetText(65), UpdateName[HasFiles], downloadProgress),
                                                          FormatBytes(e.BytesReceived) + " / " + FormatBytes(e.TotalBytesToReceive),
                                                          FormatBytes(bytesPerSecond) + "/s");

                lastTotalBytesReceived = e.BytesReceived;
                LastUpdatedState = now;
            }
        }

        /// <summary>
        /// Handles the completion of a download and initiates the next file download.
        /// </summary>
        private static void DownloadFileComplete(object sender, AsyncCompletedEventArgs e)
        {
            InstallFile();
            Thread.Sleep(50);
            HasFiles++;
            GetFiles();
        }

        /// <summary>
        /// Formats the size in bytes into a human-readable string (e.g., KB, MB, GB).
        /// </summary>
        private static string FormatBytes(long bytes)
        {
            if (bytes < 0)
                return "";

            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            double size = bytes;

            while (size >= 1024 && i < suffix.Length - 1)
            {
                size /= 1024.0;
                i++;
            }

            return string.Format("{0:0.##} {1}", size, suffix[i]);
        }

        private static DateTime LastUpdatedState = DateTime.MinValue;
        private static long lastTotalBytesReceived = 0;
    }
}