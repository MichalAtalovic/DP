using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
using PubCiterAPI.Repositories;
using PubCiterAPI.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace PubCiterAPI.Controllers
{
    [ApiController]
    [Route(@"[controller]")]
    public class EnumController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<AuthorController> logger;

        /// <summary>
        /// Enumerator Repository instance
        /// </summary>
        private readonly IEnumRepository enumRepository;

        /// <summary>
        /// Current DB Context
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorRepository">Author repository instance</param>
        /// <param name="context">Application DB context</param>
        /// <param name="logger">Logger instance</param>
        public EnumController(
            EnumRepository enumRepository,
            ApplicationDbContext context,
            ILogger<AuthorController> logger)
        {
            this.context = context;
            this.enumRepository = enumRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gets list of export formats
        /// </summary>
        [HttpGet]
        [Route(@"exportFormat")]
        [SwaggerOperation(Summary = "Gets list of export formats")]
        public IEnumerable<ExportFormat> ListExportFormats()
        {
            try
            {
                return this.enumRepository.GetListOfExportFormats(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Removes export format by ID
        /// </summary>
        [HttpDelete]
        [Route(@"exportFormat/{id}")]
        [SwaggerOperation(Summary = "Removes export format by ID")]
        public void DeleteExportFormat(long id)
        {
            try
            {
                this.enumRepository.RemoveExportFormat(context, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Inserts or updates incoming export format
        /// </summary>
        [HttpPut]
        [Route(@"exportFormat")]
        [SwaggerOperation(Summary = "Inserts or updates incoming export format")]
        public ExportFormat PutExportFormat(ExportFormat exportFormat)
        {
            try
            {
                return this.enumRepository.InsertUpdateExportFormat(context, exportFormat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Gets list of publication categories
        /// </summary>
        [HttpGet]
        [Route(@"publicationCategory")]
        [SwaggerOperation(Summary = "Gets list of publication categories")]
        public IEnumerable<PublicationCategory> ListPublicationCategories()
        {
            try
            {
                return this.enumRepository.GetListOfPublicationCategories(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Removes publication category by ID
        /// </summary>
        [HttpDelete]
        [Route(@"publicationCategory/{id}")]
        [SwaggerOperation(Summary = "Removes publication category by ID")]
        public void DeletePublicationCategory(long id)
        {
            try
            {
                this.enumRepository.RemovePublicationCategory(context, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Inserts or updates incoming publication category
        /// </summary>
        [HttpPut]
        [Route(@"publicationCategory")]
        [SwaggerOperation(Summary = "Inserts or updates incoming publication category")]
        public PublicationCategory PutPublicationCategory(PublicationCategory publicationCategory)
        {
            try
            {
                return this.enumRepository.InsertUpdatePublicationCategory(context, publicationCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Gets list of citation categories
        /// </summary>
        [HttpGet]
        [Route(@"citationCategory")]
        [SwaggerOperation(Summary = "Gets list of citation categories")]
        public IEnumerable<CitationCategory> ListCitationCategories()
        {
            try
            {
                return this.enumRepository.GetListOfCitationCategories(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Removes citation category by ID
        /// </summary>
        [HttpDelete]
        [Route(@"citationCategory/{id}")]
        [SwaggerOperation(Summary = "Removes citation category by ID")]
        public void DeleteCitationCategory(long id)
        {
            try
            {
                this.enumRepository.RemoveCitationCategory(context, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Inserts or updates incoming citation category
        /// </summary>
        [HttpPut]
        [Route(@"citationCategory")]
        [SwaggerOperation(Summary = "Inserts or updates incoming citation category")]
        public CitationCategory PutCitationCategory(CitationCategory citationCategory)
        {
            try
            {
                return this.enumRepository.InsertUpdateCitationCategory(context, citationCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }
    }
}
