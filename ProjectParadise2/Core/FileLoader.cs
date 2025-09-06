using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

namespace ProjectParadise2.Core
{
    class FileLoader
    {
        public static int NeedFiles = 0;
        public static int HasFiles = 0;
        public static bool NeedUpdate = false;
        public static int PercentLoad = 0;

        #region Download Info Holder
        public static List<string> UpdateFile = new List<string>();
        public static List<string> InstallDir = new List<string>();
        public static List<string> UpdateName = new List<string>();
        #endregion

        /// <summary>
        /// Adds a new file to the download queue.
        /// </summary>
        /// <param name="File">The URL of the file to download.</param>
        /// <param name="Filename">The name of the file.</param>
        /// <param name="FileDestination">The destination path for the file.</param>
        public static void AddFile(string File, string Filename, string FileDestination)
        {
            NeedFiles++;
            NeedUpdate = true;
            UpdateFile.Add(File);
            InstallDir.Add(FileDestination);
            UpdateName.Add(Filename);
        }

        /// <summary>
        /// Prepares the file loading process by cleaning up any existing files and initiating downloads.
        /// </summary>
        public static void PrepareFileloading()
        {
            if (NeedUpdate)
            {
                try
                {
                    foreach (var item in InstallDir)
                    {
                        if (File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles]))
                        {
                            File.Delete(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles]);
                        }
                    }
                }
                catch (Exception)
                {
                    //TODO: Handle file deletion exceptions
                }

                GetFiles();
            }
        }

        /// <summary>
        /// Initiates the file download process for all queued files.
        /// </summary>
        public static void GetFiles()
        {
            try
            {
                if (HasFiles != NeedFiles)
                {
                    WebClient client = new WebClient();
                    Thread.Sleep(120);
                    string tmpUri = Path.Combine(Constans.Modloader, UpdateName[HasFiles]);
                    Uri uri = new Uri(tmpUri + ".zip");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileComplete);
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                    client.DownloadFileAsync(uri, (Database.Database.p2Database.Usersettings.Gamedirectory + "/" + InstallDir[HasFiles] + ".zip"));

                    Log.Log.PrintMod("[MOD] Start Download: " + InstallDir[HasFiles], Modloader.Installing.Modname);
                }
                else
                {
                    Modloader.SendMessage("Mod installation finished", 100, 100, true);
                    Modloader.IsRunning = false;
                }
            }
            catch (Exception ex)
            {
                Log.Log.Error("[MOD]Failed to get File: " + InstallDir[HasFiles] + " : " + ex.Message);
            }
        }

        private static DateTime LastUpdatedState;
        private static long lastTotalBytesReceived = 0;

        /// <summary>
        /// Handles the download progress and updates the progress percentage.
        /// </summary>
        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            if (LastUpdatedState <= DateTime.Now.AddMilliseconds(-1000.0))
            {
                long bytes = (long)((e.BytesReceived - lastTotalBytesReceived) / (DateTime.Now - LastUpdatedState).TotalSeconds);
                PercentLoad = e.ProgressPercentage;
                Modloader.SendMessage(
                    string.Format("Downloading file: {0} {1}/{2}", "", FormatBytes(e.BytesReceived), FormatBytes(e.TotalBytesToReceive)),
                    100,
                    e.ProgressPercentage,
                    false,
                    FormatBytes(bytes)
                );
                lastTotalBytesReceived = e.BytesReceived;
                LastUpdatedState = DateTime.Now;
            }
        }

        /// <summary>
        /// Called when a file download is complete, triggering the installation process.
        /// </summary>
        private static void DownloadFileComplete(object sender, AsyncCompletedEventArgs e)
        {
            Log.Log.PrintMod("[MOD] File Download Complete", Modloader.Installing.Modname);
            InstallFile();
            Thread.Sleep(100);
            HasFiles++;
            GetFiles();
        }

        /// <summary>
        /// Installs the downloaded file by extracting its contents.
        /// </summary>
        public static void InstallFile()
        {
            try
            {
                if (File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + InstallDir[HasFiles]))
                {
                    File.Delete(Database.Database.p2Database.Usersettings.Gamedirectory + InstallDir[HasFiles]);
                }

                Log.Log.PrintMod("[MOD]Install File: " + InstallDir[HasFiles], Modloader.Installing.Modname);
                using (ZipArchive archive = ZipFile.OpenRead(Database.Database.p2Database.Usersettings.Gamedirectory + InstallDir[HasFiles] + ".zip"))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Database.Database.p2Database.Usersettings.Gamedirectory + InstallDir[HasFiles].Replace(".zip", String.Empty), true);
                    }
                }
                Log.Log.PrintMod("[MOD]Remove File: " + InstallDir[HasFiles] + ".zip", Modloader.Installing.Modname);
                if (File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + InstallDir[HasFiles] + ".zip"))
                {
                    File.Delete(Database.Database.p2Database.Usersettings.Gamedirectory + InstallDir[HasFiles] + ".zip");
                }
            }
            catch (Exception ex)
            {
                Log.Log.Error("[MOD]Failed install File: " + InstallDir[HasFiles] + " : " + ex.Message);
            }
        }

        /// <summary>
        /// Formats a byte count into a human-readable string (e.g., KB, MB).
        /// </summary>
        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }
            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

    }
}