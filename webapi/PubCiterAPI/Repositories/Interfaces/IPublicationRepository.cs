namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;
    using System.Collections.Generic;

    /// <summary>
    /// PublicationRepository Interface
    /// </summary>
    public interface IPublicationRepository
    {
        /// <summary>
        /// Gets publications of author by their name
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="searchText">Searched string</param>
        /// <returns>Collection of publications</returns>
        public IEnumerable<Publication> GetPublications(ApplicationDbContext context, string searchText);

        /// <summary>
        /// Synchronizes database of publications and their citations
        /// from Google Scholar
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="name">Author's name</param>
        public void SyncFromScholar(ApplicationDbContext context, string name);

        /// <summary>
        /// Synchronizes database of publications and their citations
        /// from Semantics Scholar
        /// </summary>
        /// <param name="context">Application DB context</param>
        public void SyncFromSemantics(ApplicationDbContext context);

        /// <summary>
        /// Synchronizes database of publications and their citations
        /// from opencitations.net
        /// </summary>
        /// <param name="context">Application DB context</param>
        public void SyncFromOpenCitations(ApplicationDbContext context);

        /// <summary>
        /// Updates publication's DOI by publication ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publicationId">Publication ID</param>
        /// <param name="doi">DOI string value</param>
        public void UpdatePublicationDoi(ApplicationDbContext context, long publicationId, string doi);

        /// <summary>
        /// Inserts publication to the library
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publication">Publication object</param>
        void InsertPublication(ApplicationDbContext context, Publication publication);

        /// <summary>
        /// Updates or inserts publication in/to the library
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publication">Publication object</param>
        void UpdateInsertPublication(ApplicationDbContext context, Publication publication);
    }
}
