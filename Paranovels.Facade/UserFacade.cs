using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paranovels.Common;
using Paranovels.DataModels;
using Thi.Core;
using Paranovels.Services;
using Paranovels.ViewModels;

using Paranovels.DataAccess;

namespace Paranovels.Facade
{
    public class UserFacade : IFacade
    {
        public int AddUser(UserForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserService(uow);
                return service.SaveChanges(form);
            }
        }

        public AuthSession GetAuthSession(UserCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserService(uow);
                var userDetail = service.Get(criteria);
                
                if(userDetail == null)
                {
                    var form = new UserForm
                    {
                        Username =  criteria.Username,
                        Location = criteria.Username,
                    };
                    var id = service.SaveChanges(form);
                    return GetAuthSession(criteria);
                }

                var session = new AuthSession();
                session.UserID = userDetail.UserID;
                session.Username = userDetail.Username;
                session.FirstName = userDetail.FirstName;
                session.LastName = userDetail.LastName;
                session.Email = userDetail.Email;

                return session;
            }
        }

        public UserDetail Get(UserCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new UserService(uow);
                var detail = service.Get(criteria);

                detail.Preferences = service.View<UserPreference>().Where(w => w.UserID == detail.UserID).ToList();

                return detail;
            }
        }

        public int AddPreference(PreferenceForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new PreferenceService(uow);
                return service.SaveChanges(form);
            }
        }
    }
}
