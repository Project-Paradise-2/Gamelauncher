
using ProjectParadise2.Core.Log;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ProjectParadise2
{
    /// <summary>
    /// Class to handle the creation and management of backup files.
    /// </summary>
    class Savebackup
    {
        /// <summary>
        /// Creates a backup of the specified source folder and stores it in the target folder.
        /// If the maximum number of backups is reached, the oldest backup is deleted.
        /// </summary>
        /// <param name="sourceFolder">The folder whose contents are to be backed up.</param>
        /// <param name="targetFolder">The folder where the backup will be stored.</param>
        /// <param name="maxBackups">The maximum number of backups allowed. Older backups will be deleted if this limit is reached.</param>
        public static void CreateBackup(string sourceFolder, string targetFolder, int maxBackups)
        {
            try
            {
                // Check if the source folder exists
                if (!Directory.Exists(sourceFolder))
                {
                    Log.Info("The local savegame backup folder does not exist.");
                    return;
                }

                // Create the target folder if it doesn't exist
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                // Get all existing backup files and sort them by creation time
                var existingBackups = Directory.GetFiles(targetFolder, "Backup_*.zip")
                                               .OrderBy(f => File.GetCreationTime(f))
                                               .ToList();

                // Find the highest backup number
                int maxBackupNumber = 0;
                foreach (var backup in existingBackups)
                {
                    // Extract the backup number from the filename (e.g. "Backup_01.zip" -> 1)
                    var fileName = Path.GetFileName(backup);
                    var match = System.Text.RegularExpressions.Regex.Match(fileName, @"Backup_(\d+).zip");
                    if (match.Success)
                    {
                        int backupNumber = int.Parse(match.Groups[1].Value);
                        maxBackupNumber = Math.Max(maxBackupNumber, backupNumber);
                    }
                }

                // If the maximum number of backups is reached, delete the oldest
                if (existingBackups.Count >= maxBackups)
                {
                    var oldestBackup = existingBackups.First();
                    File.Delete(oldestBackup);
                    Log.Warning($"Backup limit reached, deleting the oldest backup: {oldestBackup}");
                }

                // Create the next backup with the next available number
                int nextBackupNumber = maxBackupNumber + 1;
                string backupName = Path.Combine(targetFolder, $"Backup_{nextBackupNumber:D2}.zip");

                // Create the backup file
                ZipFile.CreateFromDirectory(sourceFolder, backupName);

                Log.Info($"Backup created: {backupName}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error($"Error: Unauthorized access. Please check permissions: " + ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                Log.Error($"Error: Directory not found: " + ex);
            }
            catch (IOException ex)
            {
                Log.Error($"I/O error occurred: " + ex);
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected error occurred: " + ex); ;
            }
        }

        /// <summary>
        /// Creates a backup using default parameters for source folder, target folder, and maximum number of backups.
        /// </summary>
        public static void CreateBackup()
        {
            try
            {
                CreateBackup(Constans.SaveFolder, Constans.DokumentsFolder + "Local_Backups", Database.Database.p2Database.Usersettings.NumOfBackups);
                Log.Info("Start Create Backup");
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected error occurred during the backup process: " + ex);
            }
        }
    }
}