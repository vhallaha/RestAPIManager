using Service.MemberMgr.Service;
using System;

namespace Service.MemberMgr
{
    public class MemberUnitOfWork : IDisposable
    {
        #region Private Vars

        private string _dbConn = String.Empty;

        #endregion Private Vars

        #region Ctor
        public MemberUnitOfWork(string dbConn)
        {
            _dbConn = dbConn;
        }
        #endregion Ctor 

        /* MEMBER MANAGER SVC
        ----------------------------------------------------------------------*/
        public MemberManagerSvc MemberManagerSvc => _memberManagerSvc ?? (_memberManagerSvc = new MemberManagerSvc(_dbConn));
        private MemberManagerSvc _memberManagerSvc = null;

        /* MEMBER SVC
        ----------------------------------------------------------------------*/
        public MemberSvc MemberSvc => _memberSvc ?? (_memberSvc = new MemberSvc(_dbConn));
        private MemberSvc _memberSvc = null;

        #region Methods

        public void Dispose()
        {
            if (_memberManagerSvc != null)
                _memberManagerSvc.Dispose();

            if (_memberSvc != null)
                _memberSvc.Dispose();
             
        }

        #endregion Methods
    }
}
