using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Text;

namespace Thi.Core
{
    /// <summary>
    /// Class - EFUnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties

        /// <summary>
        /// Property - Db context
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        /// Property - Lazy loading enabled
        /// </summary>
        public bool LazyLoadingEnabled
        {
            get { return Context.Configuration.ProxyCreationEnabled; }
            set { Context.Configuration.LazyLoadingEnabled = value; }
        }

        /// <summary>
        /// Property - Proxy creation enabled
        /// </summary>
        public bool ProxyCreationEnabled
        {
            get { return Context.Configuration.ProxyCreationEnabled; }
            set { Context.Configuration.ProxyCreationEnabled = value; }
        }

        /// <summary>
        /// Property - Connection string
        /// </summary>
        public string ConnectionString
        {
            get { return Context.Database.Connection.ConnectionString; }
            set { Context.Database.Connection.ConnectionString = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor - Creates a new EFUnitOfWork
        /// </summary>
        public UnitOfWork(DbContext context)
        {
            // set to 'true' to remove IS NULL check
            context.Configuration.UseDatabaseNullSemantics = true;

            Context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method - Save changes
        /// </summary>
        public bool Commit()
        {
            try
            {
                return Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                var errors = new List<string>();
                foreach (DbEntityValidationResult entityErr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityErr.ValidationErrors)
                    {
                        errors.Add(string.Format("Error Property Name {0} : Error Message: {1}", error.PropertyName, error.ErrorMessage));
                    }
                }
                throw new Exception(string.Join(Environment.NewLine, errors), dbEx);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Error '{0}' occured in UnitOfWork.Commit().", ex.Message), ex);
            }
        }

        /// <summary>
        /// Create a repository of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> Repository<T>() where T : class, new()
        {
            return new Repository<T>(this);
        }

        /// <summary>
        /// Method - Dispose
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }

        #endregion
    }
}