namespace PubCiterAPI.Repositories
{
    using System.Collections.Generic;
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
        public Author GetAuthorByName(ApplicationDbContext context, string name)
        {
            return context.Authors.Include(x => x.Settings).FirstOrDefault(a => a.Name == name || a.DisplayName == name);
        }

        /// <summary>
        /// Finds author by name
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="authorId">Author's ID</param>
        /// <returns>Author instance</returns>
        public Author GetAuthorById(ApplicationDbContext context, long? authorId)
        {
            return context.Authors.Include(x => x.Settings).FirstOrDefault(a => a.AuthorId == authorId);
        }

        /// <summary>
        /// Lists authors
        /// </summary>
        /// <param name="context">Application context</param>
        /// <returns>Collection of authors</returns>
        public IEnumerable<Author> GetAuthors(ApplicationDbContext context)
        {
            return context.Authors.Include(x => x.Settings).ToList();
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
                    Scholar = true
                }
            };

            context.Authors.Add(newAuthor);
            context.SaveChanges();

            return newAuthor;
        }

        /// <summary>
        /// Recounts the total number of citations
        /// </summary>
        /// <param name="context">Application DB context</param>
        public void RecountCitations(ApplicationDbContext context)
        {
            var author = context.Authors.FirstOrDefault(x => x.Name == AppSettings.Author);
            if (author != null)
            {
                author.TotalCites = context.Publications
                                                .Include(x => x.PublicationCitationList)
                                           .Sum(x => x.PublicationCitationList.Count);
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Updates author by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="updatedAuthor">Updated author instance</param>
        /// <returns></returns>
        public Author UpdateAuthor(ApplicationDbContext context, Author updatedAuthor)
        {
            var author = context.Authors.Include(x => x.Settings).FirstOrDefault(x => x.AuthorId == updatedAuthor.AuthorId);
            if (author != null)
            {
                author.DisplayName = updatedAuthor.DisplayName;
                author.Affiliation = updatedAuthor.Affiliation;

                author.Settings.Scholar = updatedAuthor.Settings.Scholar;
                author.Settings.Semantics = updatedAuthor.Settings.Semantics;
                author.Settings.OpenCitations = updatedAuthor.Settings.OpenCitations;
                author.Settings.LibraryTableView = updatedAuthor.Settings.LibraryTableView;
                author.Settings.GraphFontSize = updatedAuthor.Settings.GraphFontSize;

                context.SaveChanges();
            }

            return author;
        }
    }
}
