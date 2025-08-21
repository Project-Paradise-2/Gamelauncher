namespace ProjectParadise2.Database.Data
{
    /// <summary>
    /// Defines the possible backup types for the application.
    /// </summary>
    public enum BackUptype : byte
    {
        /// <summary>
        /// The backup will be triggered when the application starts.
        /// </summary>
        OnStart,

        /// <summary>
        /// The backup will be triggered when the application closes.
        /// </summary>
        OnClose
    }
}