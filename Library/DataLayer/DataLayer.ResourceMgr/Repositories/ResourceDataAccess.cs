using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Repositories
{
    public class ResourceDataAccess : CoreDataAccess<ResourceManagerDbContext>
    {

        #region Generics

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        public ResourceDataAccess(string connStr)
            : base(connStr)
        { 
        }

        public ResourceDataAccess(ResourceManagerDbContext dbContext)
            : base(dbContext)
        { 
        }

        #endregion Generics

        #region Private Vars

        private Repository<Resource> _resource;
        private Repository<ResourceClaim> _resourceClaim;
        private Repository<ResourceSettings> _resourceSettings;
         
        #endregion Private Vars

        #region Repositories

        public Repository<Resource> ResourceManager => _resource ?? (_resource = new Repository<Resource>(_dbContext));
        public Repository<ResourceClaim> ResourceClaim => _resourceClaim ?? (_resourceClaim = new Repository<ResourceClaim>(_dbContext));
        public Repository<ResourceSettings> ResourceSettings => _resourceSettings ?? (_resourceSettings = new Repository<ResourceSettings>(_dbContext));
         
        #endregion Repositories

        #region Methods

        public override void Dispose()
        {
            _resource = null;
            _resourceClaim = null;
            _resourceSettings = null;
        }

        #endregion Methods

    }
}
