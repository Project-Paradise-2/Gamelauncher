using HardwareInformation;
using Microsoft.Win32;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using ProjectParadise2.Core.UPnP;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// Interaction logic for InfoView.xaml
    /// </summary>
    public partial class InfoView : UserControl
    {
        /// <summary>
        /// Registry path for DirectX information.
        /// </summary>
        internal const string DirectX = @"SOFTWARE\\Microsoft\\DirectX";

        /// <summary>
        /// Instance of the InfoView class.
        /// </summary>
        public static InfoView Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoView"/> class.
        /// Sets up initial data display.
        /// </summary>
        public InfoView()
        {
            InitializeComponent();
            Instance = this;
            Refresh();
        }

        /// <summary>
        /// Refreshes the information displayed in the view.
        /// Updates DirectX status, launcher version, redistributable packages, port state, and build time.
        /// </summary>
        public void Refresh()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(Refresh));
                return;
            }

            try
            {
                var hardware = MachineInformationGatherer.GatherInformation(true);
                RegistryKey dxVersion = Registry.LocalMachine.OpenSubKey(DirectX, true);
                if (dxVersion != null)
                {
                    if (dxVersion.GetValue("Version").ToString() == "4.09.00.0904")
                        this.DirectXText.Text = "Installed (4.09.00.0904)";
                    else
                        this.DirectXText.Text = "Not-Installed";
                }

                NatTypeText.ToolTip = Lang.GetTooltipText(24);
                PortTypeText.ToolTip = Lang.GetTooltipText(25);

                this.Launcherversion.Text = Constans.LauncherVersion.ToString();
                this.NatTypeText.Text = BackgroundWorker.MyNatType;

                bool isVC2005Installed = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x86);
                bool isVC2008Installed = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x86);

                bool isVC2005InstalledX = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x64);
                bool isVC2008InstalledX = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x64);

                this.CPlusText.Text = $"" +
                    $"VC2005* (x86): {ParseBool(isVC2005Installed)}\n" +
                    $"VC2008  (x86): {ParseBool(isVC2008Installed)}\n";

                var state = UPnP.IsOpen(Protocol.UDP, 8889);
                if (state)
                    this.PortTypeText.Text = $"The port 8889 has been successfully opened.";
                else
                    this.PortTypeText.Text = $"Failed to open port 8889.Possible: UPnP might be disabled on your router. Port 8889 may already be forwarded manually.";


                this.Launcherbuild.Text = new FileInfo(Assembly.GetExecutingAssembly().Location).CreationTime.ToString();
                try
                {
                    this.HardwareText.Text = "Once you’ve opened the page, press CTRL + V here in the chat to paste your system information. This will help us assist you more quickly.\nHardwareinfos:";
                    this.HardwareText.Text += GenerateInfo(hardware);
                }
                catch (Exception e)
                {
                    Log.Print("Unable to read Hardware Info: ", e);
                }
            }
            catch (Exception ex)
            {
                Log.Print("Failed to initialize InfoView: ", ex);
            }
        }

        private string GenerateInfo(MachineInformation hardware)
        {
            StringBuilder View = new StringBuilder();
            StringBuilder File = new StringBuilder();

            File.AppendLine($"----------------------- Hardware Report -----------------------------\n");
            // Betriebssystem
            File.AppendLine($"OS: {hardware.OperatingSystem.VersionString}");
            File.AppendLine($"Platform: {hardware.Platform}");

            // CPU
            File.AppendLine($"\nCPU: {hardware.Cpu.Name} {hardware.Cpu.Socket}");
            File.AppendLine($"- Logical Cores: {hardware.Cpu.LogicalCores}");
            File.AppendLine($"- Physical Cores: {hardware.Cpu.PhysicalCores}");
            File.AppendLine($"- Speed: {hardware.Cpu.NormalClockSpeed} MHz");

            // GPUs
            File.AppendLine("\nGPUs:");
            foreach (var gpu in hardware.Gpus)
            {
                File.AppendLine($"- Name: {gpu.Name} Device: {gpu.Type.ToString()}");
                File.AppendLine($"  - VRAM: {gpu.AvailableVideoMemoryHRF}");
                File.AppendLine($"  - Driverversion: {gpu.DriverVersion}");
                File.AppendLine($"  - Driverdate: {gpu.DriverDate}");
            }

            // RAM
            ulong totalRam = 0;
            File.AppendLine("\nSystem Memory:");
            foreach (var ram in hardware.RAMSticks)
            {
                File.AppendLine($"- {ram.Manufacturer} | {ram.PartNumber} | {ram.CapacityHRF} | {ram.Speed} MHz | {ram.Name}");
                totalRam += ram.Capacity;
            }
            File.AppendLine($"\nTotal: {Utils.FormatBytes((long)totalRam)} in {hardware.RAMSticks.Count} Sticks");

            File.AppendLine("\nDisplays: " + hardware.Displays.Count);
            foreach (var ram in hardware.Displays)
            {
                File.AppendLine($"- {ram.Manufacturer} | {ram.Name}");
            }


            File.AppendLine("\nGametype: " + Database.Database.p2Database.Usersettings.Packedgame + " Mods: " + Database.Database.p2Database.Usermods.Count);
            foreach (var mod in Database.Database.p2Database.Usermods)
            {
                File.AppendLine($"-Mod: " + mod.Name + "|" + mod.Version);
            }



            File.AppendLine($"\n----------------------- Hardware Report END--------------------------");
            Clipboard.SetText(File.ToString());

            View.AppendLine($"\nCPU: {hardware.Cpu.Name}");
            View.AppendLine("\nGPUs:");
            foreach (var gpu in hardware.Gpus)
            {
                View.AppendLine($"- Name: {gpu.Name}");
                View.AppendLine($"  - Driverversion: {gpu.DriverVersion}");
                View.AppendLine($"  - Driverdate: {gpu.DriverDate}");
            }
            View.AppendLine("\nRam:");
            View.AppendLine($"Total: {Utils.FormatBytes((long)totalRam)} in {hardware.RAMSticks.Count} Sticks");


            return View.ToString();
        }



        /// <summary>
        /// Parses a boolean value into a string indicating installation status.
        /// </summary>
        /// <param name="value">The boolean value to parse.</param>
        /// <returns>"Installed" if true, otherwise "Not-Installed".</returns>
        public string ParseBool(bool value)
        {
            return value ? "Installed" : "Not-Installed";
        }

        /// <summary>
        /// Opens a website URL in the default browser, after user confirmation.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        private void OpenWebsite(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
        }

        /// <summary>
        /// Event handler to open the project's GitHub page.
        /// </summary>
        private void OpenGit(object sender, RoutedEventArgs e)
        {
            OpenWebsite("http://kuxii.de:8080/PP2-Group/PP2_Gamelauncher");
        }

        /// <summary>
        /// Event handler to open the project's official website.
        /// </summary>
        private void OpenWebsite(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://project-paradise2.de/");
        }

        /// <summary>
        /// Event handler to open the project's Facebook page.
        /// </summary>
        private void OpenFacebook(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://www.facebook.com/tdupp2");
        }

        /// <summary>
        /// Event handler to open the project's YouTube channel.
        /// </summary>
        private void OpenYoutube(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://www.youtube.com/channel/UC4kYDX2HX1ixNPO77hF8B3g");
        }

        /// <summary>
        /// Event handler to open the project's Twitter page.
        /// </summary>
        private void OpenTwitter(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://x.com/ProjParadise2");
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {
            OpenWebsite("https://nova-appendix-de2.notion.site/TDU-2-Guides-77024b8a3b3b4f518301d9d2ff354773");
        }
    }
}