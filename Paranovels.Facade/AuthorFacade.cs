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
    public class AuthorFacade : IFacade
    {
        public PagedList<AuthorGrid> Search(SearchModel<AuthorCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AuthorService(uow);
                var pagedList = service.Search(searchModel);

                return pagedList;
            }
        }

        public AuthorDetail Get(AuthorCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AuthorService(uow);
                var detail = service.Get(criteria);

                detail.Releases = new List<Release>();
                detail.Series = new List<Series>();
                detail.Summarize = new Summarize();
                detail.UserAction = new UserActionDetail();
                
                return detail;
            }
        }

        public int AddAuthor(AuthorForm authorForm)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AuthorService(uow);
                return service.SaveChanges(authorForm);
            }
        }
    }
}
