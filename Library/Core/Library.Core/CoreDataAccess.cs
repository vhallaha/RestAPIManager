using System;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Library.Core
{
    public class CoreDataAccess<T> : GenericClassBase, IDisposable where T : DbContext
    {

        #region Generics

        protected readonly T _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        public CoreDataAccess(string connStr)
        {
            var ctor = GetCtor<T>(typeof(string));
            if (ctor == null)
                throw new Exception(InvalidModel_Message);

            _dbContext = ctor(connStr);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Data Access</param>
        public CoreDataAccess(T dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Generics

        #region Methods

        /// <summary>
        /// Database Save
        /// </summary>
        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var ex = new FormattedDbException(e);
                throw ex;
            }
        }

        /// <summary>
        /// Database Save Async
        /// </summary>
        public void SaveAsync()
        {
            _dbContext.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
