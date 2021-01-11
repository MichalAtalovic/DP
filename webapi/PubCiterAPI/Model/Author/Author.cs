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
        [Key]
        [Column(@"name")]
        public string Name { get; set; }

        /// <summary>
        /// Author's name
        /// </summary>
        [Key]
        [Column(@"display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Author's scholar ID
        /// </summary>
        [Key]
        [Column(@"scholar_id")]
        public string ScholarId { get; set; }

        /// <summary>
        /// URL to author's photo from google scholar
        /// </summary>
        [Key]
        [Column(@"url_picture")]
        public string UrlPicture { get; set; }

        /// <summary>
        /// Author's affiliation
        /// </summary>
        [Key]
        [Column(@"affiliation")]
        public string Affiliation { get; set; }

        /// <summary>
        /// Total cites of author's publications
        /// </summary>
        [Key]
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
