using System.Collections.Generic;

namespace ProjectParadise2
{
    /// <summary>
    /// Represents the details of a data file including its name, path, size, and hash.
    /// </summary>
    public class FileInfos
    {
        /// <summary>
        /// The name of the data file.
        /// </summary>
        public string FileName;

        /// <summary>
        /// The file path where the data file is stored.
        /// </summary>
        public string FilePath;

        /// <summary>
        /// The size of the data file, typically in bytes or a string representation of size.
        /// </summary>
        public string FileSize;

        /// <summary>
        /// The hash of the data file, used for verification purposes.
        /// </summary>
        public string FileHash;
    }

    /// <summary>
    /// Represents the information related to updates, including file details, directories, and version.
    /// </summary>
    public class UpdateInfos
    {
        /// <summary>
        /// List of file information for the update, including file name, path, size, and hash.
        /// </summary>
        public List<FileInfos> File = new List<FileInfos>();

        /// <summary>
        /// Array of directory paths associated with the update.
        /// </summary>
        public string[] Directorys;

        /// <summary>
        /// The version of the update.
        /// </summary>
        public string Version;
    }
}
