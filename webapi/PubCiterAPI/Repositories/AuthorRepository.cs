namespace PubCiterAPI.Repositories
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;

    public class AuthorRepository : IAuthorRepository
    {
        /// <summary>
        /// Finds author by name
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="name">Author's name</param>
        /// <returns>Author instance</returns>
        public Author GetAuthorInstanceByName(ApplicationDbContext context, string name)
        {
            return context.Authors.Include(x => x.Settings).FirstOrDefault(a => a.Name == name);
        }

        /// <summary>
        /// Creates initial record for author
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="author">Author's name</param>
        /// <returns>Author record</returns>
        public Author InitAuthor(ApplicationDbContext context, string author)
        {
            var newAuthor = new Author
            {
                Name = author,
                DisplayName = author,
                Settings = new AuthorSettings
                {
                    SyncStatus = true
                }
            };

            context.Authors.Add(newAuthor);
            context.SaveChanges();

            return newAuthor;
        }
    }
}
