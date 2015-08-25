using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Facade
{
    public class SeriesFacade : IFacade
    {
        public int AddSeries(SeriesForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new SeriesService(uow);
                var id = service.SaveChanges(form);

                var connectorService = new ConnectorService(uow);

                if (form.Categories != null || form.InlineEditProperty == form.PropertyName(m => m.Categories))
                {
                    form.Categories = form.Categories ?? new List<int>();
                    var connectorForm = new GenericForm<Connector>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Connector { ConnectorType = R.ConnectorType.SERIES_TAGCATEGORY, SourceID = id }
                    };
                    connectorService.Connect(connectorForm, form.Categories);
                }
                if (form.Genres != null || form.InlineEditProperty == form.PropertyName(m => m.Genres))
                {
                    form.Genres = form.Genres ?? new List<int>();
                    var connectorForm = new GenericForm<Connector>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Connector { ConnectorType = R.ConnectorType.SERIES_TAGGENRE, SourceID = id }
                    };
                    connectorService.Connect(connectorForm, form.Genres);
                }
                if (form.Contains != null || form.InlineEditProperty == form.PropertyName(m => m.Contains))
                {
                    form.Contains = form.Contains ?? new List<int>();
                    var connectorForm = new GenericForm<Connector>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Connector { ConnectorType = R.ConnectorType.SERIES_TAGCONTAIN, SourceID = id }
                    };
                    connectorService.Connect(connectorForm, form.Contains);
                }

                if (form.Feeds != null || form.InlineEditProperty == form.PropertyName(m => m.Feeds))
                {
                    var feedService = new FeedService(uow);
                    foreach (var feed in form.Feeds)
                    {
                        feed.UrlHash = feed.Url.GetIntHash();
                        feed.Status = feed.Status == 0 ? R.FeedStatus.ACTIVE : feed.Status;
                        feed.LastSuccessDate = feed.LastSuccessDate == DateTime.MinValue ? DateTime.Now : feed.LastSuccessDate;
                        var feedForm = new GenericForm<Feed>
                        {
                            ByUserID = form.ByUserID,
                            DataModel = feed
                        };
                        var feedID = feedService.SaveChanges(feedForm);

                        // add to connector only if it a new feed
                        if (feed.ID == 0)
                        {
                            // connect series to feed
                            var connectorForm = new ConnectorForm()
                            {
                                ByUserID = form.ByUserID,
                                ConnectorType = R.ConnectorType.SERIES_FEED,
                                SourceID = id,
                                TargetID = feedID
                            };
                            connectorService.SaveChanges(connectorForm);
                        }
                    }
                }
                if (form.Groups != null || form.InlineEditProperty == form.PropertyName(m => m.Groups))
                {
                    foreach (var group in form.Groups)
                    {
                        // connect series to feed
                        var connectorForm = new ConnectorForm()
                        {
                            ByUserID = form.ByUserID,
                            ConnectorType = R.ConnectorType.SERIES_GROUP,
                            SourceID = id,
                            TargetID = group.ID
                        };
                        connectorService.SaveChanges(connectorForm);
                    }
                }
                if (form.Authors != null || form.InlineEditProperty == form.PropertyName(m => m.Authors))
                {
                    foreach (var author in form.Authors)
                    {
                        // connect series to feed
                        var connectorForm = new ConnectorForm()
                        {
                            ByUserID = form.ByUserID,
                            ConnectorType = R.ConnectorType.SERIES_AUTHOR,
                            SourceID = id,
                            TargetID = author.ID
                        };
                        connectorService.SaveChanges(connectorForm);
                    }
                }
                if (form.Akas != null || form.InlineEditProperty == form.PropertyName(m => m.Akas))
                {
                    var akaService = new AkaService(uow);
                    foreach (var aka in form.Akas)
                    {
                        var akaForm = new AkaForm
                        {
                            ByUserID = form.ByUserID,
                            SourceID = form.ID,
                            SourceTable = R.SourceTable.SERIES
                        };
                        new PropertyMapper<Aka, AkaForm>(aka, akaForm).Map();
                        var akaID = akaService.SaveChanges(akaForm);
                    }
                }

                return id;
            }
        }

        public SeriesDetail GetSeries(SeriesCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new SeriesService(uow);
                var detail = service.Get(criteria);

                var connectorTypes = new[]
                {
                    R.ConnectorType.SERIES_TAGCATEGORY,
                    R.ConnectorType.SERIES_TAGGENRE,
                    R.ConnectorType.SERIES_TAGTHEME,
                    R.ConnectorType.SERIES_TAGCONTAIN,
                    R.ConnectorType.SERIES_GROUP,
                    R.ConnectorType.SERIES_AUTHOR,
                    R.ConnectorType.SERIES_PUBLISHER,
                    R.ConnectorType.SERIES_GLOSSARY,
                    R.ConnectorType.SERIES_FEED,
                    R.ConnectorType.SERIES_USERLIST,
                };
                var connectors = service.View<Connector>().Where(w => w.IsDeleted == false && connectorTypes.Contains(w.ConnectorType) && w.SourceID == detail.ID).ToList();

                var tagTypes = new[] { R.TagType.CATEGORY, R.TagType.GENRE, R.TagType.CONTAIN };
                var tags = service.View<Tag>().Where(w => tagTypes.Contains(w.TagType)).ToList();

                detail.Categories = tags.Where(w => w.TagType == R.TagType.CATEGORY && connectors.Any(a => a.ConnectorType == R.ConnectorType.SERIES_TAGCATEGORY && a.TargetID == w.ID)).ToList();

                detail.Genres = tags.Where(w => w.TagType == R.TagType.GENRE && connectors.Any(a => a.ConnectorType == R.ConnectorType.SERIES_TAGGENRE && a.TargetID == w.ID)).ToList();

                detail.Contains = tags.Where(w => w.TagType == R.TagType.CONTAIN && connectors.Any(a => a.ConnectorType == R.ConnectorType.SERIES_TAGCONTAIN && a.TargetID == w.ID)).ToList();

                var authorIDs = connectors.Where(w => w.ConnectorType == R.ConnectorType.SERIES_AUTHOR).Select(s=> s.TargetID).ToList();
                detail.Authors = service.View<Author>().Where(w => authorIDs.Contains(w.ID)).ToList();

                var groupIDs = connectors.Where(w => w.ConnectorType == R.ConnectorType.SERIES_GROUP).Select(s=> s.TargetID).ToList();
                detail.Groups = service.View<Group>().Where(w => groupIDs.Contains(w.ID)).ToList();

                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.SERIES && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();

                // get data for user lists
                detail.Connectors = connectors;

                detail.UserLists = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == criteria.ByUserID)
                    .OrderBy(o => o.Priority == 0 ? int.MaxValue : o.Priority).ThenBy(o => o.Name).ToList();

                detail.Releases = service.View<Release>().Where(w => w.SeriesID == detail.ID).ToList();

                detail.Feeds = service.View<Connector>()
                                .Where(w => w.ConnectorType == R.ConnectorType.SERIES_FEED && w.SourceID == detail.ID)
                                .Join(service.View<Feed>().All(), c => c.TargetID, f => f.ID, (c, f) => f).ToList();

                detail.Glossaries = detail.Glossaries = service.View<Glossary>().Where(w => w.SourceTable == R.SourceTable.SERIES && w.SourceID == detail.ID).ToList();

                detail.UserAction = new UserActionFacade().Get(new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.SERIES });

                detail.Akas = service.View<Aka>().Where(w => w.IsDeleted == false && w.SourceID == detail.ID && w.SourceTable == R.SourceTable.SERIES).ToList();
                return detail;
            }
        }

        public ReleaseDetail GetRelease(ReleaseCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ReleaseService(uow);
                var detail = service.Get(criteria);

                detail.Series = service.View<Series>().Where(w => w.ID == detail.SeriesID).SingleOrDefault() ?? new Series();

                detail.Group = service.View<Group>().Where(w => w.ID == detail.GroupID).SingleOrDefault() ?? new Group();

                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.RELEASE && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();

                // get data for user lists

                detail.Connectors = service.View<Connector>().Where(w => w.IsDeleted == false && w.SourceID == detail.SeriesID).ToList();

                detail.UserLists = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == criteria.ByUserID)
                    .OrderBy(o => o.Priority == 0 ? int.MaxValue : o.Priority).ThenBy(o => o.Name).ToList();

                detail.UserAction = new UserActionFacade().Get(new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.RELEASE });

                detail.Sticky = service.View<Sticky>().Where(w => w.SourceID == detail.ID && w.SourceTable == R.SourceTable.RELEASE).SingleOrDefault() ?? new Sticky();

                return detail;
            }
        }

        public int AddRelease(ReleaseForm releaseForm)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ReleaseService(uow);
                return service.SaveChanges(releaseForm);
            }
        }
    }
}
