using ProjectParadise2.JsonClasses;
using System;
using System.Collections.Generic;

namespace ProjectParadise2.Database.Data
{
    /// <summary>
    /// Represents a game mod with details such as its ID, name, version, installation date, whether it is packed, and associated files.
    /// </summary>
    [Serializable]
    public class Gamemod
    {
        /// <summary>
        /// The unique identifier for the mod.
        /// </summary>
        public int ModId { get; set; }

        /// <summary>
        /// The name of the mod.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version of the mod.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The date and time when the mod was installed.
        /// </summary>
        public string Installed { get; set; }

        /// <summary>
        /// Indicates whether the mod is packed (e.g., whether it is bundled in a single file).
        /// </summary>
        public bool Packed { get; set; }

        /// <summary>
        /// A list of files associated with the mod, represented by <see cref="FileInfo"/> objects.
        /// </summary>
        public List<FileInfo> Files { get; set; }
    }
}
