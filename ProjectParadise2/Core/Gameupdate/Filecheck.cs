using Newtonsoft.Json;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectParadise2.Gameupdate
{
    internal class Filecheck
    {
        /// <summary>
        /// Holds the live update file index retrieved from the server.
        /// </summary>
        public static UpdateInfos Live = new UpdateInfos();

        /// <summary>
        /// Holds the local file index of the user's game directory.
        /// </summary>
        public static UpdateInfos Local = new UpdateInfos();

        private static readonly Stopwatch stopWatch = new Stopwatch();
        public static UpdateView view;
        public static string[] Files;
        public static string[] Directories;

        /// <summary>
        /// Starts the file-checking process, scanning local files and comparing them with the live server file index.
        /// </summary>
        public static async void StartCheck()
        {
            UpdateView.Instance.PrintMessage(Lang.GetText(76), 1);
            Thread.Sleep(650);
            if (!string.IsNullOrEmpty(Database.Database.p2Database.Usersettings.Gamedirectory))
            {

                bool isUnpacked = File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/Euro/Bnk/database/db_data.cpr");
                bool isUnpacked2 = Directory.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/Unknown_bin");
                if (isUnpacked && isUnpacked2)
                {
                    UpdateView.Instance.Updatestatus(Lang.GetText(77));
                    Log.Error("Unpacked game files detected.");
                    return;
                }
                if (isUnpacked || isUnpacked2)
                {
                    UpdateView.Instance.Updatestatus(Lang.GetText(77) + " mixed game?");
                    Log.Error("Both unpacked and packed game files detected.");
                    return;
                }

                if (!File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/bigfile_EU_1.big") || !File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/bigfile_EU_2.big") || !File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/bigfile_EU_3.big") || !File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/bigfile_EU_4.big"))
                {
                    UpdateView.Instance.Updatestatus(Lang.GetText(78));
                    Log.Error("Missing bigfiles in game directory (Default Files each Version).");
                    return;
                }

                stopWatch.Start();
                Files = Directory.GetFiles(Database.Database.p2Database.Usersettings.Gamedirectory + "/", "*.*", SearchOption.AllDirectories);
                Directories = Directory.GetDirectories(Database.Database.p2Database.Usersettings.Gamedirectory, "*", SearchOption.AllDirectories);
                Local.Directorys = Directory.GetFiles(Database.Database.p2Database.Usersettings.Gamedirectory + "/", "*.*", SearchOption.AllDirectories);

                UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(79), Files.Length, Directories.Length));

                var animationTask = PlayLoadingAnimationAsync(5, Lang.GetText(80));
                bool success = await Task.Run(() => GetOnlinefile());

                await animationTask;

                if (success)
                {
                    UpdateView.Instance.Updatestatus(Lang.GetText(81));
                    CompareFiles();
                }
                else
                {
                    UpdateView.Instance.Updatestatus(Lang.GetText(82));
                }
            }
        }

        /// <summary>
        /// Compares local files with the live file index to identify missing or outdated files.
        /// </summary>
        private static void CompareFiles()
        {
            List<string> localFiles = new List<string>();
            List<string> requiredFiles = new List<string>();

            Thread.Sleep(250);
            UpdateView.Instance.Updatestatus(Lang.GetText(83));

            foreach (var liveFile in Live.File)
            {
                requiredFiles.Add(liveFile.FilePath);
                Thread.Sleep(1);
            }

            foreach (var file in Files)
            {
                localFiles.Add(file.Replace(Database.Database.p2Database.Usersettings.Gamedirectory + "/", string.Empty).Replace(@"\", @"/"));
                Thread.Sleep(1);
            }

            List<string> missingFiles = CompareFiles(requiredFiles, localFiles);
            UpdateView.Instance.UpdateProgress(requiredFiles.Count, 0, Lang.GetText(84));
            UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(85), missingFiles.Count), 0);

            foreach (string file in missingFiles)
            {
                var info = GetInfo(file);
                GamefileLoader.AddFile(file, info[0], info[1]);
                UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(86), info[0], info[2]), 0);
                Log.Info($"Missing file detected: {info[0]} Size: {info[2]}");
                Thread.Sleep(5);
            }
            PerformHashCheck();
        }

        /// <summary>
        /// Performs a hash check to detect changes in files and identifies files needing updates.
        /// </summary>
        private static void PerformHashCheck()
        {
            UpdateView.Instance.PrintMessage($"", 0);
            UpdateView.Instance.PrintMessage(Lang.GetText(87), 5);
            UpdateView.Instance.PrintMessage($"", 0);
            UpdateView.Instance.Updatestatus(Lang.GetText(88));
            string CurrentFile = "";
            int totalFiles = Live.File.Count;
            for (int u = 0; u < totalFiles; u++)
            {
                for (int i = 0; i < Files.Length; i++)
                {
                    string relativePath = Files[i].Replace(Database.Database.p2Database.Usersettings.Gamedirectory + "/", string.Empty).Replace(@"\", @"/");

                    if (relativePath == Live.File[u].FilePath)
                    {
                        UpdateView.Instance.UpdateProgress(totalFiles, u + 1, Lang.GetText(89) + relativePath);
                        string hash = GetMD5(Files[i]);
                        CurrentFile = relativePath;
                        if (hash != Live.File[u].FileHash)
                        {
                            UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(90), relativePath, hash, Live.File[u].FileHash), 0);
                            GetUpdatedFile(u);
                        }
                        else
                        {
                            UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(91), relativePath), 1);
                        }

                        Thread.Sleep(10);
                    }
                }
            }

            UpdateView.Instance.PrintMessage($"", 0);
            stopWatch.Stop();

            TimeSpan elapsed = stopWatch.Elapsed;
            TimeSpan minimumDuration = TimeSpan.FromSeconds(5);

            if (elapsed < minimumDuration)
            {
                UpdateView.Instance.PrintMessage(Lang.GetText(92), 0);
            }
            else
            {
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

                UpdateView.Instance.PrintMessage(string.Format(Lang.GetText(93), formattedTime), 5);
                UpdateView.Instance.PrintMessage($"", 0);
                GamefileLoader.CreateDirs();
            }
        }

        /// <summary>
        /// Computes the MD5 hash of a file.
        /// </summary>
        private static string GetMD5(string file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        /// <summary>
        /// Retrieves a new version of the specified file from the server.
        /// </summary>
        private static void GetUpdatedFile(int index)
        {
            GamefileLoader.AddFile(Live.File[index].FilePath, Live.File[index].FileName, Live.File[index].FilePath);
            Log.Warning($"Outdated file detected: {Live.File[index].FileName} Size: {Live.File[index].FileSize}");
        }

        /// <summary>
        /// Retrieves file information from the live file index.
        /// </summary>
        public static string[] GetInfo(string filePath)
        {
            foreach (var data in Live.File)
            {
                if (data.FilePath == filePath)
                {
                    return new[] { data.FileName, data.FilePath, data.FileSize };
                }
            }
            return null;
        }

        /// <summary>
        /// Compares two lists of file paths to identify missing files.
        /// </summary>
        public static List<string> CompareFiles(List<string> required, List<string> existing)
        {
            return required.Where(file => !existing.Contains(file)).ToList();
        }

        /// <summary>
        /// Plays a loading animation in the UI while performing background tasks.
        /// </summary>
        public static async Task PlayLoadingAnimationAsync(int seconds, string updateStatus)
        {
            string[] animationFrames = { "-", "\\", "|", "/" };
            int frameIndex = 0;
            int duration = seconds * 1000;
            int elapsedTime = 0;
            int delay = 200;

            while (elapsedTime < duration)
            {
                UpdateView.Instance.Updatestatus($"{updateStatus} {animationFrames[frameIndex]}");
                frameIndex = (frameIndex + 1) % animationFrames.Length;
                await Task.Delay(delay);
                elapsedTime += delay;
            }
        }

        /// <summary>
        /// Downloads the live file index from the server.
        /// </summary>
        public static bool GetOnlinefile()
        {
            try
            {
                ///Linux workaround for ssl
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var client = new WebClient
                {
                    Headers = { ["User-Agent"] = "Launcher Updatefiles Request" },
                };

                string url = Constans.Cdn + "/update/Update.json";
                string content = client.DownloadString(url);
                Live = JsonConvert.DeserializeObject<UpdateInfos>(content);
                UpdateView.Instance.Updatestatus(Lang.GetText(95));
                return true;
            }
            catch (WebException ex)
            {
                UpdateView.Instance.PrintMessage($"Failed to connect to server: {ex.Message}\n");
                Log.Error("Failed to connect to server: ", ex);
                UpdateView.Instance.Updatestatus(Lang.GetText(94));
                return false;
            }
            catch (Exception ex)
            {
                UpdateView.Instance.PrintMessage($"An error occurred: {ex.Message}\n");
                Log.Error("An error occurred while retrieving the online file: ", ex);
                UpdateView.Instance.Updatestatus(Lang.GetText(94));
                return false;
            }
        }
    }
}