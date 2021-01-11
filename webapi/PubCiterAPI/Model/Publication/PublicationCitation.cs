namespace PubCiterAPI.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Newtonsoft.Json;

    /// <summary>
    /// PublicationCitation class
    /// </summary>
    [Table(@"publication_citation")]
    public class PublicationCitation
    {
        /// <summary>
        /// Publication's local system ID
        /// </summary>
        [JsonIgnore]
        [Column(@"pub_id")]
        public long PublicationId { get; set; }

        /// <summary>
        /// Citation's local system ID
        /// </summary>
        [Column(@"citation_id")]
        public long CitationId { get; set; }

        /// <summary>
        /// Publication object
        /// </summary>
        [ForeignKey(@"PublicationId")]
        public Publication Publication { get; set; }

        /// <summary>
        /// Citation object
        /// </summary>
        [ForeignKey(@"CitationId")]
        public Citation Citation { get; set; }
    }
}
