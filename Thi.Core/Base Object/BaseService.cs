using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Thi.Core
{
    public class BaseService
    {
        protected IUnitOfWork _uow;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        protected void UpdateAuditFields<T>(T model, int userID) where T : class, IDataModel, new()
        {
            if (model.InsertedBy == 0)
            {
                model.InsertedBy = userID;
                model.InsertedDate = DateTime.Now;
            }
            model.UpdatedBy = userID;
            model.UpdatedDate = DateTime.Now;
        }

        protected void MapProperty<T, T1>(T fromModel, T1 toModel, string inlineEditProperty = null)
            where T : class, new()
            where T1 : class, new()
        {
            new PropertyMapper<T, T1>(fromModel, toModel).Map(inlineEditProperty);
        }

        protected IRepository<T> Table<T>() where T : class, IDataModel, new()
        {
            return _uow.Repository<T>();
        }

        public IReadOnlyRepository<T> View<T>() where T : class, IDataModel, new()
        {
            return _uow.Repository<T>();
        }

        protected bool SaveChanges()
        {
            return _uow.Commit();
        }
    }
}
