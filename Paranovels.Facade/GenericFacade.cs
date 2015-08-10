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
    // try to implement generic facade to save and get data
    // not working yet!
    public class GenericFacade<T> : IFacade where T : class, IDataModel, new()
    {
        public int SaveChanges(T form)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                var table = uow.Repository<T>();

                var model = table.Where(w => w.ID == form.ID).SingleOrDefault() ?? new T();
                new PropertyMapper<T, T>(form, model).Map();
            }
            return 0;
        }

        public T Get(BaseCriteria criteria)
        {
            using (var uow = UnitOfWorkFactory.Create<NovelContext>())
            {
                return new T();
            }
        }
    }
}
