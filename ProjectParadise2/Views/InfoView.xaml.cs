using HardwareInformation;
using ProjectParadise2.Core;
using ProjectParadise2.Core.Log;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            _ = RefreshAsync();
        }

        /// <summary>
        /// Refreshes the information displayed in the view.
        /// Updates DirectX status, launcher version, redistributable packages, port state, and build time.
        /// </summary>
        public async Task RefreshAsync()
        {
            try
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    Launcherversion.Text = Constans.LauncherVersion.ToString();
                    Launcherbuild.Text = new FileInfo(Assembly.GetExecutingAssembly().Location).CreationTime.ToString();
                });

                var hardware = MachineInformationGatherer.GatherInformation(true);
                ulong totalRam = 0;
                foreach (var ram in hardware.RAMSticks)
                {
                    totalRam += ram.Capacity;
                }
                string gpuText = string.Join("\n\n", hardware.Gpus.Select(g =>
                    $"{g.Name} VRAM: {g.AvailableVideoMemoryHRF} - {g.Type}\n - DriverVersion: {g.DriverVersion}, DriverDate: {g.DriverDate}"));

                string displayText = string.Join("\n", hardware.Displays.Select(d => $"- {d.Manufacturer} | {d.Name}"));

                await Dispatcher.InvokeAsync(() =>
                {
                    CPU.Text = $"{hardware.Cpu.Name} {hardware.Cpu.Socket}\n" +
                               $"- Logical Cores: {hardware.Cpu.LogicalCores}\n" +
                               $"- Physical Cores: {hardware.Cpu.PhysicalCores}\n" +
                               $"- Speed: {hardware.Cpu.NormalClockSpeed} MHz";

                    RAM.Text = $"{Utils.FormatBytes((long)totalRam)} in {hardware.RAMSticks.Count} Sticks";
                    GPU.Text = gpuText;
                    Displays.Text = displayText;
                    Network.Text = $"{NatDetector.Result.AfterUpnp.ToString()}" +
                    $"\n{NatDetector.Result.AfterUpnp.InternToString()}\n" +
                    $"Forwarding: {ParseNatBool(NatDetector.Result.UpnpAvailable)} \nType: {NatDetector.Result.ForwardingType}\n{NatDetector.Result.UpnpStatus} Port: {NatDetector.Result.UpnpPort}";
                    Network.ToolTip = NatDetector.Result.InternalReachable
                                        ? "This port is open and reachable from the internet."
                                        : "Connection might be blocked; external peers cannot reach this port.";

                });

                bool isVC2005x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x86);
                bool isVC2008x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x86);

                bool VC2010x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2010x86);
                bool VC2012x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2012x86);
                bool VC2013x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2013x86);

                bool isVC2005x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x64);
                bool isVC2008x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x64);

                bool VC2010x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2010x64);
                bool VC2012x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2012x64);
                bool VC2013x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2013x64);

                await Dispatcher.InvokeAsync(() =>
                {
                    CPlusText.Text =
                        $"VC2005* (x86): {ParseBool(isVC2005x86)}\n" +
                        $"VC2008  (x86): {ParseBool(isVC2008x86)}\n" +
                        $"VC2010  (x86): {ParseBool(VC2010x86)}\n" +
                        $"VC2012  (x86): {ParseBool(VC2012x86)}\n" +
                        $"VC2013  (x86): {ParseBool(VC2013x86)}\n" +

                        $"VC2005  (x64): {ParseBool(isVC2005x64)}\n" +
                        $"VC2008  (x64): {ParseBool(isVC2008x64)}\n" +
                        $"VC2010  (x64): {ParseBool(VC2010x64)}\n" +
                        $"VC2012  (x64): {ParseBool(VC2012x64)}\n" +
                        $"VC2013  (x64): {ParseBool(VC2013x64)}\n";
                });
            }
            catch (Exception ex)
            {
                Log.Error("Failed to initialize InfoView: ", ex);
            }
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
        /// Parses a boolean value into a string indicating NAT usage status.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ParseNatBool(bool value)
        {
            return value ? "Used" : "Not-Used";
        }

        /// <summary>
        /// Copies the gathered hardware and software information to the clipboard in a formatted manner.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopytoClipboard(object sender, RoutedEventArgs e)
        {
            try
            {
                var hardware = MachineInformationGatherer.GatherInformation(true);
                bool isVC2005x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x86);
                bool isVC2008x86 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x86);
                bool isVC2005x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x64);
                bool isVC2008x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x64);

                var fileInfo = new StringBuilder();
                fileInfo.AppendLine("```");
                fileInfo.AppendLine($"OS: {hardware.OperatingSystem.VersionString}");
                fileInfo.AppendLine($"Platform: {hardware.Platform}");
                fileInfo.AppendLine();
                fileInfo.AppendLine($"CPU: {hardware.Cpu.Name} {hardware.Cpu.Socket}");
                fileInfo.AppendLine($"- Logical Cores: {hardware.Cpu.LogicalCores}");
                fileInfo.AppendLine($"- Physical Cores: {hardware.Cpu.PhysicalCores}");
                fileInfo.AppendLine($"- Speed: {hardware.Cpu.NormalClockSpeed} MHz");
                fileInfo.AppendLine();
                fileInfo.AppendLine("GPUs:");
                foreach (var gpu in hardware.Gpus)
                {
                    fileInfo.AppendLine($"- Name: {gpu.Name} Device: {gpu.Type}");
                    fileInfo.AppendLine($"  - VRAM: {gpu.AvailableVideoMemoryHRF}");
                    fileInfo.AppendLine($"  - Driverversion: {gpu.DriverVersion}");
                    fileInfo.AppendLine($"  - Driverdate: {gpu.DriverDate}");
                }

                ulong totalRam = 0;
                fileInfo.AppendLine();
                fileInfo.AppendLine("System Memory:");
                foreach (var ram in hardware.RAMSticks)
                {
                    fileInfo.AppendLine($"- {ram.Manufacturer} | {ram.PartNumber} | {ram.CapacityHRF} | {ram.Speed} MHz | {ram.Name}");
                    totalRam += ram.Capacity;
                }
                fileInfo.AppendLine();
                fileInfo.AppendLine($"Total: {Utils.FormatBytes((long)totalRam)} in {hardware.RAMSticks.Count} Sticks");
                fileInfo.AppendLine();
                fileInfo.AppendLine("Displays: " + hardware.Displays.Count);
                foreach (var display in hardware.Displays)
                {
                    fileInfo.AppendLine($"- {display.Manufacturer} | {display.Name}");
                }

                fileInfo.AppendLine();
                fileInfo.AppendLine("Network:");
                string output = Regex.Replace(NatDetector.Result.AfterUpnp.ToString(), @"(\d+)\.(\d+)\.(\d+)\.(\d+)", "$1.$2.XXX.XXX");
                string netInfo = $"{output}\n{NatDetector.Result.AfterUpnp.InternToString()}\nForwarding: {ParseNatBool(NatDetector.Result.UpnpAvailable)} \nType: {NatDetector.Result.ForwardingType}\n{NatDetector.Result.UpnpStatus} Port: {NatDetector.Result.UpnpPort}";
                fileInfo.AppendLine(netInfo);
                string softwareInfo = $"\nSoftware:\n -VC2005x86:{isVC2005x86}, VC2005x64:{isVC2005x64}\n -VC2008x86:{isVC2008x86}, VC2008x64:{isVC2008x64}\n";
                fileInfo.AppendLine(softwareInfo);

                fileInfo.AppendLine("Launcher Gameprofile:");
                for (int i = 0; i < BackgroundWorker.GameProfiles.Count; i++)
                {
                    var profile = BackgroundWorker.GameProfiles[i];
                    fileInfo.AppendLine();
                    fileInfo.AppendLine($"Game Profile (" + (i + 1) + "|" + BackgroundWorker.GameProfiles.Count + $") TDU-Build: {profile.Gametype}");
                    fileInfo.AppendLine($"Profile: {profile.Profilename} type: {profile.Build} isSteam: {profile.SteamBuild}");
                    fileInfo.AppendLine($"GamePath: {profile.Gamepath}");
                    fileInfo.AppendLine($"Version: {profile.Version} | FileVersion: {profile.FileVersion}");
                    fileInfo.AppendLine($"Exe: {profile.Executable} | Debug: {profile.IsDebug}");
                    fileInfo.AppendLine($"Prio+: {ParseNatBool(profile.HighPrio)} | Ram+: {ParseNatBool(profile.LAAEnabled)}");
                    fileInfo.AppendLine($"Cores+: {ParseNatBool(profile.UseMoreCores)} | Online: {ParseNatBool(profile.OnlineMode)}");
                    fileInfo.AppendLine($"Damage: {ParseNatBool(profile.VehicleDamage)} | Dirt: {ParseNatBool(profile.VehicleDirt)}");
                    fileInfo.AppendLine($"RunParams: {profile.Arguments}");
                    fileInfo.AppendLine($"Gamemods({profile.Usermods.Count}):");
                    foreach (var mod in profile.Usermods)
                    {
                        fileInfo.AppendLine($"-Mod: {mod.Name}|{mod.Version} Installed: {mod.Installed}");
                    }
                }

                fileInfo.AppendLine();
                fileInfo.AppendLine("```");

                Clipboard.SetText(fileInfo.ToString());

                MessageBox.Show(
                    "Hardware information has been copied to the clipboard.",
                    "Project Paradise 2 - Infocreator",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to copy hardware info to clipboard: ", ex);
                MessageBox.Show(
                    "Failed to copy hardware information to the clipboard.",
                    "Project Paradise 2 - Infocreator",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}