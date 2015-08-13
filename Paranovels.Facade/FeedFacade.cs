using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Proxies;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;
using Thi.Web;
using Group = Paranovels.DataModels.Group;

namespace Paranovels.Facade
{
    public class FeedFacade : IFacade
    {
        public int CheckFeed(int connectorTypeFeed, int minutes = 5)
        {
            int newReleaseCount = 0;
            var lastCheckedDate = DateTime.Now.AddMinutes(Math.Abs(minutes) * -1);
            var mapFeedGlossary = new Dictionary<int, int>
            {
                {R.ConnectorType.SERIES_FEED, R.ConnectorType.SERIES_GLOSSARY},
                {R.ConnectorType.GROUP_FEED, R.ConnectorType.GROUP_GLOSSARY}
            };

            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new FeedService(uow);
                var releaseService = new ReleaseService(uow);

                var qFeed = service.View<Feed>().Where(w => w.Status == R.FeedStatus.ACTIVE && w.LastSuccessDate < lastCheckedDate);
                var qConnector = service.View<Connector>().Where(w => w.IsDeleted == false);
                var qAka = service.View<Aka>().Where(w => w.IsDeleted == false);
                var qSeries = service.View<Series>().Where(w => w.IsDeleted == false);
                var qGroup = service.View<Group>().Where(w => w.IsDeleted == false);

                var pendingUpdates = qConnector.Where(w => w.ConnectorType == connectorTypeFeed)
                                               .Join(qFeed, c => c.TargetID, f => f.FeedID, (c, f) => new { Connector = c, Feed = f }).OrderBy(o => o.Feed.LastSuccessDate).ToList();

                var lastRelease = releaseService.View<Release>().All().OrderByDescending(o => o.ReleaseID).First();

                foreach (var pendingUpdate in pendingUpdates)
                {
                    var releases = GetNewReleases(pendingUpdate.Feed.Url);
                    releases = releases.Where(w => w.Date > pendingUpdate.Feed.LastSuccessDate && w.Date < DateTime.Now).ToList(); // skip since the release date is older then the last feed date

                    if (!releases.Any()) continue;

                    foreach (var release in releases)
                    {
                        if (pendingUpdate.Connector.ConnectorType == R.ConnectorType.SERIES_FEED)
                        {
                            release.SeriesID = pendingUpdate.Connector.SourceID;
                            var series = qSeries.FirstOrDefault(w => w.SeriesID == release.SeriesID);
                            if (series != null)
                            {
                                var groups = qConnector.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP && w.SourceID == series.SeriesID)
                                        .Join(qGroup, c => c.TargetID, g => g.GroupID, (c, g) => g).ToList();

                                if (groups.Count == 0)
                                {

                                }
                                else if (groups.Count == 1)
                                {
                                    release.GroupID = groups[0].GroupID;
                                }
                                else
                                {
                                    foreach (var group in groups)
                                    {
                                        if (new Uri(group.Url).Host == new Uri(release.Url).Host)
                                        {
                                            release.GroupID = group.GroupID;
                                        }
                                    }
                                }
                            }
                        }
                        if (pendingUpdate.Connector.ConnectorType == R.ConnectorType.GROUP_FEED)
                        {
                            release.GroupID = pendingUpdate.Connector.SourceID;

                            // get all series this group has
                            var series = qConnector.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP && w.TargetID == release.GroupID)
                                .Join(qSeries, c => c.SourceID, s => s.SeriesID, (c, s) => s).ToList();

                            if (series.Count == 0)
                            {

                            }
                            else if (series.Count == 1)
                            {
                                release.SeriesID = series[0].SeriesID;
                            }
                            else
                            {
                                var seriesIDs = series.Select(s => s.SeriesID).ToList();
                                var akas = qAka.Where(w => w.SourceTable == R.SourceTable.SERIES && seriesIDs.Contains(w.SourceID)).ToList();
                                var titles = series.Select(s => new { ID = s.SeriesID, Title = s.Title })
                                    .Union(akas.Select(s => new { ID = s.SourceID, Title = s.Text }));

                                foreach (var title in titles)
                                {
                                    if (release.Title.ToSeo().Contains(title.Title.ToSeo()))
                                    {
                                        release.SeriesID = title.ID;
                                    }
                                }
                            }
                        }

                        release.Summary = "";
                        release.Type = R.ReleaseType.CHAPTER;
                        release.ReleaseID = releaseService.SaveChanges(release);

                        // notify only if release is new
                        if (release.ReleaseID > lastRelease.ReleaseID)
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

        IList<ReleaseForm> GetNewReleases(string url)
        {
            var chapters = new FeedChecker().Check(url);
            // filters out news and announments
            //chapters = chapters.Where(w => Regex.IsMatch(w.Title, @"\d+")); // must has number in title
            // return filtered chapters
            return chapters.ToList();
        }
    }
}
