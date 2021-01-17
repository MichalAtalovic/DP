using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
using PubCiterAPI.Repositories;
using PubCiterAPI.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace PubCiterAPI.Controllers
{
    [ApiController]
    [Route(@"[controller]")]
    public class SettingsController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<PublicationController> logger;

        /// <summary>
        /// Settings Repository instance
        /// </summary>
        private readonly ISettingsRepository settingsRepository;

        /// <summary>
        /// Current DB Context
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settingsRepository">Settings repository instance</param>
        /// <param name="context">Application DB context</param>
        /// <param name="logger">Logger instance</param>
        public SettingsController(
            SettingsRepository settingsRepository,
            ApplicationDbContext context,
            ILogger<PublicationController> logger)
        {
            this.context = context;
            this.settingsRepository = settingsRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Updates settings of author by their ID
        /// </summary>
        [HttpPut]
        [Route(@"{authorId}")]
        [SwaggerOperation(Summary = "Updates author's settings by their ID")]
        public void UpdateSettings(AuthorSettings settings, [Required] [SwaggerParameter(Description = @"Author's unique identifier")] long authorId)
        {
            try
            {
                this.settingsRepository.UpdateSettings(context, settings, authorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }
    }
}
