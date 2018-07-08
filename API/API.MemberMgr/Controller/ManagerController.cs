using API.MemberMgr.Model.Request;
using API.MemberMgr.Model.Response;
using Service.MemberMgr.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Utilities.Member.Enum;
using Utilities.Member.Settings;
using Utilities.Shared;
using Utilities.Shared.Validation;

namespace API.MemberMgr.Controller
{
    [RoutePrefix("Member/{loginProviderKey}/Manager")]
    public class ManagerController : BaseController
    {

        #region Get

        /// <summary>
        /// Grabs the list of managers that the user owns.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <returns><![CDATA[ IEnumerable<ManagerResponse> ]]></returns>
        [HttpGet, 
         ClaimAuthorize(MemberClaim.Read),
         Route("List"),
         ResponseType(typeof(IEnumerable<ManagerResponse>))]
        public IHttpActionResult List(string loginProviderKey)
        {
            var member = GetMember(loginProviderKey);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            var managers = MemberUnitOfWork.MemberManagerSvc.FindByUserId(member.Id);
            if (managers == null || !managers.Any())
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            return Ok(from m in managers.ToList() select new ManagerResponse(m));
        }

        /// <summary>
        /// Grabs a specific manager that the user owns.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="identity">Manager Identity</param>
        /// <returns></returns>
        [HttpGet,
         Route("GetByIdentity/{identity}"),
         ClaimAuthorize(MemberClaim.Read),
         ResponseType(typeof(ManagerResponse))]
        /// <returns>ManagerResponse</returns>
        public IHttpActionResult GetByIdentity(string loginProviderKey, string identity)
        {
            var member = GetMember(loginProviderKey);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            var manager = MemberUnitOfWork.MemberManagerSvc.Get(member.Id, identity);
            if (manager == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            return Ok(new ManagerResponse(manager));
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Creates a new manager under that user.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="manager">Manager Create Request</param>
        /// <returns></returns>
        [HttpPost,
         Route("CreateManager"),
         ClaimAuthorize(MemberClaim.Create),
         ResponseType(typeof(ManagerResponse))]
        public IHttpActionResult CreateManager(string loginProviderKey, [FromBody] ManagerCreateRequest manager)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = GetMember(loginProviderKey);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            try
            {
                var resp = MemberUnitOfWork.MemberManagerSvc.Create(member.Id, new MemberManagerVm()
                {
                    Name = manager.Name
                }, new MemberManagerSettingsVm()
                {
                    AutoValidateUser = manager.Settings.AutoValidateUser,
                    RestrictEmail = manager.Settings.RestrictEmail,
                    Status = MemberManagerStatus.Pending
                });


                if (resp.IsSuccess)
                    return Ok(new ManagerResponse(resp.Manager));
                else
                    return BadRequest(resp.Message);
            }
            catch
            {
                // Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }

        }

        /// <summary>
        /// Updates the specified manager
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="request">ManagerUpdateRequest</param>
        /// <returns>ManagerResponse</returns>
        [HttpPut,
         Route("UpdateManager"),
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(ManagerResponse))]
        public IHttpActionResult UpdateManager(string loginProviderKey, [FromBody] ManagerUpdateRequest request)
        {
            var member = GetMember(loginProviderKey);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            var manager = MemberUnitOfWork.MemberManagerSvc.Get(member.Id, request.Identity);
            if (manager == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            try
            {
                manager.Name = request.Name;
                var appResp = MemberUnitOfWork.MemberManagerSvc.UpdateApplication(manager);

                manager.Settings.AutoValidateUser = request.Settings.AutoValidateUser;
                manager.Settings.RestrictEmail = request.Settings.RestrictEmail;
                manager.Settings.Status = request.Settings.Status ?? manager.Settings.Status;
                var settingsResp = MemberUnitOfWork.MemberManagerSvc.UpdateSettings(manager.Settings);

                return Ok(new ManagerResponse(manager));
            }
            catch
            {
                // Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }

        }

        /// <summary>
        /// Grabs a specific manager that the user owns.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="identity">Manager Identity</param>
        /// <returns></returns>
        [HttpGet,
         Route("DeleteManager/{identity}"),
         ClaimAuthorize(MemberClaim.Delete),
         ResponseType(typeof(string))]
        public IHttpActionResult DeleteManager(string loginProviderKey, string identity)
        {
            var member = GetMember(loginProviderKey);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            var manager = MemberUnitOfWork.MemberManagerSvc.Get(member.Id, identity);
            if (manager == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            try
            {
                var resp = MemberUnitOfWork.MemberManagerSvc.Delete(manager.Id);

                if (resp.IsSuccess)
                    return Ok(resp.Message);
                else
                    return Conflict(resp.Message);
            }
            catch
            {
                // Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }
        }

        #endregion Set

        #region Helper

        public MemberVm GetMember(string providerKey)
        {
            var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKey(providerKey);
            return member;
        }

        #endregion Helper
    }
}