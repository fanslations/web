using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Thi.Core
{
    /// <summary>
    /// Class - EFRepository provides the business logic methods
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        #region Fields

        /// <summary>
        /// Field - Interface for DbSet
        /// </summary>
        private IDbSet<T> _objectset;

        #endregion

        #region Properties

        /// <summary>
        /// Property - Interface for unit of work
        /// </summary>
        private IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Property - Interface for object set
        /// </summary>
        private IDbSet<T> ObjectSet
        {
            get
            {
                if (_objectset == null)
                {
                    _objectset = UnitOfWork.Context.Set<T>();
                }
                return _objectset;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor - Creates a new EFRepository instance based on unit of work interface
        /// </summary>
        /// <param name="unitOfWork">unit of work inerface</param>
        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method - Return all 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> All()
        {
            return ObjectSet.AsQueryable();
        }

        /// <summary>
        /// Method - Where 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return ObjectSet.Where(expression);
        }

        /// <summary>
        /// Method - Add a new entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            ObjectSet.Add(entity);
        }

        /// <summary>
        /// Method - Get or create a entity
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public T GetOrAdd(Expression<Func<T, bool>> expression)
        {
            var model = ObjectSet.Where(expression).SingleOrDefault();
            if (model == null)
            {
                model = new T();
                ObjectSet.Add(model);
            }
            return model;
        }

        /// <summary>
        /// Method - Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            ObjectSet.Remove(entity);
        }

        /// <summary>
        /// method - Dispose
        /// </summary>
        public void Dispose()
        {
            _objectset = null;
            UnitOfWork.Dispose();
        }

        #endregion
    }
}
