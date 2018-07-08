using DataLayer.MemberMgr;
using DataLayer.MemberMgr.Repositories;
using Library.Core;
using System;

namespace Service.MemberMgr.Service
{
    public class ServiceBase : GenericClassBase, IDisposable
    {
        #region Private Vars 
        private MemberManagerDbContext _dbContext = null;
        private MemberDataAccess _memberDataAccess = null;
        #endregion Private Vars

        #region Ctor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        protected ServiceBase(string connStr)
        {
            _dbContext = new MemberManagerDbContext(connStr);
        }
        #endregion Ctor
         
        #region Properties

        protected MemberDataAccess DataAccess => _memberDataAccess ?? (_memberDataAccess = new MemberDataAccess(_dbContext));
         
        #endregion Properties
         
        #region Methods

        public string GenerateNewKey(bool removeDash)
        {
            Guid key = Guid.NewGuid();
            if (removeDash)
                return key.ToString("N");
            else
                return key.ToString(); 
        }

        public void Dispose()
        {
            if (_dbContext != null)  
                _dbContext = null;

            if (_memberDataAccess != null)
                _memberDataAccess.Dispose();

        }

        #endregion Methods
    }
}
