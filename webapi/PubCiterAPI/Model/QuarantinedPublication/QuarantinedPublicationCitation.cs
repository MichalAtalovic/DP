namespace PubCiterAPI.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Newtonsoft.Json;

    /// <summary>
    /// QuarantinedPublicationCitation class
    /// </summary>
    [Table(@"quarantine_citation")]
    public class QuarantinedPublicationCitation
    {
        /// <summary>
        /// Quarantined publication's local system ID
        /// </summary>
        [JsonIgnore]
        [Column(@"quarantine_id")]
        public long QuarantinedPublicationId { get; set; }

        /// <summary>
        /// Citation's local system ID
        /// </summary>
        [Column(@"citation_id")]
        public long CitationId { get; set; }

        /// <summary>
        /// QUarantined publication object
        /// </summary>
        [ForeignKey(@"QuarantinedPublicationId")]
        public QuarantinedPublication QuarantinedPublication { get; set; }

        /// <summary>
        /// Citation object
        /// </summary>
        [ForeignKey(@"CitationId")]
        public Citation Citation { get; set; }
    }
}
