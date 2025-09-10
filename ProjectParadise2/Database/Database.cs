using KuxiiSoft.Utils.Crashreport;
using Newtonsoft.Json;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Database.Data;
using ProjectParadise2.Properties;
using ProjectParadise2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ProjectParadise2.Database
{
    /// <summary>
    /// Handles database operations such as reading and writing data related to Project Paradise 2.
    /// </summary>
    class Database
    {
        /// <summary>
        /// An instance of the main database object.
        /// </summary>
        public static PP2Database p2Database = new PP2Database();
        /// <summary>
        /// Flag indicating if the database has been loaded successfully.
        /// </summary>
        public static bool IsLoadet { get; set; } = false;

        /// <summary>
        /// Reads the database from the saved location or initializes a new one if it doesn't exist.
        /// </summary>
        public static void Read()
        {
            string db = "";
            db = p2Database.DatabaseVersion;
            try
            {
                var sysp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var fold = "PP2";

                var savedir = (Path.Combine(sysp, fold));
                if (!Directory.Exists(savedir))
                    Directory.CreateDirectory(savedir);

                if (File.Exists(Constans.DokumentsFolder + "/dbData.db"))
                {
                    try
                    {
                        string json = File.ReadAllText(Constans.DokumentsFolder + "/dbData.db");
                        p2Database = JsonConvert.DeserializeObject<PP2Database>(json);
                    }
                    catch (JsonException ex)
                    {
                        Report.GenerateJson(ex, "Failed Deserialize Launcher Database");
                        Log.Error("Failed Deserialize Database: " + ex.Message, ex);
                    }
                }
                else
                {
                    Write();
                    //New User?
                    var currentLanguage = CultureInfo.CurrentCulture.ToString().Split("-");
                    if (currentLanguage[0] == "de")
                    {
                        File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_de);
                    }
                    if (currentLanguage[0] == "ru")
                    {
                        File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_ru);
                    }
                    else
                    {
                        File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_en);
                    }

                    if (File.Exists(Constans.DokumentsFolder + "/install.html"))
                    {
                        string url = Constans.DokumentsFolder + "/install.html";
                        try
                        {
                            Process.Start(url);
                        }
                        catch
                        {
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            {
                                url = url.Replace("&", "^&");
                                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            {
                                Process.Start("xdg-open", url);
                            }
                            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            {
                                Process.Start("open", url);
                            }
                            else
                            {
                                Log.Error("Failed open the Browser");
                            }
                        }
                    }
                }
                IsLoadet = true;
            }
            catch (Exception ex)
            {
                Report.Generate(ex, "Failed Load Launcher Database");
                Log.Error("Failed Open Database: " + ex.Message, ex);
            }
            Log.Info("Database Loadet Version: " + p2Database.DatabaseVersion + " launcher used: " + db);
            p2Database.Usersettings.Launcherverion = Constans.LauncherVersion;
        }

        /// <summary>
        /// Writes the current database state to a file.
        /// </summary>
        public static void Write()
        {
            try
            {
                if (!Directory.Exists(Constans.DokumentsFolder))
                    Directory.CreateDirectory(Constans.DokumentsFolder);
                string jsonData = JsonConvert.SerializeObject(p2Database, Formatting.Indented);
                File.WriteAllText(Constans.DokumentsFolder + "/dbData.db", jsonData);
            }
            catch (Exception ex)
            {
                Report.Generate(ex, "Failed Write Launcher Database");
                Log.Error("Failed Write Database: " + ex.Message);
            }
        }

        /// <summary>
        /// Adds a mod to the database, updating its version if it already exists.
        /// </summary>
        /// <param name="ModId">The ID of the mod.</param>
        /// <param name="Name">The name of the mod.</param>
        /// <param name="Version">The version of the mod.</param>
        /// <param name="ModType">The type of the mod (e.g., packed or not).</param>
        /// <param name="Files">A list of files associated with the mod.</param>
        /// <param name="ProjectName">The project name associated with the mod.</param>
        public static void AddMod(int ModId, string Name, string Version, int ModType, List<ProjectParadise2.JsonClasses.FileInfo> Files, string ProjectName)
        {
            bool packed = false;

            if (ModType == 1)
            {
                packed = true;
            }

            Gamemod mod = new Gamemod()
            {
                ModId = ModId,
                Name = Name,
                Version = Version,
                Installed = DateTime.Now.ToString("HH:mm dd.MM.yyyy"),
                Packed = packed,
                Files = Files,
            };

            for (int i = 0; i < p2Database.Usermods.Count; i++)
            {
                if (p2Database.Usermods[i].Name == mod.Name)
                {
                    p2Database.Usermods[i].Version = Version;
                    p2Database.Usermods[i].Files = Files;
                    return;
                }
            }
            ModViewModel.CountDownload(ProjectName);
            p2Database.Usermods.Add(mod);
            Write();
        }


        /// <summary>
        /// Checks if a mod is already installed by its project name.
        /// </summary>
        /// <param name="ProjectName">The project name of the mod.</param>
        /// <returns>True if the mod is installed, false otherwise.</returns>
        public static bool isModInstalled(string ProjectName)
        {
            return p2Database.Usermods.Where(x => x.Name == ProjectName).FirstOrDefault() != null;
        }

        /// <summary>
        /// Checks if the specified version of a mod is installed.
        /// </summary>
        /// <param name="ProjectName">The project name of the mod.</param>
        /// <param name="Version">The version of the mod.</param>
        /// <returns>True if the specified version of the mod is installed, false otherwise.</returns>
        public static bool GetInstalledversion(string ProjectName, string Version)
        {
            return p2Database.Usermods.Where(x => x.Name == ProjectName).FirstOrDefault().Version == Version;
        }
    }
}