using System;
using System.Linq;
using System.Linq.Expressions;

namespace Thi.Core
{
    public interface IReadOnlyRepository<T>
    {
        /// <summary>
        /// Definition - All entities of this instance
        /// </summary>
        /// <returns>IQueryable fo the entities</returns>
        IQueryable<T> All();

        /// <summary>
        /// Definition - Find all entities  that matched the specified expression
        /// </summary>
        /// <param name="expression">The expression</param>
        /// <returns>IQueryable fo the entities</returns>
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
    }
}