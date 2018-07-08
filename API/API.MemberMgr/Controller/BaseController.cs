using Service.MemberMgr;
using Service.ResourceMgr;
using Service.ResourceMgr.ViewModels.Base;
using System.Net;
using Utilities;
using Utilities.Member.Settings;
using Utilities.Shared.ApiClient;
using Utilities.Shared.CtrlHelper;

namespace API.MemberMgr.Controller
{
    public class BaseController : SharedController
    {

        #region Private Vars

        private string _dbConnStr => ConfigurationManager.Get("dbConnStr").ToString();
        private MemberUnitOfWork _memUnitOfWork = null;
        private ResourceUnitOfWork _resUnitOfWork = null;

        private ResourceVm _memResource = null;

        #endregion Private Vars

        #region Properties

        protected MemberUnitOfWork MemberUnitOfWork
        {
            get { return _memUnitOfWork ?? (_memUnitOfWork = new MemberUnitOfWork(_dbConnStr)); }
        }

        protected ResourceUnitOfWork ResourceUnitOfWork
        {
            get { return _resUnitOfWork ?? (_resUnitOfWork = new ResourceUnitOfWork(_dbConnStr)); }
        }

        protected ResourceVm MemberResource
        {
            get { return _memResource ?? (_memResource = ResourceUnitOfWork.ResourceManagerSvc.Get(MemberMgrSettings.MemberManagerResourceId)); }
        }

        #endregion Properties

        #region Helper
         
        public dynamic ReturnResponse(HttpStatusCode code, string message)
        { 
            return new ApiErrorReturnExtension(code, message, Request);
        }

        #endregion Helper

    }
}
