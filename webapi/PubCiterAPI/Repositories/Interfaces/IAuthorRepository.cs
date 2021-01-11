namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;

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
        public Author GetAuthorInstanceByName(ApplicationDbContext context, string name);

        /// <summary>
        /// Creates initial record for author
        /// </summary>
        /// <param name="context">Application context</param>
        /// <param name="author">Author's name</param>
        /// <returns>Author record</returns>
        Author InitAuthor(ApplicationDbContext context, string author);
    }
}
