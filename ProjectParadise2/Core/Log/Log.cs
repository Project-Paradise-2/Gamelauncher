using ProjectParadise2.Core.Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ProjectParadise2.Core.Log
{
    internal class Log
    {
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

            Print("[PP2]Project-Paradise 2 v: " + Constans.LauncherVersion + " started.");
        }

        public static void Print(string message, Exception ex = null)
        {
            try
            {
                // Check if the database is loaded
                if (!Database.Database.IsLoadet)
                    return;

                // Get method and class names from the stack trace
                var stackFrame = new StackTrace().GetFrame(1);
                var method = stackFrame.GetMethod();
                var className = method.DeclaringType.Name;
                var methodName = method.Name;

                // Prepare the log message
                string logtext = "";
                if (CommandLineArg.IsDebug)
                {
                    logtext = $"[{className}:{methodName}] {message}";
                }
                else
                {
                    logtext = $"{message}";
                }

                // If an exception is passed, log the exception details
                if (ex != null)
                {
                    logtext += $" error: {ex.Message} \n{ex.StackTrace} \n{ex.Source} \n{ex.InnerException?.ToString()}";
                }

                // Define the log file path
                string logFilePath = Path.Combine(Constans.DokumentsFolder, "Log", "last.log");

                // Using 'using' to ensure streams are closed after usage
                using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.Write))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.ASCII))
                {
                    sw.WriteLine($"{DateTime.Now:HH:mm:ss} {logtext}");
                }

                // Write to debug output
                Debug.WriteLine($"{DateTime.Now:HH:mm:ss} {logtext}");

            }
            catch (Exception logEx)
            {
                // Optionally log the error that occurred during the logging process
                Debug.WriteLine($"Error in logging: {logEx.Message}");
            }
        }

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
