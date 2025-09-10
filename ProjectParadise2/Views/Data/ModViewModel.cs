using Newtonsoft.Json;
using ProjectParadise2.Core.Log;
using ProjectParadise2.JsonClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Event arguments used to pass messages for mod-related notifications.
    /// </summary>
    public class ModMessage : EventArgs
    {
        /// <summary>
        /// Gets or sets the message related to the mod.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        public int Type { get; set; } = 0;
    }

    /// <summary>
    /// ViewModel class responsible for managing mod data and interacting with the mod list API.
    /// </summary>
    internal class ModViewModel : ObservableObject
    {
        private static string ModQuerry = Constans.Cdn + $"/Requests/ModlistAPI.php";
        public static string ModPackQuerry = Constans.Modloader;
        public static Root LiveMods = new Root();
        public static int SelectedMod = 0;
        public static int CurrentPage = 0;
        public static int Totalmods = 0;
        private static int Build = 0;

        // Cached mods
        public static List<GameMod> Mods = new List<GameMod>();

        /// <summary>
        /// Fetches the list of mods by calling the necessary APIs.
        /// </summary>
        public static void GetList()
        {
            try
            {
                Mods.Clear();
                TimeSpan ts = TimeSpan.FromMilliseconds(5000);
                var task2 = Task.Run(() => RequestModcount(ModQuerry)).ContinueWith((t) =>
                {
                    if (t.IsFaulted)
                    {
                        Log.Error("[DISCOVERY] Task(RequestModcount)::IsFaulted");
                    }
                    if (t.IsCompleted)
                    {
                        Debug.WriteLine("Task(RequestModcount)::IsCompleted");
                    }
                    if (t.IsCompletedSuccessfully)
                    {
                        Debug.WriteLine("Task(RequestModcount)::IsCompletedSuccessfully");
                    }
                });

                var task = Task.Run(() => RequestModData(ModQuerry)).ContinueWith((t) =>
                {
                    if (t.IsFaulted)
                    {
                        Log.Error("[DISCOVERY] Task(RequestModData)::IsFaulted");
                    }
                    if (t.IsCompleted)
                    {
                        Debug.WriteLine("Task(RequestModData)::IsCompleted");
                    }
                    if (t.IsCompletedSuccessfully)
                    {
                        Debug.WriteLine("Task(RequestModData)::IsCompletedSuccessfully");
                    }
                });

                task.Wait(ts);
            }
            catch (Exception ex)
            {
                Log.Error("[DISCOVERY] Failed to handle Initial Tasks: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Requests the mod count from the API.
        /// </summary>
        /// <param name="modQuerry">The URL for the mod query request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task RequestModcount(string modQuerry)
        {
            try
            {
                WebClient Detail = new WebClient();
                Detail.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnModCountdone);

                Build = Database.Database.p2Database.Usersettings.Packedgame ? 1 : 0;

                Detail.DownloadStringAsync(new Uri(modQuerry + $"?g={Build}&c=1"));
            }
            catch (Exception ex)
            {
                Log.Error("[DISCOVERY] Error in RequestModcount: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Event handler called when the mod count request is completed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments containing the result of the operation.</param>
        private static void OnModCountdone(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Totalmods = int.Parse(e.Result.ToString());
            }
            ModView.Instance.OnNotifymessage("Refreshing page...", 2);
        }

        /// <summary>
        /// Requests mod data from the API based on the current page and build version.
        /// </summary>
        /// <param name="modQuerry">The URL for the mod data request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task RequestModData(string modQuerry)
        {
            try
            {
                Mods.Clear();
                LiveMods.data.Clear();

                WebClient Detail = new WebClient();
                Detail.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnModlistdone);

                Build = Database.Database.p2Database.Usersettings.Packedgame ? 1 : 0;

                Detail.DownloadStringAsync(new Uri(modQuerry + $"?p={CurrentPage}&g={Build}"));
            }
            catch (Exception ex)
            {
                Log.Error("[DISCOVERY] Error in RequestModData: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Event handler called when the mod data request is completed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="task">The event arguments containing the result of the operation.</param>
        private static void OnModlistdone(object sender, DownloadStringCompletedEventArgs task)
        {
            try
            {
                var result = task.Result;
                if (task.Result != null)
                {
                    if (result != "NoMods")
                    {
                        LiveMods = JsonConvert.DeserializeObject<Root>(result);

                        foreach (var item in LiveMods.data)
                        {
                            Task.Run(() => GetModinfo(item.Publisher, item.ModPath)).ContinueWith((t) =>
                            {
                                if (t.IsFaulted)
                                {
                                    Log.Error("[DISCOVERY]Task(RequestModData)::IsFaulted");
                                }
                                if (t.IsCompleted)
                                {
                                    Debug.WriteLine("Task(RequestModData)::IsCompleted");
                                }
                                if (t.IsCompletedSuccessfully)
                                {
                                    Debug.WriteLine("Task(RequestModData)::IsCompletedSuccessfully");
                                }
                            });
                            Thread.Sleep(50);
                        }
                    }
                    else
                    {
                        ModView.Instance.OnNotifymessage("No User Mods Available", 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed get Modlist: " + ex.Message + $" Build: {Build} Currentpage: {CurrentPage} Selectedmod: {SelectedMod} Totalmods: {Totalmods}", ex);
            }
        }

        /// <summary>
        /// Retrieves information about a specific mod.
        /// </summary>
        /// <param name="Creator">The mod's creator.</param>
        /// <param name="Workingpath">The path to the mod's data.</param>
        private static void GetModinfo(string Creator, string Workingpath)
        {
            WebClient Detail = new WebClient();
            Detail.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnModRequestdone);
            Detail.DownloadStringAsync(new Uri(ModPackQuerry + $"/{Creator}/{Workingpath}/Modpack.json"));
        }

        /// <summary>
        /// Event handler called when the mod information request is completed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="result">The event arguments containing the result of the operation.</param>
        private static void OnModRequestdone(object sender, DownloadStringCompletedEventArgs result)
        {
            try
            {
                MainWindow.DoWork(true);
                if (result.Result != null)
                {
                    GameMod mod = new GameMod();
                    mod = JsonConvert.DeserializeObject<GameMod>(result.Result);
                    Log.Info("Deserialize Modinfo: " + mod.Modname + " v.: " + mod.Modversion);
                    Mods.Add(mod);

                    ModView.Instance.OnNotifymessage((Mods.Count - 1).ToString(), 3);

                    if (LiveMods.data.Count == 1)
                    {
                        ModView.Instance.OnNotifymessage(string.Format(Lang.GetText(74), Mods.Count, Totalmods), 1);
                    }
                    else
                    {
                        ModView.Instance.OnNotifymessage(string.Format(Lang.GetText(74), (Mods.Count + (CurrentPage * 10)), (Totalmods)), 1);
                    }
                }
            }
            catch (WebException ex)
            {
                Log.Error("[DISCOVERY] Failed get mod from Remote=> " + ex.Message.ToString(), ex);
            }
            catch (Exception ex)
            {
                Log.Error("[DISCOVERY] Failed get mod from Remote=> " + ex.Message.ToString(), ex);
            }
            MainWindow.DoWork();
        }

        /// <summary>
        /// Counts the number of downloads for a specific mod project.
        /// </summary>
        /// <param name="ProjectName">The name of the project to count downloads for.</param>
        internal static void CountDownload(string ProjectName)
        {
            WebClient Detail = new WebClient();
            Detail.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OnModinstalledcount);
            Detail.DownloadStringAsync(new Uri(ModQuerry + $"?m={ProjectName}&c=2"));
        }

        /// <summary>
        /// Event handler called when the mod installed count request is completed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments containing the result of the operation.</param>
        private static void OnModinstalledcount(object sender, DownloadStringCompletedEventArgs e)
        {
            // Implementation for handling the completion of mod installed count request can go here.
        }
    }
}
