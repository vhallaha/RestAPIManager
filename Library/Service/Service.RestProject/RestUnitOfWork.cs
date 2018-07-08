using Service.RestProject.Service;
using System;

namespace Service.RestProject
{
    public class RestUnitOfWork : IDisposable
    {

        #region Private Vars

        private string _dbConn = String.Empty;

        #endregion Private Vars

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConn">Database Connection String</param>
        public RestUnitOfWork(string dbConn)
        {
            _dbConn = dbConn;
        }

        #endregion Ctor

        /* USER SVC
        ----------------------------------------------------------------------*/
        public UserSvc UserSvc => _userSvc ?? (_userSvc = new UserSvc(_dbConn));
        private UserSvc _userSvc = null;

        /* USER MANAGER SVC
        ----------------------------------------------------------------------*/
        public UserMgrSvc UserMgrSvc => _userMgrSvc ?? (_userMgrSvc = new UserMgrSvc(_dbConn));
        private UserMgrSvc _userMgrSvc = null;

        #region Methods

        public void Dispose()
        {
            if (_userSvc != null)
                _userSvc.Dispose();
        }

        #endregion Methods
    }
}
