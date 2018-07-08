using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Utilities.Shared.Validation
{
    /// <summary>
    /// This class uses JWT Authentication as its main principal generator.
    /// </summary>
    public class ClaimAuthorize : AuthorizeAttribute
    {

        #region Private Vars

        private string _calimType = String.Empty;

        #endregion Private Vars

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="claimType"></param>
        public ClaimAuthorize(string claimType)
        {
            _calimType = claimType;
        }

        #endregion Ctor

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            bool isAuthorized = false;
            var claims = ClaimsPrincipal.Current.Claims;

            if (!String.IsNullOrWhiteSpace(_calimType))
            {
                if (claims.Any())
                {
                    var curClaim = claims.FirstOrDefault(f => f.Type == _calimType);
                    if (curClaim != null)
                        isAuthorized = true;

                }
            }
            else
                isAuthorized = true;

            if (isAuthorized)
                base.OnAuthorization(actionContext);
            else
                base.HandleUnauthorizedRequest(actionContext);
        }

    }
}
