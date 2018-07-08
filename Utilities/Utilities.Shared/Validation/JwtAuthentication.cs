using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;


namespace Utilities.Shared.Validation
{
    public class JwtAuthentication : Attribute, IAuthenticationFilter
    {

        public string Realm { get; set; }
        public bool AllowMultiple => false;

        private string userName = String.Empty;
        protected JwtHelper jwtCore = null;

        #region Ctor

        public JwtAuthentication()
        { 
        }

        public JwtAuthentication(string systemKey)
        {
            jwtCore = new JwtHelper(systemKey);
        }

        #endregion Ctor

        #region Base Methods

        /// <summary>
        /// Authenticate if the token has member in it
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            IEnumerable<string> keyTest;
            if (!request.Headers.TryGetValues("key", out keyTest))
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwt(token, context);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
                return;
            }

            context.Principal = principal;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        #endregion Base Methods

        #region Methods

        private bool ValidateToken(string token, HttpAuthenticationContext context, out string username)
        {
            username = String.Empty;

            var simplePrinciple = jwtCore.GetPrincipal(token);
            if (simplePrinciple == null) 
                return false;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", context.Request);
                return false;
            }

            if (!identity.IsAuthenticated)
            {
                context.ErrorResult = new AuthenticationFailureResult("UnAuthorized.", context.Request);
                return false;
            }

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (String.IsNullOrWhiteSpace(username))
                return false;

            // Database Check
            if (!DatabaseCheck(username, context))
                return false;

            return true;
        }

        /// <summary>
        /// Base Authentication using Jwt
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected Task<IPrincipal> AuthenticateJwt(string token, HttpAuthenticationContext context)
        {
            if (ValidateToken(token, context, out userName))
            {
                var claims = ConstructClaims(userName, context);
                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }

        #endregion Methods

        #region Overridables
         
        /// <summary>
        /// Override this method if you want to add more claims to the
        /// list of claims that will identify the user.
        /// </summary>
        /// <returns></returns>
        protected virtual List<Claim> ConstructClaims(string userName, HttpAuthenticationContext context)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, userName)
                };

            return claims;
        }

        /// <summary>
        /// Required method to override, this will check for database validation.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        protected virtual bool DatabaseCheck(string username, HttpAuthenticationContext context)
        {
            throw new NotImplementedException();
        }
         
        #endregion Overridables

    }
}
