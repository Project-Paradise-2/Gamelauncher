using Newtonsoft.Json;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Database.Data;
using ProjectParadise2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace ProjectParadise2
{
    [System.Serializable]
    public enum Build
    {
        Packed,
        Unpacked
    }

    [System.Serializable]
    public enum Gametype
    {
        none,
        TDU1,
        TDU2,
        TDU2Dev
    }

    [System.Serializable]
    public class GameProfile
    {
        public Build Build { get; set; } = Build.Packed;
        public Gametype Gametype { get; set; } = Gametype.none;
        public string Profilename { get; set; }
        public string Gamepath { get; set; }
        public string Basedir { get; set; }
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public string ProductVersion { get; set; }
        public string FileVersion { get; set; }
        public string Version { get; set; }
        public string InternalName { get; set; }
        public bool IsDebug { get; set; } = false;
        public bool SteamBuild { get; set; } = false;
        public bool ModBrowserAktive { get; set; } = false;
        public bool LAAEnabled { get; set; } = false;
        public bool HighPrio { get; set; } = false;
        public bool UseMoreCores { get; set; } = false;
        public bool VehicleDirt { get; set; } = true;
        public bool VehicleDamage { get; set; } = true;
        public bool OnlineMode { get; set; } = true;
        public static string[] Runtype = { "44938b8f", "957e4cc3" }; // On/Off TDU2 Only..

        /// <summary>
        /// A list of user-installed mods, each represented by a <see cref="Gamemod"/> object.
        /// </summary>
        public List<Gamemod> Usermods = new List<Gamemod>();

        public GameProfile() { }

        public GameProfile(string profilename, string gamepath, string arguments = "")
        {
            Profilename = profilename;
            Gamepath = gamepath;
            Arguments = arguments;

            if (string.IsNullOrEmpty(profilename) || string.IsNullOrEmpty(gamepath))
            {
                throw new ArgumentException("Profilename and Executable cannot be null or empty.");
            }

            if (File.Exists(gamepath))
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(gamepath);
                ProductVersion = fvi.ProductVersion.ToString();
                FileVersion = fvi.FileVersion.ToString();
                Version = fvi.ProductVersion.ToString();
                InternalName = fvi.InternalName.ToString();
                IsDebug = fvi.IsDebug;

                if (gamepath.EndsWith("TestDriveUnlimited.exe"))
                {
                    Gametype = Gametype.TDU1;
                    Basedir = gamepath.Replace(@"\TestDriveUnlimited.exe", "");
                    SteamBuild = false;
                    ModBrowserAktive = false;
                    Executable = "TestDrive.exe";
                }
                else if (gamepath.EndsWith("TestDrive2.exe"))
                {
                    Gametype = Gametype.TDU2;
                    Executable = "TestDrive2.exe";
                }
                else if (gamepath.EndsWith("TestDrive2Dev.exe"))
                {
                    Gametype = Gametype.TDU2Dev;
                    Executable = "TestDrive2Dev.exe";
                }
                else
                {
                    Gametype = Gametype.none;
                    Executable = Path.GetFileName(gamepath);
                }

                if (Gametype == Gametype.TDU2)
                {
                    Basedir = gamepath.Replace(@"\TestDrive2.exe", "").Replace(@"\TestDriveUnlimited.exe", "");
                    bool isSteam = File.Exists(Basedir + @"\steam_api.dll");

                    if (isSteam)
                    {
                        SteamBuild = true;
                        string message = Lang.GetText(111);
                        var result = MessageBox.Show(message, "Project Paradise 2 - Test Drive Unlimited Game Selector", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.No)
                        {
                            isSteam = false;
                            SteamBuild = false;
                            File.Move(Basedir + @"\steam_api.dll", Basedir + @"\steam_api.dll.backup");
                        }
                    }

                    bool unpackedDetected =
                        File.Exists(Path.Combine(Basedir, "Euro", "Bnk", "database", "db_data.cpr")) ||
                        Directory.Exists(Path.Combine(Basedir, "Unknown_bin")) ||
                        File.Exists(Path.Combine(Basedir, "TDU2_Unpacked_Uninstall.exe"));

                    if (unpackedDetected)
                    {
                        MessageBoxResult state = MessageBox.Show(
                            Lang.GetText(112),
                            "Project Paradise 2 - Test Drive Unlimited 2 Game Selector",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (state == MessageBoxResult.Yes)
                        {
                            Database.Database.p2Database.Usersettings.Packedgame = false;
                            ModBrowserAktive = false;
                            Build = Build.Unpacked;
                            MessageBoxResult warning = MessageBox.Show(
                                    Lang.GetText(113),
                                    "Project Paradise 2 - Test Drive Unlimited 2 Game Selector",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                        }
                        else
                        {
                            Database.Database.p2Database.Usersettings.Packedgame = true;
                            ModBrowserAktive = true;

                            MessageBoxResult warning = MessageBox.Show(
                                    Lang.GetText(114),
                                    "Project Paradise 2 - Test Drive Unlimited 2 Game Selector",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                        }
                    }
                }
            }

            Log.Info($"Launcher Gameprofile Created ({Profilename}) Game: {Gametype}-{Build} Gamepath: {Basedir} Version: {ProductVersion} Version: {Version} Debug: {IsDebug}");
        }

        public string GetMutex()
        {
            string runParam = "";

            if (Gametype == Gametype.TDU2 || Gametype == Gametype.TDU2Dev)
            {
                if (OnlineMode == true)
                {
                    runParam = Runtype[0];
                }
                else
                {
                    runParam = Runtype[1];
                }
            }
            return runParam;
        }

        public string GetLaunchArguments()
        {
            string runParam = "";
            runParam = " " + Arguments;
            if (HighPrio)
                runParam += " -high";

            if (UseMoreCores)
                runParam += " -USEALLAVAILABLECORES";


            return runParam;
        }

        /// <summary>
        /// Sets or unsets the Large Address Aware (LAA) flag in the specified executable file.
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool SetLargeAddressAware(string exePath, bool enable)
        {
            if (!File.Exists(exePath))
            {
                Log.Error("Error: File not found!");
                return false;
            }

            try
            {
                using (FileStream fs = new FileStream(exePath, FileMode.Open, FileAccess.ReadWrite))
                using (BinaryReader br = new BinaryReader(fs))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    fs.Seek(0x3C, SeekOrigin.Begin);
                    int peHeaderOffset = br.ReadInt32();

                    fs.Seek(peHeaderOffset + 0x18, SeekOrigin.Begin);
                    short magic = br.ReadInt16();

                    if (magic != 0x10B && magic != 0x20B)  // Check if it's a valid PE file (32-bit or 64-bit)
                    {
                        Log.Error("Error: Not a valid PE file.");
                        return false;
                    }

                    fs.Seek(peHeaderOffset + 0x16, SeekOrigin.Begin);
                    ushort characteristics = br.ReadUInt16();

                    if (enable)
                    {
                        characteristics |= 0x0020;  // Enable LAA flag
                    }
                    else
                    {
                        characteristics &= unchecked((ushort)~0x0020);  // Disable LAA flag
                    }

                    fs.Seek(peHeaderOffset + 0x16, SeekOrigin.Begin);
                    bw.Write(characteristics);
                }

                Log.Info("LAA flag has been successfully " + (enable ? "enabled" : "disabled") + "!");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error modifying EXE: " + ex.Message);
                return false;
            }
        }

        public void UpdateRegestry()
        {
            if (Gametype == Gametype.TDU1)
                return;

            try
            {

                while (Regestry.UpdateKey(SteamBuild)) break;
                Regestry.UpdatePath(Basedir + @"\TestDrive2.exe", Basedir, System.Environment.CurrentDirectory + @"\Uplauncher.exe", SteamBuild);

                var currentLanguage = CultureInfo.CurrentCulture;
                var parsed = currentLanguage.Name.Split("-");
                Regestry.UpdateKey("AudioLib", "DirectSound", SteamBuild);
                Regestry.UpdateKey("GameProductVersion", ProductVersion, SteamBuild);
                Regestry.UpdateKey("languagePack", Utils.GetRegionFromCountryCode(new RegionInfo(currentLanguage.Name).TwoLetterISORegionName), SteamBuild);
                Regestry.UpdateKey("language", parsed[0], SteamBuild);
                Regestry.UpdateKey("Project-Paradise2", Constans.LauncherVersion, SteamBuild);
                if (File.Exists(Basedir + @"\key.txt"))
                {
                    string serial = File.ReadAllText(Basedir + @"\key.txt");
                    Regestry.UpdateKey("Serial", serial, SteamBuild);
                }
                Regestry.UpdateKey("NetworkNatType", ProjectParadise2.Core.BackgroundWorker.MyNatType, SteamBuild);
            }
            catch (Exception ex)
            {
                Log.Error("Error updating registry: " + ex.Message + " Profile: " + Profilename);
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
        public void AddMod(int ModId, string Name, string Version, int ModType, List<ProjectParadise2.JsonClasses.FileInfo> Files, string ProjectName)
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

            for (int i = 0; i < Usermods.Count; i++)
            {
                if (Usermods[i].Name == mod.Name)
                {
                    Usermods[i].Version = Version;
                    Usermods[i].Files = Files;
                    return;
                }
            }
            ModViewModel.CountDownload(ProjectName);
            Usermods.Add(mod);
            Update();
        }

        /// <summary>
        /// Checks if a mod is already installed by its project name.
        /// </summary>
        /// <param name="ProjectName">The project name of the mod.</param>
        /// <returns>True if the mod is installed, false otherwise.</returns>
        public bool isModInstalled(string ProjectName)
        {
            if (Usermods == null) return false;
            return Usermods.Where(x => x.Name == ProjectName).FirstOrDefault() != null;
        }

        /// <summary>
        /// Checks if the specified version of a mod is installed.
        /// </summary>
        /// <param name="ProjectName">The project name of the mod.</param>
        /// <param name="Version">The version of the mod.</param>
        /// <returns>True if the specified version of the mod is installed, false otherwise.</returns>
        public bool GetInstalledversion(string ProjectName, string Version)
        {
            return Usermods.Where(x => x.Name == ProjectName).FirstOrDefault().Version == Version;
        }

        public bool EditExe(long offset, bool Value, string Filepath)
        {
            byte data = 0;

            if (Value)
            {
                data = 0x44;
            }
            else
            {
                data = 0x42;
            }

            try
            {
                string filePath = Filepath;

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                    fs.WriteByte(data);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Failed set: 0x" + offset.ToString("X") + " reason: " + ex.Message);
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);

                if (File.Exists(Constans.DokumentsFolder + "/GameProfiles/" + Profilename + ".profile"))
                {
                    File.Delete(Constans.DokumentsFolder + "/GameProfiles/" + Profilename + ".profile");
                }

                File.WriteAllText(Constans.DokumentsFolder + "/GameProfiles/" + Profilename + ".profile", jsonData);

                return true;
            }
            catch (JsonException ex)
            {
                Log.Error(ex);
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }
    }
}