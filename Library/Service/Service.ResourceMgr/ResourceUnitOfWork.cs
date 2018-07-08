using Service.ResourceMgr.Service;
using System;

namespace Service.ResourceMgr
{
    public class ResourceUnitOfWork : IDisposable
    {

        #region Private Vars

        private string _dbConn = String.Empty;

        #endregion Private Vars

        #region Ctor
        public ResourceUnitOfWork(string dbConn)
        {
            _dbConn = dbConn;
        }
        #endregion Ctor 

        /* RESOURCE MANAGER SVC
        ----------------------------------------------------------------------*/
        public ResourceManagerSvc ResourceManagerSvc => _resourceManagerSvc ?? (_resourceManagerSvc = new ResourceManagerSvc(_dbConn));
        private ResourceManagerSvc _resourceManagerSvc = null;

        /* CLIENT SVC
        ----------------------------------------------------------------------*/
        public ClientSvc ClientSvc => _clientSvc ?? (_clientSvc = new ClientSvc(_dbConn));
        private ClientSvc _clientSvc = null;
         
        #region Methods

        public void Dispose()
        {
            if (_clientSvc != null)
                _clientSvc.Dispose();

            if (_resourceManagerSvc != null)
                _resourceManagerSvc.Dispose(); 
        }

        #endregion Methods
    }
}
