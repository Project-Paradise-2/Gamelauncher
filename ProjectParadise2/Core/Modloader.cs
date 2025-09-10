using ProjectParadise2.Core.Log;
using ProjectParadise2.JsonClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace ProjectParadise2
{
    /// <summary>
    /// EventArgs class that represents the progress and status of an operation during the mod installation process.
    /// </summary>
    public class Call : EventArgs
    {
        /// <summary>
        /// The message associated with the current event.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The maximum value of the operation (used for progress tracking).
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// The current progress value of the operation.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Indicates whether the operation is finished or not.
        /// </summary>
        public bool Finished { get; set; }

        /// <summary>
        /// The speed of the operation (used for tracking download/upload speed).
        /// </summary>
        public string Speed { get; set; } = "";
    }

    /// <summary>
    /// Static class for generating events with progress information.
    /// </summary>
    class Event
    {
        /// <summary>
        /// Creates a new event with the specified parameters.
        /// </summary>
        /// <param name="Message">The message to be included in the event.</param>
        /// <param name="Max">The maximum value of the operation (default is 0).</param>
        /// <param name="Value">The current value of the operation (default is 0).</param>
        /// <param name="Finished">Indicates if the operation is finished (default is false).</param>
        /// <param name="Speed">The speed of the operation (default is an empty string).</param>
        /// <returns>A new instance of the Call class.</returns>
        public static Call GetEvent(string Message, int Max = 0, int Value = 0, bool Finished = false, string Speed = "")
            => new Call { Message = Message, Max = Max, Value = Value, Finished = Finished, Speed = Speed };
    }

    /// <summary>
    /// Manages the installation of mods, including checking directories, downloading missing files, and verifying the integrity of existing files.
    /// </summary>
    public static class Modloader
    {
        /// <summary>
        /// Stopwatch used to measure the time taken for the mod installation process.
        /// </summary>
        public static Stopwatch stopWatch;

        /// <summary>
        /// Indicates whether the mod installation process is currently running.
        /// </summary>
        public static bool IsRunning = false;

        /// <summary>
        /// The game mod currently being installed.
        /// </summary>
        public static GameMod Installing;

        /// <summary>
        /// List of file paths that are part of the mod installation.
        /// </summary>
        public static List<string> Files;

        /// <summary>
        /// Event handler for sending progress updates during the mod installation process.
        /// </summary>
        public static EventHandler Loaderevents;

        /// <summary>
        /// Sends a message with optional parameters: maximum value, current value, finished status, and speed.
        /// </summary>
        /// <param name="Message">The message to be sent.</param>
        /// <param name="Max">The maximum value (default is 0).</param>
        /// <param name="Value">The current value (default is 0).</param>
        /// <param name="Finished">Indicates whether the process is finished (default is false).</param>
        /// <param name="Speed">The speed of the process (default is an empty string).</param>
        internal static void SendMessage(string Message, int Max = 0, int Value = 0, bool Finished = false, string Speed = "")
        {
            Loaderevents?.Invoke(null, Event.GetEvent(Message, Max, Value, Finished, Speed));
        }

        /// <summary>
        /// Checks the directories and files required for the mod installation, creating any missing directories and adding missing files.
        /// </summary>
        public static void CheckDirs()
        {
            // Initializes mod installation if not already running
            if (Installing == null)
            {
                return;
            }

            // Creates required mod installation directories if they do not exist
            if (!Directory.Exists(Constans.DokumentsFolder + "Modloader/"))
            {
                Directory.CreateDirectory(Constans.DokumentsFolder + "Modloader/");
            }

            Log.PrintMod("[MOD] Start Installing from Mod", Installing.Modname);

            // Start the stopwatch to measure the installation time
            IsRunning = true;
            stopWatch = new Stopwatch();
            stopWatch.Start();
            Files = new List<string>();

            // Check and create missing directories
            foreach (var item in Installing.Directorys)
            {
                if (!Directory.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + "/" + item))
                {
                    Directory.CreateDirectory(Database.Database.p2Database.Usersettings.Gamedirectory + item);
                    Log.PrintMod("[MOD] Create Directory: " + Database.Database.p2Database.Usersettings.Gamedirectory + item, Installing.Modname);
                    SendMessage("Create missing path: " + item);
                }
            }

            // Check for missing files and add them to the list for download
            foreach (var item in Installing.File)
            {
                if (!File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + item.FilePath))
                {
                    FileLoader.AddFile(item.FileName, Installing.ModCreator + "/" + Installing.ProjectName + item.FilePath, item.FilePath);
                    Log.PrintMod("[MOD] Add File to Download: " + item.FileName, Installing.Modname);
                    FileLoader.NeedUpdate = true;
                    SendMessage(string.Format("Add file to reload: {0}", item.FileName));
                }
            }

            // Check for existing files and verify their integrity
            foreach (var item in Installing.File)
            {
                if (File.Exists(Database.Database.p2Database.Usersettings.Gamedirectory + item.FilePath))
                {
                    Files.Add(Database.Database.p2Database.Usersettings.Gamedirectory + item.FilePath);
                }
            }

            // Verify if existing files match the expected hash and size
            foreach (var remote in Installing.File)
            {
                foreach (var local in Files)
                {
                    if (local.Equals(Database.Database.p2Database.Usersettings.Gamedirectory + remote.FilePath))
                    {
                        var LocalMD5 = GetMD5(local);
                        if (!LocalMD5.Equals(remote.FileHash))
                        {
                            FileLoader.AddFile(remote.FileName, Installing.ModCreator + "/" + Installing.ProjectName + remote.FilePath, remote.FilePath);
                            SendMessage(string.Format("File changed: {0} reload needed", remote.FileName));
                            FileLoader.NeedUpdate = true;
                            Log.PrintMod("[MOD] File is Changed, Add to Redownload: " + remote.FileName, Installing.Modname);
                        }
                        else
                        {
                            var info = new System.IO.FileInfo(local);
                            if (info.Length == remote.FileSize)
                            {
                                SendMessage(string.Format("Files are ok: {0} no reload needed", remote.FileName));
                                Log.PrintMod("[MOD] File is not Changed: " + remote.FileName, Installing.Modname);
                            }
                            else
                            {
                                FileLoader.AddFile(remote.FileName, Installing.ModCreator + "/" + Installing.ProjectName + remote.FilePath, remote.FilePath);
                                SendMessage(string.Format("File size difference: {0} reload needed", remote.FileName));
                                Log.PrintMod("[MOD] File has Different Size: " + remote.FileName, Installing.Modname);
                                FileLoader.NeedUpdate = true;
                            }
                        }
                    }
                }
            }

            // If no files need to be updated, mark the installation as complete
            if (!FileLoader.NeedUpdate)
            {
                Modloader.SendMessage("Installation complete", 100, 100, true);
                Modloader.IsRunning = false;
            }
            // Prepare files for loading
            FileLoader.PrepareFileloading();
        }

        /// <summary>
        /// Computes the MD5 hash for a given file to verify its integrity.
        /// </summary>
        /// <param name="file">The file path for which the MD5 hash is computed.</param>
        /// <returns>The MD5 hash as a string.</returns>
        public static string GetMD5(string file)
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
    }
}