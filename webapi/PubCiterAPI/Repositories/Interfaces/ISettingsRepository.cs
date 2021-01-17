namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;

    /// <summary>
    /// SettingsRepository Interface
    /// </summary>
    public interface ISettingsRepository
    {
        /// <summary>
        /// Updates settings of author defined in webconfig
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="settings">Author settings</param>
        /// <param name="authorId">Author's ID</param>
        public void UpdateSettings(ApplicationDbContext context, AuthorSettings settings, long authorId);
    }
}
