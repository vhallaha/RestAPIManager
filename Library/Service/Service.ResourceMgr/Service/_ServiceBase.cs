using DataLayer.ResourceMgr;
using DataLayer.ResourceMgr.Repositories;
using Library.Core;
using System;

namespace Service.ResourceMgr.Service
{
    public class ServiceBase : GenericClassBase, IDisposable
    {

        #region Private Vars
        private ResourceManagerDbContext _dbContext = null;
        private ResourceDataAccess _resourceDataAccess = null;
        private ClientManagerDataAccess _clientDataAccess = null;
        #endregion Private Vars

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        protected ServiceBase(string connStr)
        {
            _dbContext = new ResourceManagerDbContext(connStr);
        }

        #endregion Ctor

        #region Properties

        protected ResourceDataAccess ResourceDataAccess => _resourceDataAccess ?? (_resourceDataAccess = new ResourceDataAccess(_dbContext));
        protected ClientManagerDataAccess ClientDataAccess => _clientDataAccess ?? (_clientDataAccess = new ClientManagerDataAccess(_dbContext));
         
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

            if (_resourceDataAccess != null)
                _resourceDataAccess.Dispose(); 
        }

        #endregion Properties
    }
}
