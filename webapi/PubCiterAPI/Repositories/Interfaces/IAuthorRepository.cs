namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;
    using System.Collections.Generic;

    /// <summary>
    /// AuthorRepository Interface
    /// </summary>
    public interface IAuthorRepository
    {
        /// <summary>
        /// Finds author by name
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="name">Author's name</param>
        /// <returns>Author instance</returns>
        public Author GetAuthorByName(ApplicationDbContext context, string name);

        /// <summary>
        /// Finds author by ID
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="authorId">Author's ID</param>
        /// <returns>Author instance</returns>
        public Author GetAuthorById(ApplicationDbContext context, long? authorId);

        /// <summary>
        /// Updates author by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="author">Updated author instancce</param>
        /// <returns>Author instance</returns>
        public Author UpdateAuthor(ApplicationDbContext context, Author author);

        /// <summary>
        /// Lists authors
        /// </summary>
        /// <param name="context">Application context</param>
        /// <returns>Collection of authors</returns>
        public IEnumerable<Author> GetAuthors(ApplicationDbContext context);

        /// <summary>
        /// Creates initial record for author
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="author">Author's name</param>
        /// <returns>Author record</returns>
        public Author InitAuthor(ApplicationDbContext context, string author);

        /// <summary>
        /// Re-counts overall author's citations
        /// </summary>
        /// <param name="context">Application context</param>
        public void RecountCitations(ApplicationDbContext context);
    }
}
