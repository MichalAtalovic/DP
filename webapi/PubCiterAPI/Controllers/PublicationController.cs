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
    public class PublicationController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<PublicationController> logger;

        /// <summary>
        /// Author Repository instance
        /// </summary>
        private readonly IAuthorRepository authorRepository;

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
        public PublicationController(
            AuthorRepository authorRepository,
            PublicationRepository publicationRepository,
            ApplicationDbContext context,
            ILogger<PublicationController> logger)
        {
            this.context = context;
            this.authorRepository = authorRepository;
            this.publicationRepository = publicationRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Synchronizes data for author defined in webconfig
        /// </summary>
        [HttpPost]
        [Route(@"citations/sync")]
        [SwaggerOperation(Summary = "Synchronizes data for author defined in webconfig")]
        public void Sync()
        {
            try
            {
                // find author
                var author = this.authorRepository.GetAuthorByName(context, AppSettings.Author);

                // if author does not exist, create initial record
                if (author == null)
                {
                    author = this.authorRepository.InitAuthor(context, AppSettings.Author);
                }

                if (author != null && author.Settings != null)
                {
                    // synchronize from Google Scholar
                    if (author.Settings.Scholar)
                    {
                        CurrentSyncState.GoogleScholar = SyncStateEnum.Running;
                        CurrentSyncState.SemanticsScholar = SyncStateEnum.Pending;
                        CurrentSyncState.OpenCitations = SyncStateEnum.Pending;
                        this.publicationRepository.SyncFromScholar(context, author.Name);
                        CurrentSyncState.GoogleScholar = SyncStateEnum.Done;
                    }
                    else
                    {
                        CurrentSyncState.GoogleScholar = SyncStateEnum.Skipped;
                    }

                    // synchronize from Semantics Scholar
                    if (author.Settings.Semantics)
                    {
                        CurrentSyncState.SemanticsScholar = SyncStateEnum.Running;
                        CurrentSyncState.OpenCitations = SyncStateEnum.Pending;
                        this.publicationRepository.SyncFromSemantics(context);
                        CurrentSyncState.SemanticsScholar = SyncStateEnum.Done;
                    }
                    else
                    {
                        CurrentSyncState.SemanticsScholar = SyncStateEnum.Skipped;
                    }

                    // synchronize from opencitations.net
                    if (author.Settings.OpenCitations)
                    {
                        CurrentSyncState.OpenCitations = SyncStateEnum.Running;
                        this.publicationRepository.SyncFromOpenCitations(context);
                        CurrentSyncState.OpenCitations = SyncStateEnum.Done;
                    }
                    else
                    {
                        CurrentSyncState.OpenCitations = SyncStateEnum.Skipped;
                    }
                }

                // re-count overall citations
                this.authorRepository.RecountCitations(context);

                // Reset states of synchronization
                CurrentSyncState.GoogleScholar = SyncStateEnum.Idle;
                CurrentSyncState.SemanticsScholar = SyncStateEnum.Idle;
                CurrentSyncState.OpenCitations = SyncStateEnum.Idle;
            }
            catch (Exception ex)
            {
                // Reset states of synchronization
                CurrentSyncState.GoogleScholar = SyncStateEnum.Idle;
                CurrentSyncState.SemanticsScholar = SyncStateEnum.Idle;
                CurrentSyncState.OpenCitations = SyncStateEnum.Idle;

                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Updates publication's DOI by publication ID
        /// </summary>
        /// <param name="publicationId">Publication ID</param>
        /// <param name="doi">DOI stringvalue</param>
        [HttpPut]
        [Route(@"{publicationId}")]
        [SwaggerOperation(Summary = "Updates publication's DOI by publication ID")]
        public void UpdatePublicationDoi([SwaggerParameter(Description = @"Unique publication's identifier")] long publicationId, [Required][SwaggerParameter(Description = @"Digital object identifier to be applied")] string doi)
        {
            try
            {
                this.publicationRepository.UpdatePublicationDoi(context, publicationId, doi);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }
        
        /// <summary>
        /// Gets synchronization status
        /// </summary>
        /// <returns>Synchronization status</returns>
        [HttpGet]
        [Route(@"sync/status")]
        [SwaggerOperation(Summary = "Gets synchronization status")]
        public SyncState GetSyncState()
        {
            return new SyncState
            {
                GoogleScholar = CurrentSyncState.GoogleScholar,
                SemanticsScholar = CurrentSyncState.SemanticsScholar,
                OpenCitations = CurrentSyncState.OpenCitations
            };
        }

        /// <summary>
        /// Gets list of publications by author's name defined in webconfig
        /// </summary>
        /// <returns>Collection of publications</returns>
        [HttpGet]
        [Route(@"")]
        [SwaggerOperation(Summary = "Gets list of publications by author's name defined in webconfig")]
        public IEnumerable<Publication> GetPublications([SwaggerParameter(Description = @"String input for fulltext search query")]string searchText = null)
        {
            try
            {
                return this.publicationRepository.GetPublications(context, searchText);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());

                return null;
            }
        }

        /// <summary>
        /// Adds publication to the library
        /// </summary>
        [HttpPost]
        [Route(@"")]
        [SwaggerOperation(Summary = "Adds publication to the library")]
        public void InsertPublication(Publication publication)
        {
            try
            {
                this.publicationRepository.InsertPublication(context, publication);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Updates or inserts publication in/to the library
        /// </summary>
        [HttpPut]
        [Route(@"")]
        [SwaggerOperation(Summary = "Updates publication in the library")]
        public void UpdateInsertPublication(Publication publication)
        {
            try
            {
                this.publicationRepository.UpdateInsertPublication(context, publication);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Adds citation to publication
        /// </summary>
        [HttpPost]
        [Route(@"{publicationId}/citation")]
        [SwaggerOperation(Summary = "Adds citation to publication")]
        public void InsertCitation(Citation citation, long publicationId)
        {
            try
            {
                this.publicationRepository.InsertCitation(context, citation, publicationId);
                this.authorRepository.RecountCitations(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.logger.LogError(ex.ToString());
            }
        }
    }
}
