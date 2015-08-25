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
    public class GlossaryFacade : IFacade
    {
        public GlossaryDetail Get(GlossaryCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GlossaryService(uow);
                var detail = service.Get(criteria);

                detail.Summarize = service.View<Summarize>().Where(w => w.SourceTable == R.SourceTable.GLOSSARY && w.SourceID == detail.ID).SingleOrDefault() ?? new Summarize();

                detail.UserAction = new UserActionFacade().Get(new ViewForm { UserID = criteria.ByUserID, SourceID = detail.ID, SourceTable = R.SourceTable.GLOSSARY });

                return detail;
            }
        }

        public int AddGlossary(GlossaryForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GlossaryService(uow);
                var results = service.SaveChanges(form);

                return results;
            }
        }

        public PagedList<GlossaryGrid> Search(SearchModel<GlossaryCriteria> searchModel)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new GlossaryService(uow);
                var results = service.Search(searchModel);

                return results;
            }
        }
    }
}
