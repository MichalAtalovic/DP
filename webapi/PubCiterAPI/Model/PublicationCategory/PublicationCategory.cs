namespace PubCiterAPI.Model
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(@"publication_category")]
    public class PublicationCategory
    {
        /// <summary>
        /// Publication category's local system ID
        /// </summary>
        [Required]
        [Key]
        [Column(@"id")]
        public long Id { get; set; }

        /// <summary>
        /// Category group
        /// </summary>
        [Column(@"category_group")]
        public string Group { get; set; }

        /// <summary>
        /// Category code
        /// </summary>
        [Column(@"code")]
        public string Code { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        [Column(@"name")]
        public string Name { get; set; }
    }
}
