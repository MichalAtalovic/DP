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

        public PublicationCategory InsertUpdatePublicationCategory(ApplicationDbContext context, PublicationCategory publicationCategory)
        {
            // new item
            if (publicationCategory.Id == 0)
            {
                context.PublicationCategories.Add(publicationCategory);
                context.SaveChanges();

                return publicationCategory;
            }
            // update
            else
            {
                var toUpdate = context.PublicationCategories.FirstOrDefault(x => x.Id == publicationCategory.Id);
                if (toUpdate != null)
                {
                    toUpdate.Code = publicationCategory.Code;
                    toUpdate.Group = publicationCategory.Group;
                    toUpdate.Name = publicationCategory.Name;
                    context.SaveChanges();

                    return toUpdate;
                }

                return null;
            }
        }

        public CitationCategory InsertUpdateCitationCategory(ApplicationDbContext context, CitationCategory citationCategory)
        {
            // new item
            if (citationCategory.Id == 0)
            {
                context.CitationCategories.Add(citationCategory);
                context.SaveChanges();

                return citationCategory;
            }
            // update
            else
            {
                var toUpdate = context.CitationCategories.FirstOrDefault(x => x.Id == citationCategory.Id);
                if (toUpdate != null)
                {
                    toUpdate.Code = citationCategory.Code;
                    toUpdate.Name = citationCategory.Name;
                    context.SaveChanges();

                    return toUpdate;
                }

                return null;
            }
        }

        /// <summary>
        /// Removes export format by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="id">Export format ID</param>
        public void RemoveExportFormat(ApplicationDbContext context, long id)
        {
            var toDelete = context.ExportFormats.ToList().FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                context.ExportFormats.Remove(toDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes publication category by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="id">Publication category ID</param>
        public void RemovePublicationCategory(ApplicationDbContext context, long id)
        {
            var toDelete = context.PublicationCategories.ToList().FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                context.PublicationCategories.Remove(toDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes citation category by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="id">Citation category ID</param>
        public void RemoveCitationCategory(ApplicationDbContext context, long id)
        {
            var toDelete = context.CitationCategories.ToList().FirstOrDefault(x => x.Id == id);
            if (toDelete != null)
            {
                context.CitationCategories.Remove(toDelete);
                context.SaveChanges();
            }
        }
    }
}
