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
    public class ListFacade : IFacade
    {
        public int AddList(ListForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ListService(uow);
                return service.SaveChanges(form);
            }
        }

        public PagedList<UserList> Search(SearchModel<ListCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ListService(uow);

                return service.Search(searchModel);
            }
        }

        public ListDetail Get(ListCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new ListService(uow);

                var detail = service.Get(criteria);

                var listSeriesIDs = service.View<Connector>()
                        .Where(w => w.IsDeleted == false && w.ConnectorType == R.ConnectorType.SERIES_USERLIST)
                        .Where(w => w.TargetID == detail.UserListID).Select(s => s.SourceID).ToList();

                detail.Series = new SeriesService(uow).Search(new SearchModel<SeriesCriteria>
                {
                    Criteria = new SeriesCriteria { IDs = listSeriesIDs },
                    PagedListConfig = new PagedListConfig { PageSize = int.MaxValue }
                }).Data;

                detail.Releases = service.View<Release>().Where(w => listSeriesIDs.Contains(w.SeriesID)).ToList();

                var releaseIDs = detail.Releases.Select(s => s.ReleaseID);
                detail.Reads = service.View<UserRead>().Where(w => w.UserID == criteria.ByUserID)
                    .Where(w => w.SourceTable == R.SourceTable.RELEASE)
                    .Where(w => releaseIDs.Contains(w.SourceID)).ToList();


                return detail;
            }
        }

        public IList<UserList> GetListTemplates()
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                return uow.Repository<UserList>().Where(w => w.IsDeleted == false && w.UserID == 0).ToList();
            }
        }
    }
}
