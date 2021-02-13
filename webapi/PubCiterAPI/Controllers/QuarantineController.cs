using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
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
        private readonly ILogger<QuarantineController> _logger;

        /// <summary>
        /// Author Repository instance
        /// </summary>
        private readonly IQuarantineRepository _quarantineRepository;

        /// <summary>
        /// Author Repository instance
        /// </summary>
        private readonly IAuthorRepository _authorRepository;

        /// <summary>
        /// Current DB Context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorRepository">Author repository instance</param>
        /// <param name="quarantineRepository">Quarantine repository instance</param>
        /// <param name="context">Application DB context</param>
        /// <param name="logger">Logger instance</param>
        public QuarantineController(
            QuarantineRepository quarantineRepository,
            AuthorRepository authorRepository,
            ApplicationDbContext context,
            ILogger<QuarantineController> logger)
        {
            this._context = context;
            this._quarantineRepository = quarantineRepository;
            this._authorRepository = authorRepository;
            this._logger = logger;
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
                this._quarantineRepository.MoveToQuarantine(_context, publicationId);
                this._authorRepository.RecountCitations(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this._logger.LogError(ex.ToString());
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
                this._quarantineRepository.RemoveFromQuarantine(_context, quarantinedPublicationId);
                this._authorRepository.RecountCitations(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this._logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Moves all quarantined publications to library
        /// </summary>
        [HttpPost]
        [Route(@"clear")]
        [SwaggerOperation(Summary = "Moves all quarantined publications to library")]
        public void ClearQuarantine()
        {
            try
            {
                this._quarantineRepository.ClearQuarantine(_context);
                this._authorRepository.RecountCitations(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this._logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the list of publication in quarantine
        /// </summary>
        [HttpGet]
        [Route(@"")]
        [SwaggerOperation(Summary = "Gets the list of publication in quarantine")]
        public IEnumerable<QuarantinedPublication> GetQuarantineList([SwaggerParameter(Description = @"If set, citations will be appended to response")] bool citations)
        {
            try
            {
                return this._quarantineRepository.GetQuarantineList(_context, citations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this._logger.LogError(ex.ToString());
            }

            return null;
        }
    }
}
