using System;
using System.Data.Entity;

namespace Thi.Core
{
    /// <summary>
    /// Interface - Interface of Entity Framework unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        /// <summary>
        /// Property - Gets or sets the context
        /// </summary>
        /// <value>
        /// The context
        /// </value>
        DbContext Context { get; set; }

        /// <summary>
        /// Property - Gets or sets a value indicating whether [lazy loading enabled]
        /// </summary>
        /// <value>
        /// <c>true</c> if [lazy loading enabled]; otherwise, <c>false</c>
        /// </value>
        bool LazyLoadingEnabled { get; set; }

        /// <summary>
        /// Property - Gets or sets a value indicating whether [proxy creation enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [proxy creation enabled]; otherwise, <c>false</c>
        /// </value>
        bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// Property - Gets or sets the connection string
        /// </summary>
        /// <value>
        /// The connection string
        /// </value>
        string ConnectionString { get; set; }

        #endregion

        #region Definitions

        /// <summary>
        /// Definition - Commits this instance
        /// </summary>
        bool Commit();

        /// <summary>
        /// Repositories this instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> Repository<T>() where T : class, new();

        #endregion
    }
}