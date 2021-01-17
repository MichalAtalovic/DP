using PubCiterAPI.Model.SyncState;

namespace PubCiterAPI
{
    /// <summary>
    /// Synchronization state model
    /// </summary>
    public static class CurrentSyncState
    {
        /// <summary>
        /// State of Google Scholar sync run
        /// </summary>
        public static SyncStateEnum GoogleScholar { get; set; }

        /// <summary>
        /// State of Semantics Scholar sync run
        /// </summary>
        public static SyncStateEnum SemanticsScholar { get; set; }

        /// <summary>
        /// State of OpenCitations sync run
        /// </summary>
        public static SyncStateEnum OpenCitations { get; set; }
    }
}
