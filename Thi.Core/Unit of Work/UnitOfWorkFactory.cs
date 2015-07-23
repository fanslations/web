using System;
using System.Data.Entity;

namespace Thi.Core
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork Create<T>() where T : DbContext
        {            
            var context = Activator.CreateInstance(typeof(T)) as DbContext;
            return new UnitOfWork(context);
        }

        void RequiredForSqlServerDLL()
        {
            System.Data.Entity.SqlServer.SqlFunctions.Char(1); // add this the EntityFramework.SqlServer.dll is copy to output directory
        }
    }
}
