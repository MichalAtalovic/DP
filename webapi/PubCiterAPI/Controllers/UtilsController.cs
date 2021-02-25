using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PubCiterAPI.Model;
using PubCiterAPI.Repositories;
using PubCiterAPI.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PubCiterAPI.Model.Utils;

namespace PubCiterAPI.Controllers
{
    [ApiController]
    [Route(@"[controller]")]
    public class UtilsController : ControllerBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<PublicationController> _logger;

        /// <summary>
        /// Current DB Context
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Conversion Repository instance
        /// </summary>
        private readonly IUtilsRepository _utilsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorRepository">Author repository instance</param>
        /// <param name="publicationRepository">Publication repository instance</param>
        /// <param name="context">Application DB context</param>
        /// <param name="logger">Logger instance</param>
        public UtilsController(
            UtilsRepository utilsRepository,
            ApplicationDbContext context,
            ILogger<PublicationController> logger)
        {
            this._context = context;
            this._utilsRepository = utilsRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Converts bibliographical record to other format
        /// </summary>
        /// <param name="convertFrom"></param>
        /// <param name="convertTo"></param>
        [HttpPost]
        [Route(@"convert")]
        [SwaggerOperation(Summary = @"Converts bibliographical record to other format")]
        public async Task<string> Convert(ConversionTypeEnum convertFrom, ConversionTypeEnum convertTo)
        {
            try
            {
                var reader = new StreamReader(Request.Body, Encoding.UTF8);
                return this._utilsRepository.Convert(await reader.ReadToEndAsync(), convertFrom, convertTo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this._logger.LogError(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Formats BibTex entry
        /// </summary>
        /// <param name="inputType">Data type of input</param>
        /// <param name="formatAs">Format as</param>
        [HttpPost]
        [Route(@"format")]
        [SwaggerOperation(Summary = @"Formats BibTex entry")]
        public async Task<string> Format(ConversionTypeEnum inputType, FormatEnum formatAs)
        {
            try
            {
                var reader = new StreamReader(Request.Body, Encoding.UTF8);
                return this._utilsRepository.Format(await reader.ReadToEndAsync(), inputType, formatAs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this._logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
