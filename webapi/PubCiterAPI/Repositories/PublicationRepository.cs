namespace PubCiterAPI.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// Publication Repository
    /// </summary>
    public class PublicationRepository : IPublicationRepository
    {
        /// <summary>
        /// Gets publications of author by their name
        /// </summary>
        /// <param name="name">Author's name</param>
        /// <returns>Collection of publications</returns>
        public IEnumerable<Publication> GetPublications(ApplicationDbContext context, string name)
        {
            return context.Publications
                .Include(x => x.PublicationCitationList)
                .ThenInclude(x => x.Citation);
        }

        public void SyncFromOpenCitations(ApplicationDbContext context, string name)
        {
            throw new System.NotImplementedException();
        }

        public void SyncFromScholar(ApplicationDbContext context, string name)
        {
            throw new System.NotImplementedException();
        }

        public void SyncFromSemantics(ApplicationDbContext context, string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
