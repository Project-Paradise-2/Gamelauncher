using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace ProjectParadise2.Core
{
    class AntiCheat
    {
        private static readonly List<string> cheatTools = new List<string>
        {
            "cheatengine", "artmoney", "ollydbg", "x64dbg", "hackerbot", "gamehacker", "Trainer", "SpeedHack", "DLLInjector",
            "Aimbot", "WallHack", "AutoClicker", "MemoryScanner", "Extreme Injector", "NoClip", "LagSwitch", "FPSUnlocker",
            "AssemblyInjector", "KernelDebugger", "injector", "Fly", "Fly mod", "Fly Hack", "Custom Name", "TDU2 Nissan GT-R Drift", "cyborg", "Cyborg II", "cyborg II","cyborg", "cyborg ii",
            "cheat engine", "ce", "ce64", "speedhack", "table", "memory editor", "memory viewer","trainer", "trainer64",
            "inject", "dll inject", "dll injector", "manual map", "mapper", "loader", "modloader",
            "ida", "ida64", "ida pro", "ghidra", "radare2", "r2", "dnspy", "dnspyex","hook", "detour", "minhook", "easyhook", "trampoline", "inline hook",
            "macro", "macros", "autohotkey", "ahk", "auto input", "scripted input",
        };
        public static string[] Messages =
        {
                "AntiCheat: Unauthorized software detected." +
                "AntiCheat: External manipulation detected." +
                "AntiCheat: Integrity check failed." +
                "AntiCheat: Client verification failed." +
                "AntiCheat: Invalid runtime environment." +
                "Nice try. Really. But no." +
                "That was not skill. That was software." +
                "Cheat detected. Talent not found." +
                "If it looks too good, it probably is." +
                "You are not better. You are modified." +
                "Achievement unlocked: Cheater detected.",
                "Speedrun any%: Disconnected." +
                "Press ALT+F4 to play fair." +
                "Skill.exe was not found." +
                "External tools do not equal skill." +
                "Thank you for testing our AntiCheat." +
                "Your cheat was visible. Very visible." +
                "This is not a feature." +
                "Your experiment has ended." +
                "Creativity noted. Permission denied." +
                "The cheat was good. Hiding it was not." +
                "You tried. That is all.",
                "Fair play is optional. AntiCheat is not." +
                "Your tool spoke first." +
                "That was your last frame." +
                "AntiCheat: Suspicious behavior detected." +
                "AntiCheat: Runtime modification confirmed." +
                "AntiCheat: Memory tampering detected." +
                "AntiCheat: Unauthorized injection found." +
                "AntiCheat: Policy violation confirmed." +
                "This was not lag." +
                "This was not desync." +
                "This was not luck." +
                "That software betrayed you." +
                "External assistance detected." +
                "Skill cannot be downloaded." +
                "You forgot to turn the cheat off." +
                "That button was a mistake." +
                "Your gameplay raised eyebrows." +
                "Your gameplay raised alarms." +
                "Client trust level: Zero." +
                "Validation failed. Disconnecting." +
                "Your advantage was artificial." +
                "Third-party tools detected." +
                "Fair play check failed." +
                "Your session has been invalidated." +
                "That improvement was suspicious." +
                "You crossed the line." +
                "Rules were broken." +
                "Integrity compromised." +
                "Trust revoked." +
                "Session denied." +
                "Connection closed." +
                "Your shortcut was detected." +
                "Software-assisted gameplay confirmed." +
                "AntiCheat: Environment mismatch detected." +
                "AntiCheat: Debugger presence detected." +
                "AntiCheat: Emulation detected." +
                "AntiCheat: Sandbox environment detected." +
                "AntiCheat: Timing manipulation detected." +
                "AntiCheat: Input manipulation detected." +
                "Your advantage was temporary." +
                "The system noticed." +
                "The system remembers." +
                "This account is not clever." +
                "This client is not clean." +
                "Clean clients do not do this." +
                "Legitimate players do not need this." +
                "Your tools left fingerprints." +
                "That was predictable." +
                "Expected outcome achieved." +
                "Cheat detection successful." +
                "Security event triggered." +
                "Violation recorded." +
                "Behavior flagged." +
                "Session terminated by AntiCheat."
        };
        private static Process gameProcess = null;
        private static HashSet<int> knownProcesses = new HashSet<int>();
        private static DispatcherTimer monitoringTimer;
        public static event Action<bool> OnCheatDetected;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        public static void StartMonitoring()
        {
            OnCheatDetected += HandleDetection;
            monitoringTimer = new DispatcherTimer();
            monitoringTimer.Interval = TimeSpan.FromSeconds(10);
            monitoringTimer.Tick += MonitorProcessesTick;
            monitoringTimer.Start();
            InitializeProcessList();
        }

        private static void InitializeProcessList()
        {
            try
            {
                var processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    knownProcesses.Add(process.Id);
                }
            }
            catch (Exception ex)
            {
                Log.Log.Error($"Error initializing process list: {ex.Message}");
            }
        }

        private static void MonitorProcessesTick(object sender, EventArgs e)
        {
            try
            {
                CheckForNewProcesses();
            }
            catch (Exception ex)
            {
                Log.Log.Error($"Error in monitoring: {ex.Message}");
            }
        }

        private static void CheckForNewProcesses()
        {
            try
            {
                var currentProcesses = Process.GetProcesses();
                var currentProcessIds = new HashSet<int>(currentProcesses.Select(p => p.Id));
                foreach (var process in currentProcesses)
                {
                    if (!knownProcesses.Contains(process.Id))
                    {
                        knownProcesses.Add(process.Id);
                        HandleNewProcess(process);
                    }
                }
                knownProcesses.RemoveWhere(pid => !currentProcessIds.Contains(pid));
            }
            catch (Exception ex)
            {
                Log.Log.Error($"Error checking processes: {ex.Message}");
            }
        }

        public static bool CanRungame()
        {
            try
            {
                var currentProcesses = Process.GetProcesses();
                foreach (var process in currentProcesses)
                {
                    if (IsCheatTool(process.ProcessName))
                    {
                        Log.Log.Warning($"sussy process detected on startup: {process.ProcessName}");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Log.Error($"Error checking if game can run: {ex.Message}");
                return false; // Safe default
            }
        }

        private static void HandleNewProcess(Process process)
        {
            try
            {
                string processName = process.ProcessName;
                if (IsCheatTool(processName))
                {
                    OnCheatDetected?.Invoke(true);
                }
            }
            catch (Exception ex)
            {
                Log.Log.Error($"Error handling new process: {ex.Message}");
            }
        }

        public static void SetProcess(Process process)
        {
            gameProcess = process;
        }

        private static bool IsCheatTool(string processName)
        {
            try
            {
                return cheatTools.Any(cheatTool => processName.Equals(cheatTool, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Log.Log.Error($"Error checking cheat tool: {ex.Message}");
                return false;
            }
        }


        public static void Decorate()
        {
            try
            {
                var processes = Process.GetProcessesByName("TestDrive2");
                if (processes.Length > 0)
                {
                    IntPtr hwnd = processes[0].MainWindowHandle;
                    if (hwnd != IntPtr.Zero)
                    {
                        string randomId = GenerateRandomId(8);
                        string title = $"Project Paradise 2 - Secured Session [{randomId}]";
                        BackgroundWorker.SecSession = randomId;
                        SetWindowText(hwnd, title);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fehler: {ex.Message}");
            }
        }

        private static readonly Random random = new Random();
        private static string GenerateRandomId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] buffer = new char[length];

            for (int i = 0; i < length; i++)
                buffer[i] = chars[random.Next(chars.Length)];

            return new string(buffer);
        }

        static void HandleDetection(bool isCheatDetected)
        {
            if (isCheatDetected)
            {
                try
                {
                    var rnd = new Random();
                    int message = rnd.Next(0, AntiCheat.Messages.Length);
                    Log.Log.Info(@"                                                                        ");
                    Log.Log.Info(@"                                  .___.                                 ");
                    Log.Log.Info(@"              /)               ,-^    ^-.                               ");
                    Log.Log.Info(@"             //               /           \                             ");
                    Log.Log.Info(@"    .-------| | -------------/  __     __  \-------------------.__      ");
                    Log.Log.Info(@"    |WMWMWMW| |>>>>>>>>>>>>> | />>\   />>\ |>>>>>>>>>>>>>>>>>>>>>>:>    ");
                    Log.Log.Info(@"    `-------| | -------------| \__/   \__/ |-------------------'^^      ");
                    Log.Log.Info(@"             \\               \    /|\    /                             ");
                    Log.Log.Info(@"              $               \   \_ /  /                               ");
                    Log.Log.Info(@"                                |       |                               ");
                    Log.Log.Info(@"                                |+ + + +|                               ");
                    Log.Log.Info(@"                                \       /                               ");
                    Log.Log.Info(@"                                 ^-----^                                ");
                    Log.Log.Info(AntiCheat.Messages[message]);
                    Log.Log.Info(@"                                                                        ");
                    Log.Log.Info(@$"game killed by Kuxii ;) SecId: {BackgroundWorker.SecSession}           ");
                    GameRunner.KillGame();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var mainWindow = Application.Current.MainWindow;
                        if (mainWindow != null)
                        {
                            MessageBox.Show(mainWindow, Lang.GetText(110),
                                Lang.GetText(109),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.Log.Error($"Error in handle detection: {ex.Message}");
                }
            }
        }
    }
}