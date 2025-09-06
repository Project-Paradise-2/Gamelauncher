using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaction logic for ModDetail.xaml
    /// </summary>
    public partial class ModDetail : UserControl
    {
        private int ModId = 0;
        private string AllUpdates = "0";
        private string AllDownloads = "0";

        /// <summary>
        /// Initializes a new instance of the <see cref="ModDetail"/> class.
        /// Sets up the UI and loads the selected mod details.
        /// </summary>
        public ModDetail()
        {
            InitializeComponent();
            BackgroundWorker.OnLangset += SetLang;
            SetLang();
            Modloader.Loaderevents += OnEvent;
            InstallingPanel.IsEnabled = false;
            InstallingPanel.Visibility = Utils.GetVisibility(InstallingPanel.IsEnabled);
            CheckModloader();
            ModId = ModViewModel.SelectedMod;

            foreach (var item in ModViewModel.LiveMods.data)
            {
                if (item.ModPath == ModViewModel.Mods[ModId].ProjectName)
                {
                    AllUpdates = item.Uploads;
                    AllDownloads = item.Downloads;
                }
            }

            ModImage.Source = Utils.LoadImage(ModViewModel.Mods[ModId].Modimage);
            Creator.Text += ": " + ModViewModel.Mods[ModId].ModCreator;
            ModName.Text += ": " + ModViewModel.Mods[ModId].Modname;

            Description.Inlines.Clear();

            string[] link = ModViewModel.Mods[ModId].Moddescription.Split(" ");
            Hyperlink hlink = null;

            for (int i = 0; i < link.Length; i++)
            {
                if (link[i].Contains("https"))
                {
                    hlink = new Hyperlink
                    {
                        NavigateUri = new Uri(link[i].Replace(" ", ""))
                    };
                    hlink.Inlines.Add("See More..");
                    hlink.RequestNavigate += OpenBrowser;
                    ModViewModel.Mods[ModId].Moddescription = ModViewModel.Mods[ModId].Moddescription.Replace(link[i], "");
                }
            }
            Description.Text += ": \n" + ModViewModel.Mods[ModId].Moddescription;
            if (hlink != null)
            {
                Description.Inlines.Add(hlink);
            }

            Version.Text += ": " + ModViewModel.Mods[ModId].Modversion;
            Changedfiles.Text += ": " + ModViewModel.Mods[ModId].File.Count.ToString();
            Downloadsize.Text += ": " + Utils.FormatBytes(ModViewModel.Mods[ModId].Downloadsize);
            Installsize.Text += ": " + Utils.FormatBytes(ModViewModel.Mods[ModId].Discsize);
            Updates.Text += ": " + AllUpdates + "x";
            Downloads.Text += ": " + AllDownloads + "x";
        }

        /// <summary>
        /// Handles mod loader events, updating the UI based on installation progress or completion.
        /// </summary>
        private void OnEvent(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(OnEvent), sender, e);
                return;
            }
            Call a = (Call)e;
            if (a.Finished)
            {
                Installing.Text = "Installation of mod" + ": " + Modloader.Installing.Modname + " finished.";
                TotalProgress.Value = 100;
                Status.Visibility = Visibility.Hidden;
                TotalProgress.Visibility = Visibility.Hidden;
                ProgressText.Visibility = Visibility.Hidden;
            }
            else
            {
                ProgressText.Visibility = Visibility.Visible;
                Status.Visibility = Visibility.Visible;
                TotalProgress.Visibility = Visibility.Visible;
                Installing.Text = "Installing mod" + ": " + Modloader.Installing.Modname + $" File {FileLoader.HasFiles + 1} of {FileLoader.NeedFiles}";
                if (!a.Speed.Contains("-"))
                {
                    Status.Text = a.Message + " at ↓" + a.Speed;
                }
                else
                {
                    Status.Text = a.Message;
                }
                TotalProgress.Maximum = a.Max;
                TotalProgress.Value = FileLoader.PercentLoad;
            }
        }

        /// <summary>
        /// Opens the mod list view.
        /// </summary>
        private void OpenModlist(object sender, RoutedEventArgs e)
        {
            MainViewModel.OpenModbrowser();
        }

        /// <summary>
        /// Opens a web browser to navigate to the specified URL.
        /// </summary>
        private void OpenBrowser(object sender, RequestNavigateEventArgs e)
        {
            string url = e.Uri.ToString();
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

        /// <summary>
        /// Checks if the mod loader is running and updates the UI accordingly.
        /// </summary>
        private void CheckModloader()
        {
            if (Modloader.IsRunning)
            {
                Installing.Text = "Start mod download";
                InstallingPanel.IsEnabled = true;
                InstallingPanel.Visibility = Utils.GetVisibility(InstallingPanel.IsEnabled);
            }
        }

        /// <summary>
        /// Starts the installation or update process for the selected mod.
        /// </summary>
        private void InstallOrUpdate(object sender, RoutedEventArgs e)
        {
            Thread install = new Thread(InstallMod)
            {
                IsBackground = true
            };
            install.Start();
        }

        /// <summary>
        /// Executes the mod installation process on the main thread if necessary.
        /// </summary>
        private void InstallMod()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(InstallMod));
                return;
            }

            if (Modloader.IsRunning)
            {
                return;
            }

            Modloader.Installing = ModViewModel.Mods[ModId];
            Installing.Text = "Start mod download";
            InstallingPanel.IsEnabled = true;
            InstallingPanel.Visibility = Utils.GetVisibility(InstallingPanel.IsEnabled);

            Database.Database.AddMod(ModId, ModViewModel.Mods[ModId].Modname, ModViewModel.Mods[ModId].Modversion, ModViewModel.Mods[ModId].Modtype, ModViewModel.Mods[ModId].File, ModViewModel.Mods[ModId].ProjectName);
            var task2 = Task.Run(() => Modloader.CheckDirs()).ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    Log.Error("[Discovery] Task(Modloader.CheckDirs)::IsFaulted");
                }
                if (t.IsCompleted)
                {
                    Debug.WriteLine("Task(Modloader.CheckDirs)::IsCompleted");
                }
                if (t.IsCompletedSuccessfully)
                {
                    Debug.WriteLine("Task(Modloader.CheckDirs)::IsCompletedSuccessfully");
                }
            });
        }

        private void SetLang(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action<object, EventArgs>(SetLang), sender, e);
                return;
            }
            BackBtn.Content = Lang.GetText(9);
            Creator.Text = Lang.GetText(10);
            Description.Text = Lang.GetText(11);
            Version.Text = Lang.GetText(14);
            Changedfiles.Text = Lang.GetText(15);
            Downloadsize.Text = Lang.GetText(16);
            Installsize.Text = Lang.GetText(17);
            Updates.Text = Lang.GetText(12);
            Downloads.Text = Lang.GetText(13);
            InstallUpdate.Content = Lang.GetText(19);
            ModName.Text = Lang.GetText(39);
        }

        private void SetLang()
        {
            BackBtn.Content = Lang.GetText(9);
            Creator.Text = Lang.GetText(10);
            Description.Text = Lang.GetText(11);
            Version.Text = Lang.GetText(14);
            Changedfiles.Text = Lang.GetText(15);
            Downloadsize.Text = Lang.GetText(16);
            Installsize.Text = Lang.GetText(17);
            Updates.Text = Lang.GetText(12);
            Downloads.Text = Lang.GetText(13);
            InstallUpdate.Content = Lang.GetText(19);
            ModName.Text = Lang.GetText(39);
        }
    }
}