
using Newtonsoft.Json;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Database.Data;
using ProjectParadise2.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
                        Log.Error("Failed Deserialize Database: " + ex.Message, ex);
                    }

                }
                else
                {
                    Write();
                    //New User?
                    var currentLanguage = CultureInfo.CurrentCulture.ToString().Split("-");
                    Log.Info("Current Language: " + currentLanguage[0] + " new user Install, Call Install File");
                    switch (currentLanguage[0])
                    {
                        case "de":
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_de);
                            break;
                        case "ru":
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_ru);
                            break;
                        case "es":
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_es);
                            break;
                        case "fr":
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_fr);
                            break;
                        case "pl":
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_pl);
                            break;
                        case "pt":
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_pg);
                            break;
                        default:
                            File.WriteAllBytes(Constans.DokumentsFolder + "/install.html", Resources.pp2_en);
                            break;
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
                Log.Error("Failed Write Database: " + ex.Message);
            }
        }
    }
}