using System.Collections.Generic;

namespace ProjectParadise2.Core.JsonClasses
{
    /// <summary>
    /// Represents information about a file in the mod.
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The path of the file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The size of the file in bytes.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// The hash of the file (e.g., for integrity checks).
        /// </summary>
        public string FileHash { get; set; }
    }

    /// <summary>
    /// Represents a game mod with associated information such as its image, files, and metadata.
    /// </summary>
    public class GameMod
    {
        /// <summary>
        /// The image data of the mod, typically used as a thumbnail or icon.
        /// </summary>
        public byte[] Modimage { get; set; }

        /// <summary>
        /// A list of directories related to the mod.
        /// </summary>
        public List<object> Directorys { get; set; }

        /// <summary>
        /// A list of files associated with the mod.
        /// </summary>
        public List<FileInfo> File { get; set; }

        /// <summary>
        /// The name of the project associated with the mod.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// The creator of the mod.
        /// </summary>
        public string ModCreator { get; set; }

        /// <summary>
        /// The version of the mod.
        /// </summary>
        public string Modversion { get; set; }

        /// <summary>
        /// The name of the mod.
        /// </summary>
        public string Modname { get; set; }

        /// <summary>
        /// A description of the mod.
        /// </summary>
        public string Moddescription { get; set; }

        /// <summary>
        /// The type of the mod, represented as an integer (e.g., packed, unpacked, etc.).
        /// </summary>
        public int Modtype { get; set; }

        /// <summary>
        /// The compression permission level for the mod.
        /// </summary>
        public int Compremission { get; set; }

        /// <summary>
        /// The download size of the mod in bytes.
        /// </summary>
        public long Downloadsize { get; set; }

        /// <summary>
        /// The disk size of the mod in bytes.
        /// </summary>
        public long Discsize { get; set; }
    }
}
