namespace PubCiterAPI.Repositories
{
    using IronPython.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Scripting.Hosting;
    using Newtonsoft.Json.Linq;
    using PubCiterAPI.Model;
    using PubCiterAPI.Repositories.Interfaces;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
        public IEnumerable<Publication> GetPublications(ApplicationDbContext context, string name)
        {
            return context.Publications
                .Include(x => x.PublicationCitationList)
                .ThenInclude(x => x.Citation);
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

                /*var jsonArr = JArray.Parse(data);

                if (publication != null)
                {
                    foreach (var obj in jsonArr)
                    {
                        publication.PublicationCitationList.Add(
                            new PublicationCitation
                            {
                                Citation = new Citation
                                {
                                    Title = obj.Value<string>("title") ?? string.Empty,
                                    PublicationUrl = obj.Value<string>("pub_url") ?? string.Empty,
                                    Journal = obj.Value<string>("journal") ?? string.Empty,
                                    PublicationYear = obj.Value<string>("pub_year") ?? string.Empty,
                                    Doi = obj.Value<string>("doi") ?? string.Empty
                                }
                            });
                    }
                }*/
            }

            context.SaveChanges();
        }

        public void SyncFromScholar(ApplicationDbContext context, string name)
        {
            /*System.Diagnostics.Process process = new System.Diagnostics.Process();
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
            process.WaitForExit();*/
            var json = JObject.Parse(File.ReadAllText($"{ AppSettings.OutputFolder }/scholar_output.data", Encoding.UTF8));

            var author = context.Authors.ToList().FirstOrDefault(x => x.Name == AppSettings.Author);
            var authorJson = json[@"author"];

            author.ScholarId = authorJson[@"scholar_id"].Value<string>();
            author.UrlPicture = authorJson[@"url_picture"].Value<string>();
            author.Affiliation = authorJson[@"affiliation"].Value<string>();
            author.TotalCites = authorJson[@"total_cites"].Value<long>();

            var publications = context.Publications.Include(x => x.PublicationCitationList).ThenInclude(x => x.Citation).ToList();

            foreach (var pub in json[@"publications"])
            {
                var bib = pub[@"bib"] ?? string.Empty;
                var publication = publications.FirstOrDefault(x => x.Title == (bib[@"title"].ToString() ?? string.Empty));
                if (publication != null) // UPDATE PUBLICATION
                {
                    publication.PublicationYear = bib[@"pub_year"].Value<string>();
                    publication.Authors = bib[@"author"].Value<string>();
                    publication.Journal = bib[@"journal"].Value<string>();
                    publication.JournalVolume = bib[@"volume"].Value<string>();
                    publication.Pages = bib[@"pages"].Value<string>();
                    publication.Publisher = bib[@"publisher"].Value<string>();
                    publication.Abstract = bib[@"abstract"].Value<string>();

                    var citations = pub.Value<string>(@"citations") ?? string.Empty;
                    if (!string.IsNullOrEmpty(citations))
                    {
                        foreach (var citationJson in JArray.Parse(citations))
                        {
                            var citationPublication = publication.PublicationCitationList.FirstOrDefault(x => x.Citation.Title == (citationJson.Value<string>("title") ?? string.Empty));
                            if (citationPublication != null) // UPDATE CITATION
                            {
                                citationPublication.Citation.Author = citationJson.Value<string>("author") ?? string.Empty;
                                citationPublication.Citation.PublicationYear = citationJson.Value<string>("pub_year") ?? string.Empty;
                                citationPublication.Citation.Journal = citationJson.Value<string>("venue") ?? string.Empty;
                                citationPublication.Citation.Abstract = citationJson.Value<string>("abstract") ?? string.Empty;
                            }
                            else // INSERT CITATION
                            {
                                publication.PublicationCitationList.Add(new PublicationCitation
                                {
                                    Citation = new Citation
                                    {
                                        Title = citationJson.Value<string>("title") ?? string.Empty,
                                        Author = citationJson.Value<string>("author") ?? string.Empty,
                                        PublicationYear = citationJson.Value<string>("pub_year") ?? string.Empty,
                                        Journal = citationJson.Value<string>("venue") ?? string.Empty,
                                        Abstract = citationJson.Value<string>("abstract") ?? string.Empty
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
                        Author = author,
                        Title = bib[@"title"].ToString() ?? string.Empty,
                        PublicationYear = bib[@"pub_year"].ToString() ?? string.Empty,
                        Authors = bib[@"author"].ToString() ?? string.Empty,
                        Journal = bib[@"journal"].ToString() ?? string.Empty,
                        JournalVolume = bib[@"volume"].ToString() ?? string.Empty,
                        Pages = bib[@"pages"].ToString() ?? string.Empty,
                        Publisher = bib[@"publisher"].ToString() ?? string.Empty,
                        Abstract = bib[@"abstract"].ToString() ?? string.Empty,
                        PublicationCitationList = new List<PublicationCitation>()
                    };

                    var citationsJson = pub[@"citations"] ?? string.Empty;
                    foreach (var citationJson in citationsJson)
                    {
                        publication.PublicationCitationList.Add(new PublicationCitation
                        {
                            Citation = new Citation
                            {
                                Title = citationJson["title"].ToString() ?? string.Empty,
                                Author = citationJson["author"].ToString() ?? string.Empty,
                                PublicationYear = citationJson["pub_year"].ToString() ?? string.Empty,
                                Journal = citationJson["venue"].ToString() ?? string.Empty,
                                Abstract = citationJson["abstract"].ToString() ?? string.Empty
                            }
                        });
                    }

                    publications.Add(publication);
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
                var data = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                var jsonArr = JArray.Parse(data);

                if (publication != null)
                {
                    foreach (var obj in jsonArr)
                    {
                        publication.PublicationCitationList.Add(
                            new PublicationCitation
                            {
                                Citation = new Citation
                                {
                                    Title = obj.Value<string>("title") ?? string.Empty,
                                    PublicationUrl = obj.Value<string>("pub_url") ?? string.Empty,
                                    Journal = obj.Value<string>("journal") ?? string.Empty,
                                    PublicationYear = obj.Value<string>("pub_year") ?? string.Empty,
                                    Doi = obj.Value<string>("doi") ?? string.Empty
                                }
                            });
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
