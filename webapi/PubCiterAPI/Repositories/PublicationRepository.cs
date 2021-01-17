namespace PubCiterAPI.Repositories
{
    using IronPython.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Scripting.Hosting;
    using Newtonsoft.Json.Linq;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;

    /// <summary>
    /// Publication Repository
    /// </summary>
    public class PublicationRepository : IPublicationRepository
    {
        /// <summary>
        /// Gets publications of author by their name
        /// </summary>
        /// <param name="name">Author's name</param>
        /// <returns>Collection of publications</returns>
        public IEnumerable<Publication> GetPublications(ApplicationDbContext context)
        {
            var author = context.Authors.ToList().FirstOrDefault(x => x.Name == AppSettings.Author);
            if (author != null)
            {
                return context.Publications
                .Include(x => x.PublicationCitationList)
                .ThenInclude(x => x.Citation).ToList().Where(x => x.AuthorId == author.AuthorId);
            }

            return null;
        }

        /// <summary>
        /// Fetches and synchronizes data from opencitations.net
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
                    if (result.IsSuccessStatusCode)
                    {
                        foreach (var citationJson in JArray.Parse(result.Content.ReadAsStringAsync().Result))
                        {
                            var citingDoi = (citationJson[@"citing"]?.Value<string>() ?? string.Empty)?.Split(' ').LastOrDefault();
                            if (!string.IsNullOrEmpty(citingDoi))
                            {
                                result = client.GetAsync($@"/index/api/v1/metadata/{citingDoi}").Result;
                                if (result.IsSuccessStatusCode)
                                {
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

                                            if (string.IsNullOrEmpty(citationPublication.Citation.Author))
                                            {
                                                citationPublication.Citation.Author = citing[@"author"]?.Value<string>() ?? string.Empty;
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
                                                    Author = citing[@"author"]?.Value<string>() ?? string.Empty
                                                }
                                            });
                                        }
                                    }
                                }
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
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = @"cmd.exe",
                Arguments = $"/C python { AppSettings.ScholarScriptPath } --author \"{name}\"",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.Start();
            var dump = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var json = JObject.Parse(File.ReadAllText($"{ AppSettings.OutputFolder }/scholar_output.data", Encoding.UTF8));

            var author = context.Authors.ToList().FirstOrDefault(x => x.Name == AppSettings.Author);
            var authorJson = json[@"author"];

            author.ScholarId = authorJson[@"scholar_id"]?.Value<string>();
            author.UrlPicture = authorJson[@"url_picture"]?.Value<string>();
            author.Affiliation = authorJson[@"affiliation"]?.Value<string>();
            author.TotalCites = authorJson[@"total_cites"].Value<long>();

            context.SaveChanges();

            var publications = context.Publications.Include(x => x.PublicationCitationList).ThenInclude(x => x.Citation).Include(x => x.Author).ToList();

            foreach (var pub in json[@"publications"])
            {
                var bib = pub[@"bib"] ?? string.Empty;
                var publication = publications.FirstOrDefault(x => string.Equals(x.Title.ToLower().Trim(), (bib[@"title"]?.ToString() ?? string.Empty).ToLower().Trim()));
                if (publication != null) // UPDATE PUBLICATION
                {
                    publication.PublicationYear = bib[@"pub_year"]?.Value<string>();
                    publication.Authors = string.Join(Environment.NewLine, bib[@"author"]?.Value<string>() ?? string.Empty) ?? string.Empty;
                    publication.Journal = bib[@"journal"]?.Value<string>();
                    publication.JournalVolume = bib[@"volume"]?.Value<string>();
                    publication.Pages = bib[@"pages"]?.Value<string>();
                    publication.Publisher = bib[@"publisher"]?.Value<string>();
                    publication.Abstract = bib[@"abstract"]?.Value<string>();

                    var citations = pub[@"citations"]?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(citations))
                    {
                        foreach (var citationJson in JArray.Parse(citations))
                        {
                            var citationPublication = publication.PublicationCitationList.FirstOrDefault(x => string.Equals(x.Citation.Title.ToLower().Trim(), (citationJson.Value<string>(@"title") ?? string.Empty).ToLower().Trim()));
                            if (citationPublication != null) // UPDATE CITATION
                            {
                                citationPublication.Citation.Author = string.Join(Environment.NewLine, citationJson[@"author"] ?? string.Empty);
                                citationPublication.Citation.PublicationYear = citationJson[@"pub_year"]?.Value<string>() ?? string.Empty;
                                citationPublication.Citation.Journal = citationJson[@"venue"]?.Value<string>() ?? string.Empty;
                                citationPublication.Citation.Abstract = citationJson[@"abstract"]?.Value<string>() ?? string.Empty;
                            }
                            else // INSERT CITATION
                            {
                                if (publication.PublicationCitationList == null)
                                {
                                    publication.PublicationCitationList = new List<PublicationCitation>();
                                }

                                publication.PublicationCitationList.Add(new PublicationCitation
                                {
                                    Citation = new Citation
                                    {
                                        Title = citationJson[@"title"]?.Value<string>() ?? string.Empty,
                                        Author = string.Join(Environment.NewLine, citationJson[@"author"] ?? string.Empty),
                                        PublicationYear = citationJson[@"pub_year"]?.Value<string>() ?? string.Empty,
                                        Journal = citationJson[@"venue"]?.Value<string>() ?? string.Empty,
                                        Abstract = citationJson[@"abstract"]?.Value<string>() ?? string.Empty
                                    }
                                });
                            }
                        }
                    }
                }
                else // INSERT PULICATION
                {
                    publication = new Publication
                    {
                        AuthorId = author.AuthorId,
                        Title = bib[@"title"]?.ToString() ?? string.Empty,
                        PublicationYear = bib[@"pub_year"]?.ToString() ?? string.Empty,
                        Authors = string.Join(Environment.NewLine, bib[@"author"]?.Value<string>() ?? string.Empty) ?? string.Empty,
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
                                Author = citationJson["author"]?.ToString() ?? string.Empty,
                                PublicationYear = citationJson["pub_year"]?.ToString() ?? string.Empty,
                                Journal = citationJson["venue"]?.ToString() ?? string.Empty,
                                Abstract = citationJson["abstract"]?.ToString() ?? string.Empty
                            }
                        });
                    }

                    context.Publications.Add(publication);
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Fetches and synchronizes data from Semantics Scholar
        /// </summary>
        /// <param name="context">Aplication DB context</param>
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
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $@"/C python {AppSettings.SemanticsScriptPath} --doi {publication.Doi}",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
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

                        if (string.IsNullOrEmpty(citationPublication.Citation.Author))
                        {
                            citationPublication.Citation.Author = citationJson[@"author"]?.Value<string>() ?? string.Empty;
                        }
                    }
                    else // INSERT CITATION
                    {
                        if (publication.PublicationCitationList == null)
                        {
                            publication.PublicationCitationList = new List<PublicationCitation>();
                        }

                        publication.PublicationCitationList.Add(new PublicationCitation
                        {
                            Citation = new Citation
                            {
                                Title = citationJson[@"title"]?.Value<string>() ?? string.Empty,
                                Author = citationJson[@"author"]?.Value<string>() ?? string.Empty,
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
            if (publication != null)
            {
                publication.Doi = doi;
                context.SaveChanges();
            }
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
            if (pub == null)
            {
                context.Publications.Add(publication);
                context.SaveChanges();
            }
        }
    }
}
