namespace PubCiterAPI.Model.SyncState
{
    /// <summary>
    /// Enumerator for synchronization states
    /// </summary>
    public enum SyncStateEnum
    {
        /// <summary>
        /// Synchronization is not running
        /// </summary>
        Idle,

        /// <summary>
        /// Synchoronization skipped
        /// </summary>
        Skipped,

        /// <summary>
        /// Synchronization is awaiting run
        /// </summary>
        Pending,

        /// <summary>
        /// Synchronization is in progress
        /// </summary>
        Running,

        /// <summary>
        /// Synchronization complete
        /// </summary>
        Done
    }
}
