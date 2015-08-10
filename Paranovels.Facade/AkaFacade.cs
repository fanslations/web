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
    public class AkaFacade : IFacade
    {

        public int AddAka(AkaForm form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AkaService(uow);
                var id = service.SaveChanges(form);
                return id;
            }
        }

        public AkaDetail Get(BaseCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var service = new AkaService(uow);
                var detail = service.Get(criteria);

                return detail;
            }
        }
    }
}
