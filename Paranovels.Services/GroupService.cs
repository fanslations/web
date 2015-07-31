using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class GroupService : BaseService
    {
        public GroupService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(GroupForm form)
        {
            var tGroup = Table<Group>();

            var group = tGroup.GetOrAdd(w => w.GroupID == form.GroupID);
            MapProperty(form, group, form.InlineEditProperty);
            UpdateAuditFields(group, form.ByUserID);
            // save
            SaveChanges();

            return group.GroupID;
        }

        public GroupDetail Get(GroupCriteria criteria)
        {
            var qGroup = View<Group>().All();

            if (criteria.IDToInt > 0)
            {
                qGroup = qGroup.Where(w => w.GroupID == criteria.IDToInt);
            }

            var group = qGroup.SingleOrDefault();
            if (group == null) return null;

            var groupDetail = new GroupDetail();
            MapProperty(group, groupDetail);

            return groupDetail;
        }

        public PagedList<GroupGrid> Search(SearchModel<GroupCriteria> searchModel)
        {
            var qGroup = View<Group>().All();
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.GROUP);

            var c = searchModel.Criteria;

            if (!string.IsNullOrWhiteSpace(c.Query))
            {
                var model = new Group();
                var columns = new[]
                {
                    model.PropertyName(m => m.Name),
                };
                qGroup = qGroup.Search(columns, c.Query.ToSearchKeywords()) as IQueryable<Group>;
            }

            var results = qGroup.GroupJoin(qSummarize, r => r.GroupID, s => s.SourceID,
                (r, s) => new {Group = r, Summarize = s.DefaultIfEmpty()})
                .SelectMany(sm => sm.Summarize.Select(s => new GroupGrid
                {
                    GroupID = sm.Group.GroupID,
                    UpdatedDate = sm.Group.UpdatedDate,
                    Name = sm.Group.Name,
                    Url = sm.Group.Url,
                    About = sm.Group.About,

                    CommentCount = s.CommentCount,
                    ViewCount = s.ViewCount,
                    VoteUp = s.VoteUp,
                    VoteDown = s.VoteDown,
                    QualityCount = s.QualityCount,
                    QualityScore = s.QualityScore

                }));

            // apply sort
            results = results.Sort(c);

            return results.ToPagedList(searchModel.PagedListConfig);
        }
    }
}
