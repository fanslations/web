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

                if (form.InlineEditProperty == form.PropertyName(m => m.Feeds))
                {
                    var feedService = new FeedService(uow);
                    var feedForm = new GenericForm<Feed>
                    {
                        ByUserID = form.ByUserID,
                        DataModel = new Feed
                        {
                            Url = form.Feeds,
                            UrlHash = form.Feeds.GetIntHash(),
                            Status = R.FeedStatus.ACTIVE,
                        }
                    };
                    var feedID = feedService.SaveChanges(feedForm);

                    // connect group to feed
                    var connectorService = new ConnectorService(uow);
                    var connectorForm = new ConnectorForm()
                    {
                        ByUserID = form.ByUserID,
                        ConnectorType = R.ConnectorType.GROUP_FEED,
                        SourceID = id,
                        TargetID = feedID
                    };
                    connectorService.SaveChanges(connectorForm);
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
                        var connectorService = new ConnectorService(uow);
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

                detail.Releases = service.View<Release>().Where(w => w.GroupID == detail.GroupID).ToList();

                detail.Feeds = service.View<Connector>()
                        .Where(w => w.ConnectorType == R.ConnectorType.GROUP_FEED && w.SourceID == detail.GroupID)
                        .Join(service.View<Feed>().All(), c => c.TargetID, f => f.FeedID, (c, f) => f).ToList();

                detail.Glossaries = service.View<Connector>()
                    .Where(w => w.ConnectorType == R.ConnectorType.GROUP_GLOSSARY && w.SourceID == detail.GroupID)
                    .Join(service.View<Glossary>().All(), c => c.TargetID, f => f.GlossaryID, (c, f) => f).ToList();

                return detail;
            }
        }

        public bool CheckFeedUpdate(int minutes = -30)
        {
            var lastCheckedDate = DateTime.Now.AddMinutes(minutes);
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ConnectorService(uow);
                var connectorFeed = service.View<Connector>().Where(w => w.ConnectorType == R.ConnectorType.GROUP_FEED)
                    .Join(service.View<Feed>().Where(w => w.Status == R.FeedStatus.ACTIVE && w.UpdatedDate < lastCheckedDate),
                        c => c.TargetID, f => f.FeedID, (c, f) => new { c.SourceID, f.UpdatedDate }).OrderBy(o => o.UpdatedDate).FirstOrDefault();

                if (connectorFeed != null)
                {
                    var facade = new FeedFacade();
                    facade.CheckFeed(new GroupCriteria { ID = connectorFeed.SourceID });
                    return true;
                }
                return false;
            }
        }
    }
}
