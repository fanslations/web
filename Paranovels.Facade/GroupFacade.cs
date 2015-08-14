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
    public class GroupFacade : IFacade
    {
        public int AddGroup(GroupForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GroupService(uow);
                var id = service.SaveChanges(form);

                var connectorService = new ConnectorService(uow);

                if (form.Feeds != null || form.InlineEditProperty == form.PropertyName(m => m.Feeds))
                {
                    var feedService = new FeedService(uow);
                    foreach (var feed in form.Feeds)
                    {
                        feed.UrlHash = feed.Url.GetIntHash();
                        feed.Status = feed.Status == 0 ? R.FeedStatus.ACTIVE : feed.Status;
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
                                ConnectorType = R.ConnectorType.GROUP_FEED,
                                SourceID = id,
                                TargetID = feedID
                            };
                            connectorService.SaveChanges(connectorForm);
                        }
                    }
                }

                if (form.Glossaries != null && form.InlineEditProperty == form.PropertyName(m => m.Glossaries))
                {
                    foreach (var glossary in form.Glossaries)
                    {
                        var glossaryService = new GlossaryService(uow);
                        var glossaryForm = new GlossaryForm();
                        new PropertyMapper<Glossary, GlossaryForm>(glossary, glossaryForm).Map();
                        glossaryForm.ByUserID = form.ByUserID;

                        var glossaryID = glossaryService.SaveChanges(glossaryForm);

                        // connect group to glossary
                        var connectorForm = new ConnectorForm()
                        {
                            ByUserID = form.ByUserID,
                            ConnectorType = R.ConnectorType.GROUP_GLOSSARY,
                            SourceID = id,
                            TargetID = glossaryID
                        };
                        connectorService.SaveChanges(connectorForm);
                    }
                }
                return id;
            }
        }

        public GroupDetail GetGroup(GroupCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GroupService(uow);
                var detail = service.Get(criteria);

                detail.Series = service.View<Connector>()
                        .Where(w => w.IsDeleted == false && w.ConnectorType == R.ConnectorType.SERIES_GROUP && w.TargetID == detail.ID)
                        .Join(service.View<Series>().All(), c => c.SourceID, s => s.ID, (c, s) => s).ToList();

                detail.Releases = service.View<Release>().Where(w => w.GroupID == detail.ID).ToList();

                detail.Feeds = service.View<Connector>()
                        .Where(w => w.ConnectorType == R.ConnectorType.GROUP_FEED && w.SourceID == detail.ID)
                        .Join(service.View<Feed>().All(), c => c.TargetID, f => f.ID, (c, f) => f).ToList();

                detail.Glossaries = service.View<Connector>()
                    .Where(w => w.ConnectorType == R.ConnectorType.GROUP_GLOSSARY && w.SourceID == detail.ID)
                    .Join(service.View<Glossary>().All(), c => c.TargetID, f => f.ID, (c, f) => f).ToList();

                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.GROUP && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();
                detail.UserAction = new UserActionFacade().Get(new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.GROUP });
                return detail;
            }
        }
    }
}
