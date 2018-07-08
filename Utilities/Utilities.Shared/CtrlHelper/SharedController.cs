using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Utilities.Shared.CtrlHelper
{
    public class SharedController : ApiController
    {

        #region Private Property

        private IEnumerable<Claim> PrincipalClaims
        {
            get
            {
                return ClaimsPrincipal.Current?.Claims;
            }
        }

        #endregion Private Property

        #region Protected Property

        /// <summary>
        /// Data Access Value
        /// Note : Use to target specific resource in ResProject
        /// </summary>
        protected int DataAccessValue
        {
            get
            {
                int.TryParse(PrincipalClaims?.FirstOrDefault(f => f.Type == SharedClaimType.DataAccessValue).Value, out _dataAccessValue);
                return _dataAccessValue;
            }
        }
        private int _dataAccessValue;

        /// <summary>
        /// Data Access Key passed by the user to the api
        /// </summary>
        protected string DataAccessKey
        {
            get
            {
                _dataAccessKey = PrincipalClaims?.FirstOrDefault(f => f.Type == SharedClaimType.DataAccessKey).Value;
                return _dataAccessKey;
            }
        }
        private string _dataAccessKey;

        /// <summary>
        /// Data Access Id
        /// </summary>
        protected int DataAccessId
        {
            get
            {
                int.TryParse(PrincipalClaims?.FirstOrDefault(f => f.Type == SharedClaimType.DataAccessId).Value, out _dataAccessId);
                return _dataAccessId;
            }
        }
        private int _dataAccessId;


        #endregion Protected Property

        #region Override

        /// <summary>
        /// returns conflic content
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IHttpActionResult Conflict(string message)
        {
            return Content(HttpStatusCode.Conflict, message);
        }
        #endregion Override
    }
}
