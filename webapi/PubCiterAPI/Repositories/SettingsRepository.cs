namespace PubCiterAPI.Repositories
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;

    public class SettingsRepository : ISettingsRepository
    {
        /// <summary>
        /// Updates settings of author defined in webconfig
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="settings">Author's settings</param>
        /// <param name="authorId">Author's ID</param>
        public void UpdateSettings(ApplicationDbContext context, AuthorSettings settings, long authorId)
        {
            var author = context.Authors.Include(x => x.Settings).ToList().FirstOrDefault(x => x.AuthorId == authorId);
            if (author != null)
            {
                author.Settings.Scholar = settings.Scholar;
                author.Settings.Semantics = settings.Semantics;
                author.Settings.OpenCitations = settings.OpenCitations;

                context.SaveChanges();
            }
        }
    }
}
