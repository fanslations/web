using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.DataAccess;
using Paranovels.DataModels;
using Paranovels.Services;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Facade
{
    public class QueryFacade : IFacade
    {
        public IList<Tag> SearchTag(TagCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new TagService(uow);
                return service.Search(new SearchModel<TagCriteria>
                {
                    Criteria = criteria,
                    PagedListConfig = new PagedListConfig() {PageSize = 999999}
                }).Data;
            }
        }

        public IList<Group> SearchGroup(GroupCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GroupService(uow);
                return service.Search(new SearchModel<GroupCriteria>
                {
                    Criteria = criteria,
                    PagedListConfig = new PagedListConfig() { PageSize = 999999 }
                }).Data;
            }
        }

        public IList<int> GetVotes(VoteCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var tUserVote = uow.Repository<UserVote>();
                var userVotedIDs = tUserVote.Where(w => w.UserID == criteria.ByUserID && w.SourceTable == criteria.SourceTable).Select(s => s.SourceID).ToList();

                return userVotedIDs;
            }
        }

        public IList<Series> SearchSeries(SeriesCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var tSeries = uow.Repository<Series>();

                var series = tSeries.Where(w => w.Title.Contains(criteria.Query));

                return series.ToList();
            }
        }
    }
}
