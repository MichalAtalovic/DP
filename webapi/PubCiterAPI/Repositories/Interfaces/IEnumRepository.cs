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
        /// Gets list of citation categories
        /// </summary>
        /// <returns>List of citation categories</returns>
        public IEnumerable<CitationCategory> GetListOfCitationCategories(ApplicationDbContext context);

        /// <summary>
        /// Removes export format by ID
        /// </summary>
        /// <param name="context">Application DB Context</param>
        /// <param name="id">Export format ID</param>
        public void RemoveExportFormat(ApplicationDbContext context, long id);
    }
}
