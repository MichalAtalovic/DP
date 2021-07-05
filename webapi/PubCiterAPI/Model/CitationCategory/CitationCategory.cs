namespace PubCiterAPI.Model
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(@"citation_category")]
    public class CitationCategory
    {
        /// <summary>
        /// Citation category's local system ID
        /// </summary>
        [Required]
        [Key]
        [Column(@"id")]
        public long Id { get; set; }

        /// <summary>
        /// Category code
        /// </summary>
        [Column(@"code")]
        public long Code { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [Column(@"name")]
        public string Name { get; set; }
    }
}
