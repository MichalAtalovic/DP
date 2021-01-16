namespace PubCiterAPI.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(@"author")]
    public class Author
    {
        /// <summary>
        /// Author's local system ID
        /// </summary>
        [Required]
        [Key]
        [Column(@"author_id")]
        public long AuthorId { get; set; }

        /// <summary>
        /// Author's name
        /// </summary>
        [Column(@"name")]
        public string Name { get; set; }

        /// <summary>
        /// Author's name
        /// </summary>
        [Column(@"display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Author's scholar ID
        /// </summary>
        [Column(@"scholar_id")]
        public string ScholarId { get; set; }

        /// <summary>
        /// URL to author's photo from google scholar
        /// </summary>
        [Column(@"url_picture")]
        public string UrlPicture { get; set; }

        /// <summary>
        /// Author's affiliation
        /// </summary>
        [Column(@"affiliation")]
        public string Affiliation { get; set; }

        /// <summary>
        /// Total cites of author's publications
        /// </summary>
        [Column(@"total_cites")]
        public long TotalCites { get; set; }

        /// <summary>
        /// Author's ID
        /// </summary>
        [JsonIgnore]
        [ForeignKey(@"Settings")]
        [Column(@"setting_id")]
        public long SettingsId { get; set; }

        /// <summary>
        /// Author's settings
        /// </summary>
        public virtual AuthorSettings Settings { get; set; }
    }
}
