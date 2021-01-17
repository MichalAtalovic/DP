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
    public class AuthorController : ControllerBase
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
        /// Current DB Context
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorRepository">Author repository instance</param>
        /// <param name="context">Application DB context</param>
        /// <param name="logger">Logger instance</param>
        public AuthorController(
            AuthorRepository authorRepository,
            ApplicationDbContext context,
            ILogger<PublicationController> logger)
        {
            this.context = context;
            this.authorRepository = authorRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gets list of authors
        /// </summary>
        [HttpGet]
        [Route(@"")]
        [SwaggerOperation(Summary = "Gets list of authors")]
        public IEnumerable<Author> ListAuthors([SwaggerParameter(Description = @"Author's name")] string name, [SwaggerParameter(Description = @"Author's unique identifier")] long? authorId)
        {
            try
            {
                if (authorId != null)
                {
                    return new List<Author> { this.authorRepository.GetAuthorById(context, authorId) };
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    return new List<Author> { this.authorRepository.GetAuthorByName(context, name) };
                }
                else
                {
                    return this.authorRepository.GetAuthors(context);
                }
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
