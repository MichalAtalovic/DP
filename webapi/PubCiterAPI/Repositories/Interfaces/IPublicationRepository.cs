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
        /// <param name="name">Author's name</param>
        /// <returns>Collection of publications</returns>
        public IEnumerable<Publication> GetPublications(ApplicationDbContext context, string name);

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
        /// <param name="name">Author's name</param>
        public void SyncFromSemantics(ApplicationDbContext context, string name);

        /// <summary>
        /// Synchronizes database of publications and their citations
        /// from opencitations.net
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="name">Author's name</param>
        public void SyncFromOpenCitations(ApplicationDbContext context, string name);
    }
}
