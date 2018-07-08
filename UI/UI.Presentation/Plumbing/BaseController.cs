using Service.RestProject;
using System.Web.Mvc;
using System.Web.Security;

namespace UI.Presentation.Plumbing
{
    public class BaseController : Controller
    {

        #region Helpers

        /// <summary>
        /// Rest Unit of work
        /// </summary>
        internal RestUnitOfWork RestUnitOfWork => _restUnitOfWork ?? (_restUnitOfWork = new RestUnitOfWork(RPSettings.DbConnString));
        private RestUnitOfWork _restUnitOfWork = null;

        public bool IsAuthenticated
        {
            get {
                _isAuthenticated = User?.Identity?.IsAuthenticated;
                return _isAuthenticated ?? false;
            }
        }
        private bool? _isAuthenticated;

        public string CurrentUser
        {
            get {
                _currentUser = User?.Identity?.Name; 
                return _currentUser;
            }
        }
        private string _currentUser;

        public string CurrentUserProviderToken
        {
            get {
                var currIdentity = (FormsIdentity)User.Identity;
                _currentUserProviderToken = currIdentity?.Ticket?.UserData;
                return _currentUserProviderToken;
            }
        }
        private string _currentUserProviderToken;

        #endregion Helpers

    }
}