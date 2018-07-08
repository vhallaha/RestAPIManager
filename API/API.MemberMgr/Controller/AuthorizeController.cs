using API.MemberMgr.Model.Request;
using API.MemberMgr.Model.Response;
using System.Web.Http;
using System.Web.Http.Description;
using Utilities.Shared;
using Utilities.Shared.Validation;

namespace API.MemberMgr.Controller
{
    public class AuthorizeController : BaseController
    {

        [HttpPost,
         ClaimAuthorize(MemberClaim.Authorize),
         ResponseType(typeof(MemberLoginResponse))]
        public IHttpActionResult Login(MemberLoginRequest request)
        {

        }

    }
}
