using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Proxies;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;

namespace Paranovels.Facade
{
    public class FeedFacade : IFacade
    {
        public int CheckFeed(int connectorTypeFeed, int feedID)
        {
            int newReleaseCount = 0;
            var lastCheckedDate = DateTime.Now.AddMinutes(Math.Abs(5) * -1);

            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new FeedService(uow);
                var releaseService = new ReleaseService(uow);

                var qFeed = service.View<Feed>().Where(w => w.Status == R.FeedStatus.ACTIVE && w.LastSuccessDate < lastCheckedDate);
                var qConnector = service.View<Connector>().Where(w => w.IsDeleted == false);
                var qAka = service.View<Aka>().Where(w => w.IsDeleted == false);
                var qSeries = service.View<Series>().Where(w => w.IsDeleted == false);
                var qGroup = service.View<Group>().Where(w => w.IsDeleted == false);

                // override datel limit when feedID is available
                if (feedID > 0)
                {
                    qFeed = service.View<Feed>().Where(w => w.ID == feedID);
                }

                var pendingUpdates = qConnector.Where(w => w.ConnectorType == connectorTypeFeed)
                                               .Join(qFeed, c => c.TargetID, f => f.ID, (c, f) => new { Connector = c, Feed = f }).OrderBy(o => o.Feed.LastSuccessDate).ToList();

                var lastRelease = releaseService.View<Release>().All().OrderByDescending(o => o.ID).First();

                foreach (var pendingUpdate in pendingUpdates)
                {
                    var lastSuccessDate = pendingUpdate.Feed.Total == 0 ? DateTime.MinValue : pendingUpdate.Feed.LastSuccessDate;
                    var releases = GetNewReleases(pendingUpdate.Feed.Url);
                    releases = releases.Where(w => w.Date > lastSuccessDate && w.Date < DateTime.Now).ToList(); // skip since the release date is older then the last feed date

                    if (!releases.Any()) continue;

                    foreach (var release in releases)
                    {
                        if (pendingUpdate.Connector.ConnectorType == R.ConnectorType.SERIES_FEED)
                        {
                            release.SeriesID = pendingUpdate.Connector.SourceID;
                            var series = qSeries.FirstOrDefault(w => w.ID == release.SeriesID);
                            if (series != null)
                            {
                                var groups = qConnector.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP && w.SourceID == series.ID)
                                                       .Join(qGroup, c => c.TargetID, g => g.ID, (c, g) => g).ToList();
                                
                                foreach (var group in groups)
                                {
                                    if (new Uri(group.Url).Host == new Uri(release.Url).Host)
                                    {
                                        release.GroupID = group.ID;
                                    }
                                }
                                // unable to match group to url, then set groupID to the one this series linked to
                                if (release.GroupID == 0 && groups.Count == 1)
                                {
                                    release.GroupID = groups[0].ID;
                                }
                            }
                        }
                        if (pendingUpdate.Connector.ConnectorType == R.ConnectorType.GROUP_FEED)
                        {
                            release.GroupID = pendingUpdate.Connector.SourceID;

                            // get all series this group has
                            var series = qConnector.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP && w.TargetID == release.GroupID)
                                                   .Join(qSeries, c => c.SourceID, s => s.ID, (c, s) => s).ToList();

                            var seriesIDs = series.Select(s => s.ID).ToList();
                            var akas = qAka.Where(w => w.SourceTable == R.SourceTable.SERIES && seriesIDs.Contains(w.SourceID)).ToList();
                            var titles = series.Select(s => new { s.ID, s.Title })
                                               .Union(akas.Select(s => new { ID = s.SourceID, Title = s.Text }));

                            foreach (var title in titles)
                            {
                                var titleSeo = title.Title.ToSeo();
                                if (release.Title.ToSeo().Contains(titleSeo) || release.Url.Contains(titleSeo))
                                {
                                    release.SeriesID = title.ID;
                                }
                            }
                            // unable to match series to title, then set seriesID to the one this group linked to
                            if (release.SeriesID == 0 && series.Count == 1)
                            {
                                release.SeriesID = series[0].ID;
                            }
                        }

                        release.Summary = "";
                        release.Type = release.Title.IsChapterRelease() ? R.ReleaseType.CHAPTER : R.ReleaseType.NEWS;
                        release.ID = releaseService.SaveChanges(release);

                        // notify only if release is new
                        if (release.ID > lastRelease.ID)
                        {
                            new ListFacade().EmailNotifySubscriber(release);
                        }

                        pendingUpdate.Feed.Total += 1;
                        newReleaseCount += 1;
                    }
                    // update feed last process date
                    if (releases.Any()) pendingUpdate.Feed.LastSuccessDate = DateTime.Now;
                    service.SaveChanges(new GenericForm<Feed> { DataModel = pendingUpdate.Feed });
                }
            }
            return newReleaseCount;
        }

        public IList<ReleaseForm> GetNewReleases(string url)
        {
            var chapters = new FeedChecker().Check(url);
            // filters out news and announments
            //chapters = chapters.Where(w => Regex.IsMatch(w.Title, @"\d+")); // must has number in title
            // return filtered chapters
            return chapters.ToList();
        }

        public Release FixRelease(int id)
        {
            string text;
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var rText = uow.Repository<Text>();
                var qGroup = uow.Repository<Group>().All();
                var qSeries = uow.Repository<Series>().All();
                var qRelease = uow.Repository<Release>().All().Where(w=> w.SeriesID > 0);

                // if no ID specify then limit to only release that mark as crawlable
                if (id == 0)
                {
                    qGroup = qGroup.Where(w => w.CrawlType == R.CrawlType.WORDPRESS);
                    qRelease = qRelease.Where(w => w.Type == R.ReleaseType.CHAPTER && w.SeriesID > 0 && qGroup.Any(a => a.ID == w.GroupID));
                }

                var release = id == 0 ? qRelease.OrderByDescending(o => o.ID).FirstOrDefault() : qRelease.SingleOrDefault(w => w.ID == id);               
                if (release == null) return null;

                var series = qSeries.Single(w => w.ID == release.SeriesID);
                if (series == null) return null;

                var group = qGroup.Single(w => w.ID == release.GroupID);
                if (group.CrawlType == R.CrawlType.WORDPRESS)
                {
                    text = FixReleaseFromWordpress(release);
                }
                else
                {
                    text = FixReleaseFromWordpress(release);
                    release.Summary = text;
                    return release;
                }

                if (text.Length > 1000)
                {
                    var chapterText = rText.Where(w=> w.SourceID == release.ID && w.SourceTable == R.SourceTable.RELEASE && w.Type == R.TextType.CHAPTER).SingleOrDefault();
                    if (chapterText == null)
                    {
                        chapterText = new Text
                        {
                            InsertedDate = DateTime.Now,
                            SourceID = release.ID,
                            SourceTable = R.SourceTable.RELEASE,
                            Type = R.TextType.CHAPTER
                        };
                        rText.Add(chapterText);
                    }

                    chapterText.UpdatedDate = DateTime.Now;
                    chapterText.TextMax = text;

                    release.Type = R.ReleaseType.CHAPTER_CRAWLED;
                    release.Title = series.Title + " " + release.Title.ExtractVolumeChapter();
                    // save
                    uow.Commit();
                }
                return release;
            }
        }

        string FixReleaseFromWordpress(Release release)
        {
            var webClient = WebClientFactory.ChromeClient();
            string html = webClient.DownloadString(release.Url);

            // fixed html entity
            html = html.Replace("&nbsp;", " ");

            var htmlDoc = new HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.OptionAutoCloseOnEnd = true;

            // filePath is a path to a file containing the html
            htmlDoc.LoadHtml(html);

         
            // Use:  htmlDoc.LoadHtml(xmlString);  to load from a string (was htmlDoc.LoadXML(xmlString)

            // ParseErrors is an ArrayList containing any errors from the Load statement
            //if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
            //{
            //    // Handle any parse errors as required
            //    return string.Format("Html parse errors ({0})", string.Join(", ", htmlDoc.ParseErrors.Select(s => s.Code + " - " + s.Reason)));
            //}

            if (htmlDoc.DocumentNode != null)
            {
                var nodes = htmlDoc.DocumentNode.SelectNodes(@"//div[@class=""entry-content-inner""]") ?? 
                            htmlDoc.DocumentNode.SelectNodes(@"//div[@class=""entry-content""]") ??
                            htmlDoc.DocumentNode.SelectNodes("//*[contains(@class,'event-text')]");
                // if no 'entry-content-inner' found then use 'entry-content'
                //var nodes = htmlDoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("float"));

                if (nodes != null)
                {
                    // remove meta & info blocks
                    var removeBlocks = new[] { "date", "post-info", "post-meta", "navigation", "code-block", "sharedaddy", "entry-content", "entry-content-inner" };
                    foreach (var removeBlock in removeBlocks)
                    {
                        var removeNodes = nodes[0].SelectNodes(string.Format("//*[contains(@class,'{0}')]", removeBlock));
                        if (removeNodes != null)
                        {
                            foreach (var removeNode in removeNodes)
                            {
                                removeNode.Remove();
                            }
                        }
                    }

                    var pTags = nodes[0].Descendants("p").ToList();
                    var aTags = pTags.Select(s => s.Element("a")).Where(aTag=> aTag != null).ToList();
                    if (pTags.Count() < 10 && aTags.Any())
                    {
                        var chapterUrl = "";
                        foreach (var htmlNode in aTags)
                        {
                            if(!string.IsNullOrWhiteSpace(chapterUrl) || htmlNode == null) continue;
                            
                            var url = htmlNode.GetAttributeValue("href", "");
                            var allowHosts = new[]
                            {
                                new Uri(release.Url).Host,
                                "wp.me"
                            };
                            if (allowHosts.Contains(new Uri(url).Host))
                            {
                                chapterUrl = url;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(chapterUrl))
                        {
                            release.Url = chapterUrl;
                            return FixReleaseFromWordpress(release);
                        }
                    }
                    else
                    {
                        var extras = new[] {"next","prev","table of contents", "notify me"};
                        // remove next chapter from first paragraph
                        var checkTags = pTags.Take(5).Union(pTags.Skip(pTags.Count - 5).Take(5)).ToList();
                        foreach (var pTag in checkTags)
                        {
                            if (string.IsNullOrWhiteSpace(pTag.InnerText) ||
                                extras.Any(w => pTag.InnerText.Contains(w, StringComparison.OrdinalIgnoreCase)))
                            {
                                pTags.Remove(pTag);
                            }
                        }
                        return string.Join("\n", pTags.Select(s=> s.InnerText.Trim()));
                    }
                }
            }

            return html;
        } 
    }
}
