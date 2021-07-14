using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PubCiterAPI.Model;
using PubCiterAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace PubCiterAPI.Repositories
{
    /// <summary>
    /// Publication Repository
    /// </summary>
    public class PublicationRepository : IPublicationRepository
    {
        /// <summary>
        /// Gets publications of author (fulltext search option)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="searchedText"></param>
        /// <returns></returns>
        public IEnumerable<Publication> GetPublications(ApplicationDbContext context, string searchedText)
        {
            var author = context.Authors.ToList().FirstOrDefault(x => x.Name == AppSettings.Author);
            if (author == null)
            {
                return null;
            }


            var list = context.Publications
                .Include(x => x.PublicationCitationList)
                .ThenInclude(x => x.Citation).ToList().Where(x => x.AuthorId == author.AuthorId);

            if (string.IsNullOrEmpty(searchedText))
            {
                return list;
            }
            else
            {
                var filteredList = new List<Publication>();
                foreach (var publication in list)
                {
                    if (publication?.Journal != null)
                    {
                        if (publication.Journal.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                        {
                            filteredList.Add(publication);
                            continue;
                        }
                    }

                    if (publication?.PublicationYear != null)
                    {
                        if (publication.PublicationYear.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                        {
                            filteredList.Add(publication);
                            continue;
                        }
                    }

                    if (publication?.Publisher != null)
                    {
                        if (publication.Publisher.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                        {
                            filteredList.Add(publication);
                            continue;
                        }
                    }

                    if (publication?.Title != null)
                    {
                        if (publication.Title.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                        {
                            filteredList.Add(publication);
                            continue;
                        }
                    }

                    if (publication?.Abstract != null)
                    {
                        if (publication.Abstract.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                        {
                            filteredList.Add(publication);
                            continue;
                        }
                    }

                    if (publication?.Authors != null)
                    {
                        if (publication.Authors.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                        {
                            filteredList.Add(publication);
                            continue;
                        }
                    }

                    foreach (var publicationCitation in publication.PublicationCitationList)
                    {
                        if (publicationCitation?.Citation == null)
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(publicationCitation.Citation.Abstract))
                        {
                            if (publicationCitation.Citation.Abstract.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                            {
                                filteredList.Add(publication);
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(publicationCitation.Citation.Authors))
                        {
                            if (publicationCitation.Citation.Authors.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                            {
                                filteredList.Add(publication);
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(publicationCitation.Citation.Title))
                        {
                            if (publicationCitation.Citation.Title.Trim().ToLower().Contains(searchedText.Trim().ToLower()))
                            {
                                filteredList.Add(publication);
                                break;
                            }
                        }
                    }
                }

                return filteredList;
            }
        }

        /// <summary>
        /// Fetches and synchronizes data from www.OpenCitations.net
        /// </summary>
        /// <param name="context"></param>
        public void SyncFromOpenCitations(ApplicationDbContext context)
        {
            // iterate all publications
            foreach (var publication in context.Publications
                                        .Include(x => x.PublicationCitationList)
                                            .ThenInclude(x => x.Citation)
                                        .ToList())
            {
                // skip publications without DOI
                if (string.IsNullOrEmpty(publication.Doi))
                {
                    continue;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"https://opencitations.net");
                    var result = client.GetAsync($@"/index/api/v1/citations/{publication.Doi}").Result;
                    if (!result.IsSuccessStatusCode)
                    {
                        continue;
                    }

                    foreach (var citationJson in JArray.Parse(result.Content.ReadAsStringAsync().Result))
                    {
                        var citingDoi = (citationJson[@"citing"]?.Value<string>() ?? string.Empty)?.Split(' ').LastOrDefault();
                        if (string.IsNullOrEmpty(citingDoi))
                        {
                            continue;
                        }

                        result = client.GetAsync($@"/index/api/v1/metadata/{citingDoi}").Result;

                        if (!result.IsSuccessStatusCode)
                        {
                            continue;
                        }

                        foreach (var citing in JArray.Parse(result.Content.ReadAsStringAsync().Result))
                        {
                            var citationPublication = publication.PublicationCitationList
                                .FirstOrDefault(x => string.Equals(x.Citation.Title.ToLower().Trim(), (citing[@"title"]?.Value<string>() ?? string.Empty).ToLower().Trim()));
                            if (citationPublication != null) // UPDATE CITATION
                            {
                                if (string.IsNullOrEmpty(citationPublication.Citation.PublicationUrl))
                                {
                                    citationPublication.Citation.PublicationUrl = citing[@"oa_link"]?.Value<string>() ?? string.Empty;
                                }

                                if (string.IsNullOrEmpty(citationPublication.Citation.Authors))
                                {
                                    citationPublication.Citation.Authors = citing[@"author"]?.Value<string>() ?? string.Empty;
                                }

                                if (string.IsNullOrEmpty(citationPublication.Citation.Journal))
                                {
                                    citationPublication.Citation.Journal = citing[@"source_title"]?.Value<string>() ?? string.Empty;
                                }

                                if (string.IsNullOrEmpty(citationPublication.Citation.PublicationYear))
                                {
                                    citationPublication.Citation.PublicationYear = citing[@"year"]?.Value<string>() ?? string.Empty;
                                }

                                if (string.IsNullOrEmpty(citationPublication.Citation.Doi))
                                {
                                    citationPublication.Citation.Doi = citing[@"doi"]?.Value<string>() ?? string.Empty;
                                }
                            }
                            else // INSERT CITATION
                            {
                                publication.PublicationCitationList.Add(
                                    new PublicationCitation
                                    {
                                        Citation = new Citation
                                        {
                                            Title = citing[@"title"]?.Value<string>() ?? string.Empty,
                                            PublicationUrl = citing[@"oa_link"]?.Value<string>() ?? string.Empty,
                                            Journal = citing[@"source_title"]?.Value<string>() ?? string.Empty,
                                            PublicationYear = citing[@"year"]?.Value<string>() ?? string.Empty,
                                            Doi = citing[@"doi"]?.Value<string>() ?? string.Empty,
                                            Authors = citing[@"author"]?.Value<string>() ?? string.Empty
                                        }
                                    });
                            }
                        }
                    }
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Fetches and synchronizes data from Google Scholar
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="name">Author's name</param>
        public void SyncFromScholar(ApplicationDbContext context, string name)
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"cmd.exe",
                    Arguments = $"/C python {AppSettings.ScholarScriptPath} --author \"{name}\"",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            var dump = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var json = JObject.Parse(File.ReadAllText($"{ AppSettings.OutputFolder }/scholar_output.data", Encoding.UTF8));

            var author = context.Authors.ToList().FirstOrDefault(x => x.Name == AppSettings.Author);
            var authorJson = json[@"author"];

            if (author != null)
            {
                if (authorJson != null)
                {
                    author.ScholarId = authorJson[@"scholar_id"]?.Value<string>();
                    author.UrlPicture = authorJson[@"url_picture"]?.Value<string>();
                    author.Affiliation = authorJson[@"affiliation"]?.Value<string>();
                    author.TotalCites = authorJson[@"total_cites"].Value<long>();
                }

                context.SaveChanges();

                var publications = context.Publications.Include(x => x.PublicationCitationList)
                    .ThenInclude(x => x.Citation).Include(x => x.Author).ToList();

                foreach (var pub in json[@"publications"])
                {
                    var bib = pub[@"bib"] ?? string.Empty;
                    var publication = publications.FirstOrDefault(x => string.Equals(x.Title.ToLower().Trim(),
                        (bib[@"title"]?.ToString() ?? string.Empty).ToLower().Trim()));
                    if (publication != null) // UPDATE PUBLICATION
                    {
                        publication.PublicationYear = bib[@"pub_year"]?.Value<string>();
                        publication.Authors =
                            string.Join(Environment.NewLine, bib[@"author"]?.Value<string>() ?? string.Empty) ??
                            string.Empty;
                        publication.Journal = bib[@"journal"]?.Value<string>();
                        publication.JournalVolume = bib[@"volume"]?.Value<string>();
                        publication.Pages = bib[@"pages"]?.Value<string>();
                        publication.Publisher = bib[@"publisher"]?.Value<string>();
                        publication.Abstract = bib[@"abstract"]?.Value<string>();
                        publication.EprintUrl = pub[@"eprint_url"]?.Value<string>() ??
                                                (pub[@"pub_url"]?.Value<string>() ?? string.Empty);
                        publication.CitesPerYear = pub[@"cites_per_year"]?.ToString() ?? string.Empty;

                        var citations = pub[@"citations"]?.ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(citations))
                        {
                            continue;
                        }

                        foreach (var citationJson in JArray.Parse(citations))
                        {
                            var citationPublication = publication.PublicationCitationList.FirstOrDefault(x =>
                                string.Equals(x.Citation.Title.ToLower().Trim(),
                                    (citationJson.Value<string>(@"title") ?? string.Empty).ToLower().Trim()));
                            if (citationPublication != null) // UPDATE CITATION
                            {
                                citationPublication.Citation.Authors = string.Join(Environment.NewLine,
                                    citationJson[@"author"] ?? string.Empty);
                                citationPublication.Citation.PublicationYear =
                                    citationJson[@"pub_year"]?.Value<string>() ?? string.Empty;
                                citationPublication.Citation.Journal =
                                    citationJson[@"venue"]?.Value<string>() ?? string.Empty;
                                citationPublication.Citation.Abstract =
                                    citationJson[@"abstract"]?.Value<string>() ?? string.Empty;
                            }
                            else // INSERT CITATION
                            {
                                publication.PublicationCitationList ??= new List<PublicationCitation>();

                                publication.PublicationCitationList.Add(new PublicationCitation
                                {
                                    Citation = new Citation
                                    {
                                        Title = citationJson[@"title"]?.Value<string>() ?? string.Empty,
                                        Authors = string.Join(Environment.NewLine,
                                            citationJson[@"author"] ?? string.Empty),
                                        PublicationYear =
                                            citationJson[@"pub_year"]?.Value<string>() ?? string.Empty,
                                        Journal = citationJson[@"venue"]?.Value<string>() ?? string.Empty,
                                        Abstract = citationJson[@"abstract"]?.Value<string>() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                    else // INSERT PULICATION
                    {
                        var quarantinedPublications = context.QuarantinedPublications.ToList().FirstOrDefault(x =>
                            string.Equals(x.Title.Trim().ToLower(),
                                (bib[@"title"]?.ToString() ?? string.Empty).Trim().ToLower()));
                        if (quarantinedPublications != null)
                        {
                            // if in quarantine, skip
                            continue;
                        }

                        publication = new Publication
                        {
                            AuthorId = author.AuthorId,
                            Title = bib[@"title"]?.ToString() ?? string.Empty,
                            PublicationYear = bib[@"pub_year"]?.ToString() ?? string.Empty,
                            Authors =
                                string.Join(Environment.NewLine, bib[@"author"]?.Value<string>() ?? string.Empty) ??
                                string.Empty,
                            Journal = bib[@"journal"]?.ToString() ?? string.Empty,
                            JournalVolume = bib[@"volume"]?.ToString() ?? string.Empty,
                            Pages = bib[@"pages"]?.ToString() ?? string.Empty,
                            Publisher = bib[@"publisher"]?.ToString() ?? string.Empty,
                            Abstract = bib[@"abstract"]?.ToString() ?? string.Empty,
                            PublicationCitationList = new List<PublicationCitation>()
                        };

                        var citationsJson = pub[@"citations"] ?? string.Empty;
                        foreach (var citationJson in citationsJson)
                        {
                            publication.PublicationCitationList.Add(new PublicationCitation
                            {
                                Citation = new Citation
                                {
                                    Title = citationJson["title"]?.ToString() ?? string.Empty,
                                    Authors = citationJson["author"]?.ToString() ?? string.Empty,
                                    PublicationYear = citationJson["pub_year"]?.ToString() ?? string.Empty,
                                    Journal = citationJson["venue"]?.ToString() ?? string.Empty,
                                    Abstract = citationJson["abstract"]?.ToString() ?? string.Empty
                                }
                            });
                        }

                        context.Publications.Add(publication);
                    }
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Fetches and synchronizes data from Semantics Scholar
        /// </summary>
        /// <param name="context">Application DB context</param>
        public void SyncFromSemantics(ApplicationDbContext context)
        {
            // iterate all publications
            foreach (var publication in context.Publications
                                        .Include(x => x.PublicationCitationList)
                                            .ThenInclude(x => x.Citation)
                                        .ToList())
            {
                // skip publications without DOI
                if (string.IsNullOrEmpty(publication.Doi))
                {
                    continue;
                }

                // run script
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        Arguments = $@"/C python {AppSettings.SemanticsScriptPath} --doi {publication.Doi}",
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };

                process.Start();
                var dump = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // fetch data from script's output
                var jsonArr = JArray.Parse(File.ReadAllText($"{ AppSettings.OutputFolder }/semantics_output.data", Encoding.UTF8));

                foreach (var citationJson in jsonArr)
                {
                    var citationPublication = publication.PublicationCitationList.FirstOrDefault(x => string.Equals(x.Citation.Title.ToLower().Trim(), (citationJson[@"title"]?.Value<string>() ?? string.Empty).ToLower().Trim()));
                    if (citationPublication != null) // UPDATE CITATION
                    {
                        if (string.IsNullOrEmpty(citationPublication.Citation.Title))
                        {
                            citationPublication.Citation.Title = citationJson[@"title"]?.Value<string>() ?? string.Empty;
                        }

                        if (string.IsNullOrEmpty(citationPublication.Citation.PublicationYear))
                        {
                            citationPublication.Citation.PublicationYear = citationJson[@"pub_year"]?.Value<string>() ?? string.Empty;
                        }

                        if (string.IsNullOrEmpty(citationPublication.Citation.Journal))
                        {
                            citationPublication.Citation.Journal = citationJson[@"journal"]?.Value<string>() ?? string.Empty;
                        }

                        if (string.IsNullOrEmpty(citationPublication.Citation.PublicationUrl))
                        {
                            citationPublication.Citation.PublicationUrl = citationJson[@"pub_url"]?.Value<string>() ?? string.Empty;
                        }

                        if (string.IsNullOrEmpty(citationPublication.Citation.Doi))
                        {
                            citationPublication.Citation.Doi = citationJson[@"doi"]?.Value<string>() ?? string.Empty;
                        }

                        if (string.IsNullOrEmpty(citationPublication.Citation.Authors))
                        {
                            citationPublication.Citation.Authors = citationJson[@"author"]?.Value<string>() ?? string.Empty;
                        }
                    }
                    else // INSERT CITATION
                    {
                        publication.PublicationCitationList ??= new List<PublicationCitation>();

                        publication.PublicationCitationList.Add(new PublicationCitation
                        {
                            Citation = new Citation
                            {
                                Title = citationJson[@"title"]?.Value<string>() ?? string.Empty,
                                Authors = citationJson[@"author"]?.Value<string>() ?? string.Empty,
                                PublicationYear = citationJson[@"pub_year"]?.Value<string>() ?? string.Empty,
                                Journal = citationJson[@"journal"]?.Value<string>() ?? string.Empty,
                                PublicationUrl = citationJson[@"pub_url"]?.Value<string>() ?? string.Empty,
                                Doi = citationJson[@"doi"]?.Value<string>() ?? string.Empty
                            }
                        });
                    }
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Updates publication's DOI by publication ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publicationId">Publication ID</param>
        /// <param name="doi">DOI string value</param>
        public void UpdatePublicationDoi(ApplicationDbContext context, long publicationId, string doi)
        {
            var publication = context.Publications.ToList().FirstOrDefault(x => x.PublicationId == publicationId);
            if (publication == null)
            {
                return;
            }

            publication.Doi = doi;
            context.SaveChanges();
        }

        /// <summary>
        /// Updates or inserts publication in/to the library
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publication">Publication object</param>
        public void UpdateInsertPublication(ApplicationDbContext context, Publication publication)
        {
            if (publication.PublicationId == 0)
            {
                throw new Exception(@"ERROR: Cannot update publication with unspecified publication ID");
            }

            var pub = context.Publications.ToList().FirstOrDefault(x => x.PublicationId == publication.PublicationId);
            if (pub != null)
            {
                pub.Authors = publication.Authors;
                pub.Doi = publication.Doi;
                pub.Journal = publication.Journal;
                pub.JournalVolume = publication.JournalVolume;
                pub.PublicationCitationList = publication.PublicationCitationList;
                pub.PublicationYear = publication.PublicationYear;
                pub.Publisher = publication.Publisher;
                pub.Title = publication.Title;
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Inserts publication to the library
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publication">Publication object</param>
        public void InsertPublication(ApplicationDbContext context, Publication publication)
        {
            var pub = context.Publications.ToList().FirstOrDefault(x => x.Title == publication.Title);
            if (pub != null)
            {
                return;
            }

            var quarantinedPublications = context.QuarantinedPublications.ToList().FirstOrDefault(x => string.Equals(x.Title.Trim().ToLower(), (publication?.Title ?? string.Empty).Trim().ToLower()));
            if (quarantinedPublications != null)
            {
                // if in quarantine, skip
                return;
            }

            context.Publications.Add(publication);
            context.SaveChanges();
        }

        /// <summary>
        /// Insert citation to publication
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="citation">Citation oject</param>
        /// <param name="publicationId">Publication ID</param>
        public void InsertCitation(ApplicationDbContext context, Citation citation, long publicationId)
        {
            var publication = context.Publications.Include(x => x.PublicationCitationList).ToList().FirstOrDefault(x => x.PublicationId == publicationId);
            if (publication == null)
            {
                return;
            }

            publication.PublicationCitationList.Add(
                new PublicationCitation
                {
                    Citation = citation
                }
            );

            context.SaveChanges();
        }

        /// <summary>
        /// Removes citation by ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="publicationId">Publication ID</param>
        /// <param name="citationId">Citation ID</param>
        public void RemoveCitation(ApplicationDbContext context, long publicationId, long citationId)
        {
            var publication = context.Publications.Include(x => x.PublicationCitationList).ThenInclude(x => x.Citation).ToList()
                .FirstOrDefault(x => x.PublicationId == publicationId);

            publication?.PublicationCitationList.Remove(publication.PublicationCitationList.FirstOrDefault(x => x.Citation.CitationId == citationId));

            context.SaveChanges();
        }

        public List<KeyValuePair<Publication, Citation>> ReportCitations(ApplicationDbContext context, long? yearFrom, long? yearTo, List<string> publicationCategories, List<long> citationCategories)
        {
            var dict = new List<KeyValuePair<Publication, Citation>>();

            var allPublications = context.Publications
               .Include(x => x.PublicationCategory)
               .Include(x => x.PublicationCitationList)
               .ThenInclude(x => x.Citation)
               .ThenInclude(x => x.CitationCategory)
               .ToList();

            foreach(var publication in allPublications)
            {
                if (publication.PublicationCategory == null && publicationCategories.Count > 0)
                {
                    continue;
                }

                if (publicationCategories.Count > 0 && !publicationCategories.Contains(publication.PublicationCategory.Code))
                {
                    continue;
                }

                foreach(var citation in publication.PublicationCitationList.Select(x => x.Citation))
                {
                    if (!Int64.TryParse(citation.PublicationYear, out var yr))
                    {
                        continue;
                    }

                    if ((yearFrom != null && yr < yearFrom) || ( yearTo != null && yr > yearTo))
                    {
                        continue;
                    }

                    if (citation.CitationCategory == null && citationCategories.Count > 0)
                    {
                        continue;
                    }

                    if (citationCategories.Count > 0 && !citationCategories.Contains(citation.CitationCategory.Code))
                    {
                        continue;
                    }

                    dict.Add(new KeyValuePair<Publication, Citation>(publication, citation));
                }
            }

            return dict;
        }

        /// <summary>
        /// Updates citation By ID
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="citation">Citation object</param>
        /// <param name="publicationId">Publication ID</param>
        /// <returns></returns>
        public Citation UpdateCitation(ApplicationDbContext context, Citation citation, long publicationId)
        {
            var origin = context.Citations.FirstOrDefault(x => x.CitationId == citation.CitationId);
            if (origin != null)
            {
                origin.Authors = citation.Authors;
                origin.CitationCategoryId = citation.CitationCategoryId;
                origin.Title = citation.Title;
                origin.Journal = citation.Journal;

                context.SaveChanges();

                return origin;
            }

            return null;
        }
    }
}
