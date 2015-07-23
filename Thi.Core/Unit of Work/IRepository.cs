using System;
using System.Linq;
using System.Linq.Expressions;

namespace Thi.Core
{
    /// <summary>
    /// Interface - Interface of Entity Framework repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T>, IDisposable
    {
        #region Definitions

        /// <summary>
        /// Get entity or create new one if not exist
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T GetOrAdd(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity">The entity</param>
        void Add(T entity);

        /// <summary>
        /// Deletes the specified entity
        /// </summary>
        /// <param name="entity">The entity</param>
        void Delete(T entity);

        #endregion
    }
}

