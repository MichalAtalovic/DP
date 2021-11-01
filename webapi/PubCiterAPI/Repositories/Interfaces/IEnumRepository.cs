namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;
    using System.Collections.Generic;

    /// <summary>
    /// EnumRepository Interface
    /// </summary>
    public interface IEnumRepository
    {
        /// <summary>
        /// Gets list of export formats
        /// </summary>
        /// <returns>List of export formats</returns>
        public IEnumerable<ExportFormat> GetListOfExportFormats(ApplicationDbContext context);

        /// <summary>
        /// Inserts or updates incoming export format
        /// </summary>
        public ExportFormat InsertUpdateExportFormat(ApplicationDbContext context, ExportFormat format);

        /// <summary>
        /// Gets list of publication categories
        /// </summary>
        /// <returns>List of publication categories</returns>
        public IEnumerable<PublicationCategory> GetListOfPublicationCategories(ApplicationDbContext context);

        /// <summary>
        /// Inserts or updates incoming publication category
        /// </summary>
        public PublicationCategory InsertUpdatePublicationCategory(ApplicationDbContext context, PublicationCategory publicationCategory);

        /// <summary>
        /// Gets list of citation categories
        /// </summary>
        /// <returns>List of citation categories</returns>
        public IEnumerable<CitationCategory> GetListOfCitationCategories(ApplicationDbContext context);

        /// <summary>
        /// Inserts or updates incoming citation category
        /// </summary>
        public CitationCategory InsertUpdateCitationCategory(ApplicationDbContext context, CitationCategory citationCategory);

        /// <summary>
        /// Removes export format by ID
        /// </summary>
        /// <param name="context">Application DB Context</param>
        /// <param name="id">Export format ID</param>
        public void RemoveExportFormat(ApplicationDbContext context, long id);

        /// <summary>
        /// Removes publication category by ID
        /// </summary>
        /// <param name="context">Application DB Context</param>
        /// <param name="id">Publication category ID</param>
        public void RemovePublicationCategory(ApplicationDbContext context, long id);

        /// <summary>
        /// Removes citation category by ID
        /// </summary>
        /// <param name="context">Application DB Context</param>
        /// <param name="id">Citation category ID</param>
        public void RemoveCitationCategory(ApplicationDbContext context, long id);
    }
}
