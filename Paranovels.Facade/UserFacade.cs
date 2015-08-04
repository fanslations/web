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
                var detail = service.Get(criteria);
                
                if(detail == null)
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
                session.UserID = detail.UserID;
                session.Username = detail.Username;
                session.FirstName = detail.FirstName;
                session.LastName = detail.LastName;
                session.Email = detail.Email;

                criteria.ByUserID = detail.UserID;
                session.HiddenSeriesIDs = GetHiddenSeriesIDs(criteria);

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

                detail.HideSeriesIDs = GetHiddenSeriesIDs(criteria);

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

        public IList<int> GetHiddenSeriesIDs(BaseCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                
                var service = new UserService(uow);
                // start - get series IDs that user don't want to see via preferences
                var connectorTypes = new[] { R.ConnectorType.SERIES_TAGCATEGORY, R.ConnectorType.SERIES_TAGGENRE };
                var preferenceTypes = new[] { R.PreferenceType.CATEGORY, R.PreferenceType.GENRE };
                var qConnector = service.View<Connector>().Where(w => connectorTypes.Contains(w.ConnectorType));
                var qUserPreference = service.View<UserPreference>().Where(w => w.UserID == criteria.ByUserID && preferenceTypes.Contains(w.Type) && w.SourceTable == R.SourceTable.TAG);

                var hiddenSeriesIDs = qConnector.Join(qUserPreference, cn => cn.TargetID, up => up.SourceID,
                    (cn, up) => new { cn.SourceID, up.Score })
                    .GroupBy(g => new { g.SourceID, g.Score })
                    .Where(w => w.Sum(s => s.Score) < 0)
                    .Select(s => s.Key.SourceID);

                // end
                // start - get series IDs that user don't want to vee via user lists
                var hiddenUserListIDs = service.View<UserList>().Where(w => w.IsDeleted == false && w.UserID == criteria.ByUserID && w.IsHiddenInFrontpage == true).Select(s => s.UserListID).ToList();
                var qConnector2 = service.View<Connector>().Where(w => w.ConnectorType == R.ConnectorType.SERIES_USERLIST);

                var hiddenSeriesIDs2 = qConnector2.Where(w => w.ConnectorType == R.ConnectorType.SERIES_USERLIST && hiddenUserListIDs.Contains(w.TargetID)).Select(s => s.SourceID);
                // end

                return  hiddenSeriesIDs.Union(hiddenSeriesIDs2).ToList();
            }
        }

    }
}
