using System.Collections.Generic;

namespace ProjectParadise2.JsonClasses
{
    /// <summary>
    /// Represents information about a specific mod.
    /// </summary>
    public class ModInfo
    {
        /// <summary>
        /// The unique identifier for the mod.
        /// </summary>
        public string ModId { get; set; }

        /// <summary>
        /// The publisher of the mod.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// The type of the mod (e.g., packed, unpacked, etc.).
        /// </summary>
        public string ModType { get; set; }

        /// <summary>
        /// The file path to the mod on the local system.
        /// </summary>
        public string ModPath { get; set; }

        /// <summary>
        /// A flag indicating whether the mod is public.
        /// </summary>
        public string Public { get; set; }

        /// <summary>
        /// The date or timestamp when the mod was last modified.
        /// </summary>
        public string Changed { get; set; }

        /// <summary>
        /// Additional notes or metadata associated with the mod (could be an object for flexibility).
        /// </summary>
        public object Note { get; set; }

        /// <summary>
        /// The number of downloads of the mod.
        /// </summary>
        public string Downloads { get; set; }

        /// <summary>
        /// The number of uploads of the mod.
        /// </summary>
        public string Uploads { get; set; }

        /// <summary>
        /// The current state of the mod (e.g., active, archived, etc.).
        /// </summary>
        public string ModState { get; set; }
    }

    /// <summary>
    /// Represents a root object containing a list of mod information.
    /// </summary>
    public class Root
    {
        /// <summary>
        /// A list of <see cref="ModInfo"/> objects, each representing a specific mod.
        /// </summary>
        public List<ModInfo> data { get; set; } = new List<ModInfo>();
    }
}
