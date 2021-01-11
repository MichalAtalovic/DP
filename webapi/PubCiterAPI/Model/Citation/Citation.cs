namespace PubCiterAPI.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Newtonsoft.Json;

    /// <summary>
    /// Citation class
    /// </summary>
    [Table(@"citation")]
    public class Citation
    {
        /// <summary>
        /// Citation's local system ID
        /// </summary>
        [Key]
        [Column(@"citation_id")]
        public long CitationId { get; set; }

        /// <summary>
        /// Citation's title
        /// </summary>
        [Column(@"title")]
        public string Title { get; set; }

        /// <summary>
        /// Citation's author
        /// </summary>
        [Column(@"author")]
        public string Author { get; set; }

        /// <summary>
        /// Citation's author
        /// </summary>
        [Column(@"pub_year")]
        public string PublicationYear { get; set; }

        /// <summary>
        /// Medium of publication where the citing publication was published
        /// </summary>
        [Column(@"venue")]
        public string Journal { get; set; }

        /// <summary>
        /// Citation's abstract
        /// </summary>
        [Column(@"abstract")]
        public string Abstract { get; set; }

        /// <summary>
        /// Paper's digital object identifier (DOI)
        /// </summary>
        [Column(@"doi")]
        public string Doi { get; set; }
    }
}
