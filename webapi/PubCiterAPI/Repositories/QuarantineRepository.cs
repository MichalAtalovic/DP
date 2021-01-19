namespace PubCiterAPI.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;

    /// <summary>
    /// Quarantine Repository
    /// </summary>
    public class QuarantineRepository : IQuarantineRepository
    {
        /// <summary>
        /// Gets the list of quarantined publiations
        /// </summary>
        /// <param name="context">Application DB context instance</param>
        /// <param name="appendCitations">Whether to append citations to response</param>
        public IEnumerable<QuarantinedPublication> GetQuarantineList(ApplicationDbContext context, bool appendCitations)
        {
            return appendCitations ?
                context.QuarantinedPublications
                            .Include(x => x.QuarantinedPublicationCitationList)
                            .ThenInclude(x => x.Citation).ToList()
                                    : context.QuarantinedPublications.ToList();
        }

        /// <summary>
        /// Moves publication to quarantine by ID
        /// </summary>
        /// <param name="context">Application DB context instance</param>
        /// <param name="publicationId">Unique publication's system identifier</param>
        public void MoveToQuarantine(ApplicationDbContext context, long publicationId)
        {
            var publication = context.Publications.Include(x => x.PublicationCitationList).ThenInclude(x => x.Citation).ToList().FirstOrDefault(x => x.PublicationId == publicationId);
            if (publication != null)
            {
                var quarantinedPublication = new QuarantinedPublication
                {
                    AuthorId = publication.AuthorId,
                    Title = publication.Title,
                    PublicationYear = publication.PublicationYear,
                    Authors = publication.Authors,
                    Journal = publication.Journal,
                    JournalVolume = publication.JournalVolume,
                    Pages = publication.Pages,
                    Publisher = publication.Publisher,
                    Abstract = publication.Abstract,
                    Doi = publication.Doi,
                    QuarantinedPublicationCitationList = new List<QuarantinedPublicationCitation>()
                };

                // append citations
                foreach (var publicationCitation in publication.PublicationCitationList)
                {
                    quarantinedPublication.QuarantinedPublicationCitationList.Add(new QuarantinedPublicationCitation
                    {
                        Citation = new Citation
                        {
                            Title = publicationCitation.Citation.Title,
                            Author = publicationCitation.Citation.Author,
                            PublicationYear = publicationCitation.Citation.PublicationYear,
                            Journal = publicationCitation.Citation.Journal,
                            JournalVolume = publicationCitation.Citation.JournalVolume,
                            Abstract = publicationCitation.Citation.Abstract,
                            PublicationUrl = publicationCitation.Citation.PublicationUrl,
                            Doi = publicationCitation.Citation.Doi,
                        }
                    });
                }

                // add publication to quarantine
                context.QuarantinedPublications.Add(quarantinedPublication);

                // remove publication from the library
                context.Publications.Remove(publication);

                // save changes
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes publication from quarantine by quarantined publication ID
        /// </summary>
        /// <param name="context">Application DB context instance</param>
        /// <param name="publicationId">Unique quarantined publication's system identifier</param>
        public void RemoveFromQuarantine(ApplicationDbContext context, long quarantinedPublicationId)
        {
            var quarantinedPublication = context.QuarantinedPublications.Include(x => x.QuarantinedPublicationCitationList).ThenInclude(x => x.Citation).ToList().FirstOrDefault(x => x.QuarantinedPublicationId == quarantinedPublicationId);
            if (quarantinedPublication != null)
            {
                var publication = new Publication
                {
                    AuthorId = quarantinedPublication.AuthorId,
                    Title = quarantinedPublication.Title,
                    PublicationYear = quarantinedPublication.PublicationYear,
                    Authors = quarantinedPublication.Authors,
                    Journal = quarantinedPublication.Journal,
                    JournalVolume = quarantinedPublication.JournalVolume,
                    Pages = quarantinedPublication.Pages,
                    Publisher = quarantinedPublication.Publisher,
                    Abstract = quarantinedPublication.Abstract,
                    Doi = quarantinedPublication.Doi,
                    PublicationCitationList = new List<PublicationCitation>()
                };

                // append citations
                foreach (var quarantinedPublicationCitation in quarantinedPublication.QuarantinedPublicationCitationList)
                {
                    publication.PublicationCitationList.Add(new PublicationCitation
                    {
                        Citation = new Citation
                        {
                            Title = quarantinedPublicationCitation.Citation.Title,
                            Author = quarantinedPublicationCitation.Citation.Author,
                            PublicationYear = quarantinedPublicationCitation.Citation.PublicationYear,
                            Journal = quarantinedPublicationCitation.Citation.Journal,
                            JournalVolume = quarantinedPublicationCitation.Citation.JournalVolume,
                            Abstract = quarantinedPublicationCitation.Citation.Abstract,
                            PublicationUrl = quarantinedPublicationCitation.Citation.PublicationUrl,
                            Doi = quarantinedPublicationCitation.Citation.Doi,
                        }
                    });
                }

                // add publication to library
                context.Publications.Add(publication);

                // remove publication from the quarantine
                context.QuarantinedPublications.Remove(quarantinedPublication);

                // save changes
                context.SaveChanges();
            }
        }
    }
}
