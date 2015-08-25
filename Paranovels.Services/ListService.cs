using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Paranovels.ViewModels;
using Thi.Core;

namespace Paranovels.Services
{
    public class ListService : BaseService
    {
        public ListService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public int SaveChanges(ListForm form)
        {
            var tUserList = Table<UserList>();

            var userList = tUserList.GetOrAdd(w => w.ID == form.ID || (w.UserID == form.UserID && w.Name == form.Name));
            MapProperty(form, userList, form.InlineEditProperty);
            UpdateAuditFields(userList, form.ByUserID);

            // override
            if (form.InlineEditProperty == form.PropertyName(m => m.ShareLevel))
            {
                userList.ShareLevel = form.ShareLevel;
            }

            // save
            SaveChanges();

            return userList.ID;
        }

        public PagedList<ListGrid> Search(SearchModel<ListCriteria> searchModel)
        {
            var c = searchModel.Criteria;

            var qUserList = View<UserList>().Where(w => w.IsDeleted == false);
            var qSummarize = View<Summarize>().Where(w => w.SourceTable == R.SourceTable.USERLIST);

            if (c.ByUserID > 0)
            {
                qUserList = qUserList.Where(w => w.UserID == c.ByUserID);
                qUserList = qUserList.OrderBy(o => o.Priority == 0 ? int.MaxValue : o.Priority).ThenBy(o => o.Name);

            }

            if (c.IsPublic)
            {
                qUserList = qUserList.Where(w => w.ShareLevel == R.ShareLevel.PUBLIC);

            }
            var results = qUserList.GroupJoin(qSummarize, l => l.ID, s => s.SourceID,
                (l, s) => new {UserList = l, Summarize = s.DefaultIfEmpty()})
                .SelectMany(sm => sm.Summarize.Select(s => new ListGrid
                {
                    ID = sm.UserList.ID,
                    InsertedBy = sm.UserList.InsertedBy,
                    InsertedDate = sm.UserList.InsertedDate,
                    UpdatedBy = sm.UserList.UpdatedBy,
                    UpdatedDate = sm.UserList.UpdatedDate,
                    IsDeleted = sm.UserList.IsDeleted,
                    UserID = sm.UserList.UserID,
                    Type = sm.UserList.Type,
                    Name = sm.UserList.Name,
                    Description = sm.UserList.Description,
                    ShareLevel = sm.UserList.ShareLevel,
                    Priority = sm.UserList.Priority,
                    Icon = sm.UserList.Icon,
                    Color = sm.UserList.Color,
                    IsAutoAddWhenRead = sm.UserList.IsAutoAddWhenRead,
                    IsColorIcon = sm.UserList.IsColorIcon,
                    IsEmailNotifyOfNewRelease = sm.UserList.IsEmailNotifyOfNewRelease,
                    IsHiddenInFrontpage = sm.UserList.IsHiddenInFrontpage,
                    IsNotifyOfNewRelease = sm.UserList.IsNotifyOfNewRelease,
                    IsPlaceInFrontOfTitle = sm.UserList.IsPlaceInFrontOfTitle,

                    CommentCount = s.CommentCount,
                    ViewCount = s.ViewCount,
                    VoteUp = s.VoteUp,
                    VoteDown = s.VoteDown,
                    QualityCount = s.QualityCount,
                    QualityScore = s.QualityScore

                }));

            // apply sorted
            if (c.ByUserID > 0)
            {
                results = results.OrderBy(o => o.Priority == 0 ? int.MaxValue : o.Priority).ThenBy(o => o.Name);
            }
            results = results.Sort(c);

            return results.ToPagedList(searchModel.PagedListConfig);
        }

        public ListDetail Get(ListCriteria criteria)
        {
            var qUserList = View<UserList>().All();

            var userList = qUserList.SingleOrDefault(w => w.ID == criteria.IDToInt);

            if (userList == null) return null;

            var listDetail = new ListDetail();
            MapProperty(userList, listDetail);

            return listDetail;
        }
    }

}
