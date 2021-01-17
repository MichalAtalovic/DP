using Swashbuckle.AspNetCore.Annotations;

namespace PubCiterAPI.Model.SyncState
{
    /// <summary>
    /// Synchronization state model
    /// </summary>
    public class SyncState
    {
        /// <summary>
        /// State of Google Scholar sync run
        /// </summary>
        public SyncStateEnum GoogleScholar { get; set; }

        /// <summary>
        /// State of Semantics Scholar sync run
        /// </summary>
        public SyncStateEnum SemanticsScholar { get; set; }

        /// <summary>
        /// State of OpenCitations sync run
        /// </summary>
        public SyncStateEnum OpenCitations { get; set; }
    }
}
