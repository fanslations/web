using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paranovels.Common;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Facade;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Tests.Paranovels.Services
{
    [TestClass]
    public class Group_UnitTest
    {
        [TestMethod]
        public void Updating_feed_url_test()
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var facade = new UpdateFacade();
                var service = new GroupService(uow);

                var groupsWithFeedUrl = service.View<Connector>().Where(w=>w.ConnectorType == R.ConnectorType.GROUP_FEED).Select(s=> s.SourceID).ToList();
                var groupsWithoutFeedUrl = service.View<Group>().Where(w=> !string.IsNullOrEmpty(w.Url) && !groupsWithFeedUrl.Contains(w.GroupID)).ToList();

                foreach (var g in groupsWithoutFeedUrl.Where(w=> w.Url.Contains("blogspot")))
                {
                    var groupForm = new GroupForm
                    {
                        ID = g.GroupID,
                        InlineEditProperty = "Feeds",
                        Feeds = g.Url.Trim(new[] { '/', ' ' }) + "/feeds/posts/default?alt=rss"
                    };
                    facade.SaveChanges(groupForm);
                }
            }
            
        }
    }
}
