using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectParadise2.Core.Log
{
    internal class Log
    {
        private static readonly object _lock = new object();

        public static void Initlog()
        {
            if (!Directory.Exists(Constans.DokumentsFolder))
            {
                Directory.CreateDirectory(Constans.DokumentsFolder);
            }

            if (!Directory.Exists(Constans.DokumentsFolder + "Log/"))
            {
                Directory.CreateDirectory(Constans.DokumentsFolder + "Log/");
            }

            if (File.Exists(Constans.DokumentsFolder + "Log/" + "last.log"))
            {
                File.Delete(Constans.DokumentsFolder + "Log/" + "last.log");
            }

            if (File.Exists(Constans.DokumentsFolder + "Log/" + "nat.log"))
            {
                File.Delete(Constans.DokumentsFolder + "Log/" + "nat.log");
            }

            Info("[PP2]Project-Paradise 2 v: " + Constans.LauncherVersion + " started.");
        }

        private static string GetCallerInfo([CallerMemberName] string callerMember = "")
        {
            var stackTrace = new StackTrace(skipFrames: 1, fNeedFileInfo: false);
            foreach (var frame in stackTrace.GetFrames())
            {
                var method = frame.GetMethod();
                if (method == null) continue;

                var declaringType = method.DeclaringType;
                if (declaringType == null) continue;

                string fullName = declaringType.FullName;

                // Log-Klasse selbst überspringen
                if (fullName == typeof(Log).FullName)
                    continue;

                // Compiler-generierte async/state-machine Klassen überspringen
                if (fullName.Contains("+<") ||
                    fullName.StartsWith("System.Runtime.CompilerServices") ||
                    fullName.StartsWith("System.Threading.Tasks"))
                    continue;

                return $"{fullName}::{method.Name}";
            }

            // Fallback auf CallerMemberName (hilft bei async)
            return callerMember;
        }

        private static void LogMessage(string type, ConsoleColor color, object[] parameters, [CallerMemberName] string callerMember = "")
        {
            lock (_lock)
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                string caller = GetCallerInfo(callerMember);

                string paramStr = string.Join(", ", parameters ?? Array.Empty<object>());

                string logMessage = $"[{time}] [{type}] {caller}({paramStr})";

                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{time}] ");
                Console.ForegroundColor = color;
                Console.Write($"[{type}] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"{caller}({paramStr})");
                Console.ForegroundColor = oldColor;

                WriteToFile(logMessage);
            }
        }

        private static void WriteToFile(string message)
        {
            try
            {
                string filePath = Path.Combine(Constans.DokumentsFolder, "Log", "last.log");
                using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Write))
                using (var sw = new StreamWriter(fs, Encoding.ASCII))
                {
                    sw.WriteLine(message);
                }
            }
            catch
            {
                // intentionally empty
            }
        }

        public static void Debug(params object[] parameters) => LogMessage("DEBUG", ConsoleColor.DarkYellow, parameters);
        public static void Info(params object[] parameters) => LogMessage("INFO", ConsoleColor.Green, parameters);
        public static void Warning(params object[] parameters) => LogMessage("WARNING", ConsoleColor.Yellow, parameters);
        public static void Error(params object[] parameters) => LogMessage("ERROR", ConsoleColor.Red, parameters);

        public static void PrintMod(string message, string Modname)
        {
            try
            {
                if (!Database.Database.p2Database.Usersettings.Launcherlogging)
                {
                    return;
                }

                FileStream fs = new FileStream(Constans.DokumentsFolder + "Modloader/" + Modname + ".log", FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Constans.DokumentsFolder + "Modloader/" + Modname + ".log", true, Encoding.ASCII);
                sw.Write(DateTime.Now.ToString("HH:mm:ss ") + message + " \n");
                sw.Close();
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + message + " \n");
            }
            catch (Exception) { }
        }
    }
}
