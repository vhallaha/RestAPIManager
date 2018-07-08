using Service.ResourceMgr;
using System.Web.Http;
using Utilities;

namespace API.ResourceMgr.Controller
{
    public class BaseController : ApiController
    {

        #region Private Vars

        private string _dbConnStr => ConfigurationManager.Get("dbConnStr").ToString();
        private ResourceUnitOfWork _resUnitOfWork = null;

        #endregion Private Vars

        #region Protected Vars

        protected string APIToken => ConfigurationManager.Get("systemToken").ToString();

        #endregion Protected Vars

        #region Properties

        /// <summary>
        /// Resource Unit Of Work
        /// </summary>
        protected ResourceUnitOfWork ResourceUnitOfWork
        {
            get { return _resUnitOfWork ?? (_resUnitOfWork = new ResourceUnitOfWork(_dbConnStr)); }
        }

        #endregion Properties

    }
}
