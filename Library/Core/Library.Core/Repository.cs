using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
namespace Library.Core
{
    public class Repository<T> where T : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        /// <summary>
        ///   Get the total objects count.
        /// </summary>
        public virtual int Count
        {
            get { return _dbSet.Count(); }
        }

        /// <summary>
        ///   Get the total objects count.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int CountBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        /// <summary>
        /// Returns if the collection has any items in it.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        /// <summary>
        ///   Gets all objects from database
        /// </summary>
        public virtual IQueryable<T> All()
        {
            return _dbSet.AsQueryable();
        }

        /// <summary>
        ///   Gets object by primary key.
        /// </summary>
        /// <param name="id"> primary key </param>
        /// <returns> </returns>
        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        ///   Gets objects via optional filter, sort order, and includes
        /// </summary>
        /// <param name="filter"> </param>
        /// <param name="orderBy"> </param>
        /// <param name="includeProperties"> </param>
        /// <returns> </returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var oProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(oProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }

            return query.AsQueryable();
        }

        /// <summary>
        ///   Gets objects via optional filter, sort order, and includes
        /// </summary>
        /// <param name="filter"> </param>
        /// <param name="orderBy"> </param>
        /// <param name="includeProperties"> </param>
        /// <returns> </returns>
        public IQueryable<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, out int total, string includeProperties = "", int index = 0, int size = 50)
        {
            var query = Get(filter, orderBy, includeProperties);
            return PageQuery(query, out total, index, size);
        }

        /// <summary>
        ///   Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate"> Specified a filter </param>
        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        /// <summary>
        ///   Gets objects from database with filting and paging.
        /// </summary>
        /// <param name="filter"> Specified a filter </param>
        /// <param name="total"> Returns the total records count of the filter. </param>
        /// <param name="index"> Specified the page index. </param>
        /// <param name="size"> Specified the page size </param>
        public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 0)
        {
            var query = filter != null ? _dbSet.Where(filter).AsQueryable() : _dbSet.AsQueryable();
            return PageQuery(query, out total, index, size);
        }

        /// <summary>
        ///   Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate"> Specified the filter expression </param>
        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        /// <summary>
        ///   Find object by keys.
        /// </summary>
        /// <param name="keys"> Specified the search keys. </param>
        public virtual T Find(params object[] keys)
        {
            return _dbSet.Find(keys);
        }

        /// <summary>
        ///   Find object by specified expression.
        /// </summary>
        /// <param name="predicate"> </param>
        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        ///   Create a new object to database.
        /// </summary>
        /// <param name="entity"> Specified a new object to create. </param>
        public virtual T Create(T entity)
        {
            var newEntry = _dbSet.Add(entity);
            return newEntry;
        }

        /// <summary>
        ///   Deletes the object by primary key
        /// </summary>
        /// <param name="id"> </param>
        public virtual void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        ///   Delete the object from database.
        /// </summary>
        /// <param name="entity"> Specified a existing object to delete. </param>
        public virtual void Delete(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        /// <summary>
        ///   Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"> </param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            var oEntitiesToDelete = Filter(predicate);
            foreach (var oEntity in oEntitiesToDelete)
            {
                if (_dbContext.Entry(oEntity).State == EntityState.Detached)
                {
                    _dbSet.Attach(oEntity);
                }
                _dbSet.Remove(oEntity);
            }
        }

        /// <summary>
        ///   Update object changes and save to database.
        /// </summary>
        /// <param name="entity"> Specified the object to save. </param>
        public virtual void Update(T entity)
        {
            var oEntry = _dbContext.Entry(entity);

            if (oEntry.State == EntityState.Detached)
                _dbSet.Attach(entity);

            oEntry.State = EntityState.Modified;
        }

        #region Helpers

        /// <summary>
        ///  Helps for Paging the records
        /// </summary>
        /// <param name="query">Records</param>
        /// <param name="total">Total Pages</param>
        /// <param name="index">Current Page</param>
        /// <param name="size">Display Size</param>
        /// <returns>IQueryable<T></returns>
        private IQueryable<T> PageQuery(IQueryable<T> query, out int total, int index = 0, int size = 0)
        {
            int skip = index * size;
            total = query.Count();

            int remainder = (total % size) != 0 ? 1 : 0;
            total = (total / size) + remainder;

            return skip == 0 ? query.Take(size) : query.Skip(skip).Take(size);
        }

        #endregion Helpers
    }
}