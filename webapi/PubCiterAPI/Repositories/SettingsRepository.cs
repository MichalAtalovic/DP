namespace PubCiterAPI.Repositories
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;

    public class SettingsRepository : ISettingsRepository
    {
        /// <summary>
        /// Drops and re-initializes database
        /// </summary>
        public void HardReset()
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"cmd.exe",
                    Arguments = @$"/c cd scripts && reinit_db.bat",
                    RedirectStandardError = false,
                    RedirectStandardOutput = false
                }
            };

            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Updates settings of author defined in webconfig
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="settings">Author's settings</param>
        /// <param name="authorId">Author's ID</param>
        public void UpdateSettings(ApplicationDbContext context, AuthorSettings settings, long authorId)
        {
            var author = context.Authors.Include(x => x.Settings).ToList().FirstOrDefault(x => x.AuthorId == authorId);
            if (author == null)
            {
                return;
            }

            author.Settings.Scholar = settings.Scholar;
            author.Settings.Semantics = settings.Semantics;
            author.Settings.OpenCitations = settings.OpenCitations;
            author.Settings.LibraryTableView = settings.LibraryTableView;
            author.Settings.GraphFontSize = settings.GraphFontSize;

            context.SaveChanges();
        }
    }
}
