using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Repositories
{
    public class ClientManagerDataAccess : CoreDataAccess<ResourceManagerDbContext>
    {

        #region Generic 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String</param>
        public ClientManagerDataAccess(string connStr)
            : base(connStr)
        {
        }

        public ClientManagerDataAccess(ResourceManagerDbContext dbContext)
            : base(dbContext)
        {
        }

        #endregion Generic

        #region Private Vars

        private Repository<Client> _client;
        private Repository<ClientKey> _clientKey;
        private Repository<ClientResourceAccess> _clientResourceAccess;
        private Repository<ClientResourceAccessClaim> _clientResourceAccessClaim;

        #endregion Private Vars

        #region Public Vars

        public Repository<Client> Client => _client ?? (_client = new Repository<Client>(_dbContext));
        public Repository<ClientKey> ClientKey => _clientKey ?? (_clientKey = new Repository<ClientKey>(_dbContext));
        public Repository<ClientResourceAccess> ClientResourceAccess => _clientResourceAccess ?? (_clientResourceAccess = new Repository<ClientResourceAccess>(_dbContext));
        public Repository<ClientResourceAccessClaim> ClientResourceAccessClaim => _clientResourceAccessClaim ?? (_clientResourceAccessClaim = new Repository<ClientResourceAccessClaim>(_dbContext));

        #endregion Public Vars

        #region Methods
         
        public override void Dispose()
        {
            _client = null;
            _clientKey = null;
            _clientResourceAccess = null;
            _clientResourceAccessClaim = null;
        }

        #endregion Methods

    }
}
