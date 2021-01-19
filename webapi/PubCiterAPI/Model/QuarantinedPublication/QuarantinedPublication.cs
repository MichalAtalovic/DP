namespace PubCiterAPI.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Quarantine class
    /// </summary>
    [Table(@"quarantine")]
    public class QuarantinedPublication
    {
        /// <summary>
        /// Publication's local system ID
        /// </summary>
        [Key]
        [Column(@"pub_id")]
        public long QuarantinedPublicationId { get; set; }

        /// <summary>
        /// Author's ID
        /// </summary>
        [ForeignKey(@"Author")]
        [Column(@"author_id")]
        public long AuthorId { get; set; }

        /// <summary>
        /// Publications's author
        /// </summary>
        public virtual Author Author { get; set; }

        /// <summary>
        /// Publication's title
        /// </summary>
        [Column(@"title")]
        public string Title { get; set; }

        /// <summary>
        /// Publication's year
        /// </summary>
        [Column(@"pub_year")]
        public string PublicationYear { get; set; }

        /// <summary>
        /// Publication's co-authors
        /// </summary>
        [Column(@"author")]
        public string Authors { get; set; }

        /// <summary>
        /// Medium of publication
        /// </summary>
        [Column(@"journal")]
        public string Journal { get; set; }

        /// <summary>
        /// Volume of the journal in which the publication was published
        /// </summary>
        [Column(@"volume")]
        public string JournalVolume { get; set; }

        /// <summary>
        /// Number of publication pages
        /// </summary>
        [Column(@"pages")]
        public string Pages { get; set; }

        /// <summary>
        /// Publication's publisher
        /// </summary>
        [Column(@"publisher")]
        public string Publisher { get; set; }

        /// <summary>
        /// Publication's abstract
        /// </summary>
        [Column(@"abstract")]
        public string Abstract { get; set; }

        /// <summary>
        /// Paper's digital object identifier (DOI)
        /// </summary>
        [Column(@"doi")]
        public string Doi { get; set; }

        /// <summary>
        /// Table mapping
        /// </summary>
        public virtual ICollection<QuarantinedPublicationCitation> QuarantinedPublicationCitationList { get; set; }
    }
}
