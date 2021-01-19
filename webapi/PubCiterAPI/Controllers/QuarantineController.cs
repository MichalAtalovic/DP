using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
using PubCiterAPI.Model.SyncState;
using PubCiterAPI.Repositories;
using PubCiterAPI.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PubCiterAPI.Controllers
{
    [ApiController]
    [Route(@"[controller]")]
    public class QuarantineController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<QuarantineController> logger;

        /// <summary>
        /// Author Repository instance
        /// </summary>
        private readonly IQuarantineRepository quarantineRepository;

        /// <summary>
        /// Publication Repository instance
        /// </summary>
        private readonly IPublicationRepository publicationRepository;

        /// <summary>
        /// Current DB Context
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorRepository">Author repository instance</param>
        /// <param name="publicationRepository">Publication repository instance</param>
        /// <param name="context">Application DB context</param>
        /// <param name="logger">Logger instance</param>
        public QuarantineController(
            QuarantineRepository quarantineRepository,
            PublicationRepository publicationRepository,
            ApplicationDbContext context,
            ILogger<QuarantineController> logger)
        {
            this.context = context;
            this.quarantineRepository = quarantineRepository;
            this.publicationRepository = publicationRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Moves publication to quarantine
        /// </summary>
        [HttpPost]
        [Route(@"add/{publicationId}")]
        [SwaggerOperation(Summary = "Moves publication to quarantine")]
        public void MovePublicationToQuarantine([Required][SwaggerParameter(Description = @"Unique publication's system identifier")] long publicationId)
        {
            try
            {
                this.quarantineRepository.MoveToQuarantine(context, publicationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Removes publication from quarantine
        /// </summary>
        [HttpPost]
        [Route(@"remove/{quarantinedPublicationId}")]
        [SwaggerOperation(Summary = "Removes publication from quarantine")]
        public void RemovePublicationFromQuarantine([Required][SwaggerParameter(Description = @"Unique quarantined publication's system identifier")] long quarantinedPublicationId)
        {
            try
            {
                this.quarantineRepository.RemoveFromQuarantine(context, quarantinedPublicationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the list of publication in quarantine
        /// </summary>
        [HttpGet]
        [Route(@"")]
        [SwaggerOperation(Summary = "Gets the list of publication in quarantine")]
        public IEnumerable<QuarantinedPublication> GetQuarantineList([SwaggerParameter(Description = @"If set, citations will be appended to response")]bool citations)
        {
            try
            {
                return this.quarantineRepository.GetQuarantineList(context, citations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }

            return null;
        }
    }
}
