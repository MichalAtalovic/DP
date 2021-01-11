namespace PubCiterAPI.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Publication class
    /// </summary>
    [Table(@"author_setting")]
    public class AuthorSettings
    {
        /// <summary>
        /// Setting's local system ID
        /// </summary>
        [Key]
        [Column(@"setting_id")]
        public long SettingId { get; set; }

        /// <summary>
        /// Allow synchronization from Google Scholar
        /// </summary>
        [Column(@"scholar")]
        public bool Scholar { get; set; }

        /// <summary>
        /// Allow synchronization from Semantics Scholar
        /// </summary>
        [Column(@"semantics")]
        public bool Semantics { get; set; }

        /// <summary>
        /// Allow synchronization from OpenCitations
        /// </summary>
        [Column(@"open_citations")]
        public bool OpenCitations { get; set; }

        /// <summary>
        /// Status of running synchronization [ true = running, false = idle ]
        /// </summary>
        [Column(@"sync_status")]
        public bool SyncStatus { get; set; }
    }
}
