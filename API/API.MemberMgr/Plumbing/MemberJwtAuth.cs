using Service.ResourceMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Filters;
using Utilities;
using Utilities.Resource.Enums;
using Utilities.Shared;
using Utilities.Shared.Validation;

namespace API.MemberMgr.Plumbing
{
    internal class MemberJwtAuth : JwtAuthentication
    {

        #region Private Vars

        private string APIToken => ConfigurationManager.Get("systemToken").ToString();
        private string _dbConnStr => ConfigurationManager.Get("dbConnStr").ToString();
        private ResourceUnitOfWork _resUnitOfWork = null;

        #endregion Private Vars

        #region Ctor

        internal MemberJwtAuth()
        {
            jwtCore = new JwtHelper(APIToken);
        }

        #endregion Ctor

        #region Overrides

        protected override List<Claim> ConstructClaims(string userName, HttpAuthenticationContext context)
        {
            var request = context.ActionContext.Request;
            var claims = base.ConstructClaims(userName, context);
            var clientResourceKey = request.Headers.GetValues("key").First().ToString();

            if (CacheManager.Exists(clientResourceKey + "_claim"))
            {
                claims = (List<Claim>)CacheManager.Get(clientResourceKey + "_claim");
            }
            else
            {
                _resUnitOfWork = new ResourceUnitOfWork(_dbConnStr);
                var clientCheck = _resUnitOfWork.ClientSvc.GetClientKeyByAPIKey(userName);
                var resourceAccess = _resUnitOfWork.ClientSvc.GetResourceAccess(clientResourceKey);
                if (clientCheck != null && resourceAccess != null)
                {
                    var accessClaims = _resUnitOfWork.ClientSvc.GetClientResourceClaims(clientCheck.Id, clientResourceKey);
                    if (accessClaims != null)
                    {
                        foreach (var claim in accessClaims)
                        {
                            if (claim.Access == ClientResourceClaimsAccess.Allow)
                                claims.Add(new Claim(claim.ClaimName, claim.Access.ToString()));
                        }
                    }
                };

                /*-----------------------------------------------------------------------------------
                    Include the Resource Information to be use later to get information on the target
                    resource
                -----------------------------------------------------------------------------------*/
                claims.Add(new Claim(SharedClaimType.DataAccessKey, clientResourceKey));
                claims.Add(new Claim(SharedClaimType.DataAccessId, resourceAccess.Id.ToString()));
                claims.Add(new Claim(SharedClaimType.DataAccessValue, resourceAccess.ResourceValue.ToString()));
                CacheManager.Create(clientResourceKey + "_claim", claims, true, DateTime.UtcNow.AddSeconds(20));
            }


            return claims;
        }

        protected override bool DatabaseCheck(string username, HttpAuthenticationContext context)
        {
            var request = context.ActionContext.Request;
            var clientResourceKey = request.Headers.GetValues("key").First().ToString();

            _resUnitOfWork = new ResourceUnitOfWork(_dbConnStr);
            var clientCheck = _resUnitOfWork.ClientSvc.GetClientKeyByAPIKey(username);
            if (clientCheck == null)
                return false;

            if (clientCheck.Status != ClientKeyStatus.Open)
                return false;

            var resCheck = _resUnitOfWork.ClientSvc.GetResourceAccess(clientResourceKey);

            if (resCheck == null)
                return false;

            return true;
        }

        #endregion Overrides

    }
}
