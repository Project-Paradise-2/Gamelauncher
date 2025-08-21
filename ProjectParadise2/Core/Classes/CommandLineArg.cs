namespace ProjectParadise2.Core.Classes
{
    /// <summary>
    /// A class to handle and store command line arguments passed to the application.
    /// It processes the flags and updates the corresponding static properties.
    /// </summary>
    public class CommandLineArg
    {
        /// <summary>
        /// Flag to skip UPnP functionality.
        /// </summary>
        public static bool SkipUpnp = false;

        /// <summary>
        /// Flag to automatically run the application.
        /// </summary>
        public static bool AutoRun = false;

        /// <summary>
        /// Flag to enable debug mode.
        /// </summary>
        public static bool IsDebug = false;

        /// <summary>
        /// Flag to control the visibility of the application on start.
        /// </summary>
        public static bool HideOnStart = true;

        /// <summary>
        /// Flag to set whether the application should be in online mode.
        /// </summary>
        public static bool OnlineMode = true;

        /// <summary>
        /// Flag to indicate if the application is running directly from a folder.
        /// </summary>
        public static bool IsInfolder = false;

        public static bool DebugUI = false;

        /// <summary>
        /// Reads the command line arguments passed to the application and updates
        /// the corresponding flags based on the presence of specific arguments.
        /// </summary>
        /// <param name="args">The array of command line arguments.</param>
        public static void ReadArgs(string[] args)
        {
            Database.Database.Read();
            string flags = "Read Launcher Flags:";

            // Iterate over each argument and check for specific flags
            foreach (string arg in args)
            {
                if (arg.Contains("-Skipupnp"))
                {
                    SkipUpnp = true;
                    flags += " +Skipupnp";
                }
                if (arg.Contains("-AutoRun"))
                {
                    AutoRun = true;
                    flags += " +AutoRun";
                }
                if (arg.Contains("-debug"))
                {
                    IsDebug = true;
                    flags += " +debug";
                }
                if (arg.Contains("-noHide"))
                {
                    HideOnStart = false;
                    flags += " +noHide";
                }
                if (arg.Contains("-offline"))
                {
                    OnlineMode = false;
                    flags += " +offline";
                }
                if (arg.Contains("-direct"))
                {
                    IsInfolder = true;
                    flags += " +direct";
                }
                if (arg.Contains("-uiDebug"))
                {
                    DebugUI = true;
                    flags += " +uiDebug";
                }
            }

            // Log the flags read from command line arguments
            if (flags.Equals("Read Launcher Flags:"))
            {
                Log.Log.Print("No Custom flags set.");
            }
            else
            {
                Log.Log.Print(flags);
            }

            // Perform additional actions if necessary based on specific flags
            if (SkipUpnp && AutoRun && !OnlineMode)
            {
                // Placeholder for any specific logic based on these flags.
            }
        }
    }
}
