using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public int CheckFeed(GroupCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ReleaseService(uow);
                var feedService = new FeedService(uow);
                var feedIDs = service.View<Connector>()
                    .Where(w => w.ConnectorType == R.ConnectorType.GROUP_FEED && w.SourceID == criteria.IDToInt)
                    .Select(s => s.TargetID).ToList();
                foreach (var feedID in feedIDs)
                {
                    var feed = service.View<Feed>().Where(w => w.FeedID == feedID).Single();
                    var glossaries = service.View<Connector>().Where(w => w.ConnectorType == R.ConnectorType.GROUP_GLOSSARY && w.SourceID == criteria.IDToInt)
                            .Join(service.View<Glossary>().All(), c => c.TargetID, g => g.GlossaryID,
                                (c, g) => g).ToList();
                    var releases = GetNewReleases(feed.Url);
                    foreach (var release in releases)
                    {
                        if (release.Date < feed.LastSuccessDate) continue; // skip since the release date is older then the last feed date
                        // fix release title
                        foreach (var glossary in glossaries)
                        {
                            release.Title = release.Title.Replace(glossary.Raw, glossary.Final);
                        }

                        release.Summary = "";
                        release.GroupID = criteria.IDToInt;
                        release.SeriesID = service.View<Series>()
                            .Where(w => w.GroupID == criteria.IDToInt)
                            .Where(w => release.Title.Contains(w.Title))
                            .Select(s => s.SeriesID).FirstOrDefault();
                        service.SaveChanges(release);
                        feed.Total = feed.Total + 1;
                    }
                    // update feed last process date
                    if (releases.Any()) feed.LastSuccessDate = DateTime.Now;
                    feedService.SaveChanges(new GenericForm<Feed> { DataModel = feed });
                }
            }
            return 0;
        }
        public int CheckFeed(int minutes = -30)
        {
            var lastCheckedDate = DateTime.Now.AddMinutes(Math.Abs(minutes) * -1);
            var map = new Dictionary<int, int>
            {
                {R.ConnectorType.SERIES_FEED, R.ConnectorType.SERIES_GLOSSARY},
                {R.ConnectorType.GROUP_FEED, R.ConnectorType.GROUP_GLOSSARY}
            };

            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new FeedService(uow);
                var releaseService = new ReleaseService(uow);

                var qConnector = service.View<Connector>().All();
                var qFeed = service.View<Feed>().Where(w => w.Status == R.FeedStatus.ACTIVE && w.UpdatedDate < lastCheckedDate);
                var qGlossary = service.View<Glossary>().All();

                var pendingUpdates = qConnector.Where(w => map.Keys.Contains(w.ConnectorType))
                                               .Join(qFeed, c => c.TargetID, f => f.FeedID, (c, f) => new { Connector = c, Feed = f }).ToList();
                foreach (var pendingUpdate in pendingUpdates)
                {
                    var releases = GetNewReleases(pendingUpdate.Feed.Url);

                    var connectorType = map[pendingUpdate.Connector.ConnectorType];
                    var glossaries = qConnector.Where(w => w.ConnectorType == connectorType && w.SourceID == pendingUpdate.Connector.SourceID)
                                               .Join(qGlossary, c => c.TargetID, g => g.GlossaryID, (c, g) => g).ToList();

                    foreach (var release in releases)
                    {
                        if (release.Date < pendingUpdate.Feed.LastSuccessDate) continue; // skip since the release date is older then the last feed date
                        // fix release title

                        foreach (var glossary in glossaries)
                        {
                            release.Title = release.Title.Replace(glossary.Raw, glossary.Final);
                        }
                        
                        if (pendingUpdate.Connector.ConnectorType == R.ConnectorType.SERIES_FEED)
                        {
                            release.SeriesID = pendingUpdate.Connector.SourceID;
                            release.GroupID = service.View<Series>().Where(w=> w.SeriesID == release.SeriesID)
                                .Select(s => s.GroupID).FirstOrDefault();
                        }
                        if (pendingUpdate.Connector.ConnectorType == R.ConnectorType.GROUP_FEED)
                        {
                            release.GroupID = pendingUpdate.Connector.SourceID;
                            release.SeriesID = service.View<Series>()
                                .Where(w => w.GroupID == release.GroupID)
                                .Where(w => release.Title.Contains(w.Title))
                                .Select(s => s.SeriesID).FirstOrDefault();
                        }

                        release.Summary = "";
                        releaseService.SaveChanges(release);

                        pendingUpdate.Feed.Total += 1;
                    }
                    // update feed last process date
                    if (releases.Any()) pendingUpdate.Feed.LastSuccessDate = DateTime.Now;
                    service.SaveChanges(new GenericForm<Feed> { DataModel = pendingUpdate.Feed });
                }
            }
            return -1;
        }

        IList<ReleaseForm> GetNewReleases(string url)
        {
            var chapters = new FeedChecker().Check(url);
            // filters out news and announments
            chapters = chapters.Where(w => Regex.IsMatch(w.Title, @"\d+")); // must has number in title
            // return filtered chapters
            return chapters.ToList();
        }
    }
}
