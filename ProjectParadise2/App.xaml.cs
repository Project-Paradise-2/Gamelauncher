using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace ProjectParadise2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Flag to specify default library search directories.
        /// </summary>
        private const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00000800;

        /// <summary>
        /// Sets the default DLL directories for the process.
        /// </summary>
        /// <param name="directoryFlags">The directory flags to set.</param>
        /// <returns>True if successful, otherwise false.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetDefaultDllDirectories(int directoryFlags);

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// Prevents loading unwanted crash fixes by modifying DLL search directories.
        /// </summary>
        public App()
        {
            if (!SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS))
            {
                Console.WriteLine($"Failed to exclude application directory from DLL directories, error code: {Marshal.GetLastWin32Error()}");
            }
        }
    }
}