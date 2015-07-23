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
    public class UserService : BaseService
    {
        public UserService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int SaveChanges(UserForm form)
        {
            var tUser = Table<User>();

            var user = tUser.GetOrAdd(w => w.UserID == form.UserID || w.Username == form.Username);
            UpdateAuditFields(user, form.ByUserID);
            MapProperty(form, user, form.InlineEditProperty);
            // save
            SaveChanges();

            return user.UserID;
        }

        public UserDetail Get(UserCriteria criteria)
        {
            var qUser = View<User>().All();
            if (!string.IsNullOrWhiteSpace(criteria.Username))
            {
                qUser = qUser.Where(w => w.Username == criteria.Username);
            }

            var user = qUser.SingleOrDefault();
            if (user == null) return null;

            var detail = new UserDetail();
            MapProperty(user, detail);

            return detail;
        }

        public PagedList<User> Search(SearchModel<UserCriteria> searchModel)
        {
            var qUser = View<User>().All();

            var c = searchModel.Criteria;
            if (c.UserIDs != null && c.UserIDs.Any())
            {
                qUser = qUser.Where(w => c.UserIDs.Contains(w.UserID));
            }

            return qUser.ToPagedList(searchModel.PagedListConfig);
        }
    }
}
