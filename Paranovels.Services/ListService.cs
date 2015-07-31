using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            var userList = tUserList.GetOrAdd(w => w.UserListID == form.UserListID || (w.UserID == form.UserID && w.Name == form.Name));
            MapProperty(form, userList, form.InlineEditProperty);
            UpdateAuditFields(userList, form.ByUserID);

            // override
            if (form.InlineEditProperty == form.PropertyName(m => m.ShareLevel))
            {
                userList.ShareLevel = form.ShareLevel;
            }

            // save
            SaveChanges();

            return userList.UserListID;
        }

        public PagedList<UserList> Search(SearchModel<ListCriteria> searchModel)
        {
                       
                var c = searchModel.Criteria;

                var tUserList = Table<UserList>().Where(w => w.UserID == c.ByUserID);

                return tUserList.ToPagedList(searchModel.PagedListConfig);
        }

        public ListDetail Get(ListCriteria criteria)
        {
            var qUserList = View<UserList>().All();

            var userList = qUserList.SingleOrDefault(w => w.UserListID == criteria.IDToInt);

            if (userList == null) return null;

            var listDetail = new ListDetail();
            MapProperty(userList, listDetail);

            return listDetail;
        }
    }

}
