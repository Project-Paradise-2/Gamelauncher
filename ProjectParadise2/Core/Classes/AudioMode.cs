namespace ProjectParadise2.Core.Classes
{
    /// <summary>
    /// Represents the audio modes used in the application.
    /// </summary>
    public enum AudioMode
    {
        /// <summary>
        /// Represents the DirectSound audio mode, a legacy audio API from Microsoft.
        /// </summary>
        DirectSound,

        /// <summary>
        /// Represents the XAudio2 audio mode, a more modern and advanced audio API from Microsoft.
        /// </summary>
        XAudio2
    }
}