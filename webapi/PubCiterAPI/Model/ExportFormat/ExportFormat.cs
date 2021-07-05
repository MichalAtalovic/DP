namespace PubCiterAPI.Model
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(@"export_format")]
    public class ExportFormat
    {
        /// <summary>
        /// Format's local system ID
        /// </summary>
        [Required]
        [Key]
        [Column(@"id")]
        public long Id { get; set; }

        /// <summary>
        /// Formats's code
        /// </summary>
        [Column(@"code")]
        public string Code { get; set; }

        /// <summary>
        /// Template
        /// </summary>
        [Column(@"template")]
        public string Template { get; set; }
    }
}
