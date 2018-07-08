using API.ResourceMgr.Model.Request;
using API.ResourceMgr.Model.Response;
using Service.ResourceMgr.ViewModels.Base;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using Utilities;
using Utilities.Resource.Enums;
using Utilities.Shared;

namespace API.ResourceMgr.Controller
{
    public class TokenController : BaseController
    {

        /// <summary>
        /// Generate the token with the data access that the client has
        /// NOTE : Client DataAccess needs to be cross-checked to the database
        ///        once used in the resource 
        /// </summary>
        /// <param name="request">TokenRequest</param>
        /// <returns>TokenResponse</returns> 
        [HttpPost, AllowAnonymous, ResponseType(typeof(TokenRequest))]
        public IHttpActionResult Authorize(TokenRequest request)
        {
            // Validate if the request is valid before even checking the 
            // database for identification and status.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var key = ResourceUnitOfWork.ClientSvc.GetClientKeyByAPIKey(request.APIKey);

            if (key == null)
                return NotFound();

            if (key.APISecret != request.APISecret)
                return Conflict();

            if (key.Status != ClientKeyStatus.Open)
                return Unauthorized();

            var token = CreateToken(key);

            return Ok(token);
        }
         
        #region Methods

        public TokenResponse CreateToken(ClientKeyVm key)
        {

            // Grab all the resource access of the client 
            // we'll be using that to create claims to be passed to our
            // JWT generator.
            var dataAccess = ResourceUnitOfWork.ClientSvc.GetClientResourceAccess(key.ClientId);

            if (dataAccess == null)
                return null;

            JwtHelper tokenizer = new JwtHelper(APIToken);

            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, key.APIKey));

            foreach (var access in dataAccess)
                identity.AddClaim(new Claim(SharedClaimType.DataAccess, access.ResourceId.ToString()));

            var token = tokenizer.GenerateToken(identity);
            return new TokenResponse()
            {
                ExpireDate = token.ExpireDate,
                Token = token.Token
            };
        }

        #endregion Methids


    }
}
