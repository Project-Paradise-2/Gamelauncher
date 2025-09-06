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
                bool isVC2005x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2005x64);
                bool isVC2008x64 = RedistributablePackage.IsInstalled(RedistributablePackageVersion.VC2008x64);

                await Dispatcher.InvokeAsync(() =>
                {
                    CPlusText.Text =
                        $"VC2005* (x86): {ParseBool(isVC2005x86)}\n" +
                        $"VC2008  (x86): {ParseBool(isVC2008x86)}\n" +
                        $"VC2005* (x64): {ParseBool(isVC2005x64)}\n" +
                        $"VC2008  (x64): {ParseBool(isVC2008x64)}";
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
                StringBuilder File = new StringBuilder();
                File.AppendLine($"```----------------------- System Report -----------------------------\n");
                File.AppendLine($"OS: {hardware.OperatingSystem.VersionString}");
                File.AppendLine($"Platform: {hardware.Platform}");
                File.AppendLine($"\nCPU: {hardware.Cpu.Name} {hardware.Cpu.Socket}");
                File.AppendLine($"- Logical Cores: {hardware.Cpu.LogicalCores}");
                File.AppendLine($"- Physical Cores: {hardware.Cpu.PhysicalCores}");
                File.AppendLine($"- Speed: {hardware.Cpu.NormalClockSpeed} MHz");
                File.AppendLine("\nGPUs:");
                foreach (var gpu in hardware.Gpus)
                {
                    File.AppendLine($"- Name: {gpu.Name} Device: {gpu.Type.ToString()}");
                    File.AppendLine($"  - VRAM: {gpu.AvailableVideoMemoryHRF}");
                    File.AppendLine($"  - Driverversion: {gpu.DriverVersion}");
                    File.AppendLine($"  - Driverdate: {gpu.DriverDate}");
                }
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
                File.AppendLine("\nNetwork: ");
                string output = Regex.Replace(NatDetector.Result.AfterUpnp.ToString(), @"(\d+)\.(\d+)\.(\d+)\.(\d+)", "$1.$2.XXX.XXX");
                string net = $"{output}\n{NatDetector.Result.AfterUpnp.InternToString()}\nForwarding: {ParseNatBool(NatDetector.Result.UpnpAvailable)} \nType: {NatDetector.Result.ForwardingType}\n{NatDetector.Result.UpnpStatus} Port: {NatDetector.Result.UpnpPort}";
                File.AppendLine(net);
                File.AppendLine("\nGametype: " + Database.Database.p2Database.Usersettings.Packedgame + " Mods: " + Database.Database.p2Database.Usermods.Count);
                foreach (var mod in Database.Database.p2Database.Usermods)
                {
                    File.AppendLine($"-Mod: " + mod.Name + "|" + mod.Version);
                }
                string Stoftware = $"\nSoftware:\n -VC2005x86:{isVC2005x86}, VC2005x64:{isVC2005x64}\n -VC2008x86:{isVC2008x86}, VC2008x64:{isVC2008x64}\n";
                File.AppendLine(Stoftware);
                File.AppendLine($"\n----------------------- System Report END--------------------------```");
                Clipboard.SetText(File.ToString());
                MessageBoxResult state = MessageBox.Show(
                    "Hardware information has been copied to the clipboard.",
                    "Project Paradise 2 - Infocreator",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to copy hardware info to clipboard: ", ex);
                MessageBoxResult state = MessageBox.Show(
                    "Failed to copy hardware information to the clipboard.",
                    "Project Paradise 2 - Infocreator",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}