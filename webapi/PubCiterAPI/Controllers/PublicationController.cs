using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
using PubCiterAPI.Repositories;
using PubCiterAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpPost]
        [Route(@"citations/sync")]
        public void Sync()
        {
            //Task.Run(() =>
            //{
                var author = this.authorRepository.GetAuthorInstanceByName(context, AppSettings.Author);

                // if author does not exist, create initial record
                if (author == null)
                {
                    author = this.authorRepository.InitAuthor(context, AppSettings.Author);
                }

            // DEBUG
            //this.publicationRepository.SyncFromSemantics(context, "10.1016/j.sbspro.2015.02.347");
            this.publicationRepository.SyncFromScholar(context, AppSettings.Author);

            if (author != null && author.Settings != null)
                {
                    // synchronize from Google Scholar
                    if (author.Settings.Scholar)
                    {
                        this.publicationRepository.SyncFromScholar(context, author.Name);
                    }

                    // synchronize from Semantics Scholar
                    if (author.Settings.Semantics)
                    {
                        this.publicationRepository.SyncFromSemantics(context);
                    }

                    // synchronize from opencitations.net
                    if (author.Settings.OpenCitations)
                    {
                        this.publicationRepository.SyncFromOpenCitations(context);
                    }
                }
            //});
        }

        [HttpGet]
        [Route(@"sync-status")]
        public bool GetSyncStatus()
        {
            var author = this.authorRepository.GetAuthorInstanceByName(context, AppSettings.Author);
            if (author == null)
            {
                return false;
            }

            return author.Settings.SyncStatus;
        }
    }
}
