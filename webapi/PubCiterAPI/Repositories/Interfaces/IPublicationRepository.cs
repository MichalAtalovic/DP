namespace PubCiterAPI.Repositories.Interfaces
{
    using Model;
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
        /// from www.OpenCitations.net
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

        /// <summary>
        /// Insert citation to publication
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="citation">Citation object</param>
        /// <param name="publicationId">Publication ID</param>
        void InsertCitation(ApplicationDbContext context, Citation citation, long publicationId);

        /// <summary>
        /// Removes citation by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publicationId">Publication ID</param>
        /// <param name="citationId">Citation ID</param>
        void RemoveCitation(ApplicationDbContext context, long publicationId, long citationId);

        /// <summary>
        /// Lists citations by specific filter
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="yearFrom">Year from to filter by</param>
        /// <param name="yearTo">Year to to filter by</param>
        /// <param name="publicationCategories">Publication categories to filter by</param>
        /// <param name="citationCategories">Citation categories to filter by</param>
        List<KeyValuePair<Publication, Citation>> ReportCitations(ApplicationDbContext context, long? yearFrom, long? yearTo, List<string> publicationCategories, List<long> citationCategories);
    }
}
