namespace PubCiterAPI.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;

    public class EnumRepository : IEnumRepository
    {
        public IEnumerable<CitationCategory> GetListOfCitationCategories(ApplicationDbContext context)
        {
            return context.CitationCategories.ToList();
        }

        public IEnumerable<ExportFormat> GetListOfExportFormats(ApplicationDbContext context)
        {
            return context.ExportFormats.ToList();
        }

        public IEnumerable<PublicationCategory> GetListOfPublicationCategories(ApplicationDbContext context)
        {
            return context.PublicationCategories.ToList();
        }

        public ExportFormat InsertUpdateExportFormat(ApplicationDbContext context, ExportFormat format)
        {
            // new item
            if (format.Id == 0)
            {
                context.ExportFormats.Add(format);
                context.SaveChanges();

                return format;
            }
            // update
            else
            {
                var toUpdate = context.ExportFormats.FirstOrDefault(x => x.Id == format.Id);
                if (toUpdate != null)
                {
                    toUpdate.Code = format.Code;
                    toUpdate.Template = format.Template;
                    context.SaveChanges();

                    return toUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Removes citation by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publicationId">Publication ID</param>
        /// <param name="citationId">Citation ID</param>
        public void RemoveExportFormat(ApplicationDbContext context, long id)
        {
            var toDelete = context.ExportFormats.ToList().FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                context.ExportFormats.Remove(toDelete);
                context.SaveChanges();
            }
        }
    }
}
