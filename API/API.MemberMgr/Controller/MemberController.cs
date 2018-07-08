using API.MemberMgr.Model.Request;
using API.MemberMgr.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Utilities.Member.Settings;
using Utilities.Shared;
using Utilities.Shared.Validation;

namespace API.MemberMgr.Controller
{

    public class MemberController : BaseController
    {

        #region Get

        /// <summary>
        /// Return a list of members that the Manager has access to.
        /// </summary>
        /// <returns>List of Member Response</returns>
        [HttpGet,
         ClaimAuthorize(MemberClaim.Read),
         ResponseType(typeof(IEnumerable<MemberResponse>))]
        public IHttpActionResult List()
        {
            var members = MemberUnitOfWork.MemberSvc.FindByMemberManagerId(DataAccessValue, DataAccessValue);
            if (members == null || !members.Any())
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return Ok(from m in members.ToList() select new MemberResponse(m));
        }

        /// <summary>
        /// Return a specific member information.
        /// </summary>
        /// <param name="loginProviderKey"></param>
        /// <returns>Member Response</returns>
        [HttpGet,
         Route("Member/GetByProviderKey/{loginProviderKey}"),
         ClaimAuthorize(MemberClaim.Read),
         ResponseType(typeof(MemberResponse))]
        public IHttpActionResult GetByProviderKey(string loginProviderKey)
        {
            var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKey(loginProviderKey);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return Ok(new MemberResponse(member));
        }

        /// <summary>
        /// Return a specific member information based on their login credentials
        /// </summary>
        /// <param name="request">Member Login Request</param>
        /// <returns>MemberResponse</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Read),
         ResponseType(typeof(MemberResponse))]
        public IHttpActionResult GetByLoginCredentials([FromBody] MemberLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = MemberUnitOfWork.MemberSvc.FindMember(request.Username, request.Password, DataAccessValue);
            if (member == null)
                return ReturnResponse(HttpStatusCode.NotFound,
                                      MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return Ok(new MemberResponse(member));
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Checks and create a member if it passes the restriction that the user sets
        /// in their manager.
        /// </summary>
        /// <param name="request">Member Create Request</param>
        /// <returns>MemberCreateResponse</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Create),
         ResponseType(typeof(MemberCreateResponse))]
        public IHttpActionResult CreateMember([FromBody] MemberCreateRequest request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = request.ToMemberVm();

            try
            {
                var response = MemberUnitOfWork.MemberSvc.CreateWithApplication(DataAccessValue, member);

                if (response.IsSuccess)
                    return Ok(new MemberCreateResponse(response.Member, response.Login));
                else
                    return BadRequest(response.Message);
            }
            catch
            {
                //Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }

        }

        /// <summary>
        /// Update the current member with the new information passed by the client.
        /// </summary>
        /// <param name="request">Member Update Request</param>
        /// <returns>MemberResponse</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(MemberResponse))]
        public IHttpActionResult UpdateMember([FromBody] MemberUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKeyAndManagerId(DataAccessValue, request.ProviderKey);
                if (member == null)
                    return ReturnResponse(HttpStatusCode.NotFound,
                                          MemberManagerMessages.Error.MEMBER_NOT_FOUND);

                member = request.ToMemberVm(member);

                var resp = MemberUnitOfWork.MemberSvc.UpdateMember(DataAccessId, member);

                if (resp.IsSuccess)
                    return Ok(new MemberResponse(resp.member));
                else
                    return Conflict(resp.Message);
            }
            catch
            {
                // Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }

        }

        /// <summary>
        /// Reset existing users password
        /// </summary>
        /// <param name="request">Member Reset Pwd Request</param>
        /// <returns>string</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(string))]
        public IHttpActionResult ResetPassword([FromBody] MemberResetPwdRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKeyAndManagerId(DataAccessValue, request.ProviderKey);
                if (member == null)
                    return ReturnResponse(HttpStatusCode.NotFound,
                                          MemberManagerMessages.Error.MEMBER_NOT_FOUND);

                var resp = MemberUnitOfWork.MemberSvc.ResetPassword(request.ProviderKey, request.NewPassword, request.ResetToken);

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

        /// <summary>
        /// Update the user's current username to a new one.
        /// </summary>
        /// <param name="request">Member Update Username Request</param>
        /// <returns>string</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(string))]
        public IHttpActionResult ChangeUserName([FromBody] MemberUpdateUsernameRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKeyAndManagerId(DataAccessValue, request.ProviderKey);
                if (member == null)
                    return ReturnResponse(HttpStatusCode.NotFound,
                                          MemberManagerMessages.Error.MEMBER_NOT_FOUND);

                var resp = MemberUnitOfWork.MemberSvc.ChangeUserName(request.ProviderKey, DataAccessValue, request.Username);

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

        /// <summary>
        /// Validate existing email address
        /// </summary>
        /// <param name="request">Member Validate Email Request</param>
        /// <returns>string</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(string))]
        public IHttpActionResult ValidateEmail([FromBody] MemberValidateEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKeyAndManagerId(DataAccessValue, request.ProviderKey);
                if (member == null)
                    return ReturnResponse(HttpStatusCode.NotFound,
                                          MemberManagerMessages.Error.MEMBER_NOT_FOUND);

                var resp = MemberUnitOfWork.MemberSvc.ValidateEmail(request.ProviderKey, request.Token);

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

        /// <summary>
        /// Change existing user's password
        /// </summary>
        /// <param name="request">Member Change Password Request</param>
        /// <returns>string</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(string))]
        public IHttpActionResult ChangePassword([FromBody] MemberChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKeyAndManagerId(DataAccessValue, request.ProviderKey);
                if (member == null)
                    return ReturnResponse(HttpStatusCode.NotFound,
                                          MemberManagerMessages.Error.MEMBER_NOT_FOUND);

                var resp = MemberUnitOfWork.MemberSvc.ChangePassword(request.ProviderKey, request.OldPassword, request.NewPassword, request.ConfirmPassword);

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

        /// <summary>
        /// Delete the current a member out of your manager
        /// </summary>
        /// <param name="loginProviderKey"></param>
        /// <returns>string</returns>
        [HttpDelete,
         Route("Member/DeleteMember/{loginProviderKey}"),
         ClaimAuthorize(MemberClaim.Delete),
         ResponseType(typeof(string))]
        public IHttpActionResult DeleteMember(string loginProviderKey)
        {
            if (String.IsNullOrWhiteSpace(loginProviderKey))
                return BadRequest();

            try
            {
                var response = MemberUnitOfWork.MemberSvc.DeleteLogin(loginProviderKey);
                if (response.IsSuccess)
                    return Ok(response.Message);
                else
                    return Conflict(response.Message);
            }
            catch
            {
                //Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }
        }

        #endregion Set

        #region Helper

        /// <summary>
        /// Generates a new Reset Token
        /// </summary>
        /// <param name="MemberResetTokenRequest">Member Reset Token Request</param>
        /// <returns>string</returns>
        [HttpPost,
         ClaimAuthorize(MemberClaim.Write),
         ResponseType(typeof(string))]
        public IHttpActionResult RequestResetToken([FromBody] MemberResetTokenRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.ProviderKey))
                return BadRequest("Provider Key missing.");

            try
            {
                var member = MemberUnitOfWork.MemberSvc.GetByLoginProviderKeyAndManagerId(DataAccessValue, request.ProviderKey);
                if (member == null)
                    return NotFound();

                var resp = MemberUnitOfWork.MemberSvc.CreateResetToken(DataAccessValue, request.Type);

                if (resp.IsSuccess)
                    return Ok(resp.Token);
                else
                    return Conflict(resp.Token);
            }
            catch
            {
                // Log to Elmah
                return InternalServerError(new Exception("Opps.. Seems like there is some issue with the request. please contact the support team."));
            }
        }

        #endregion Helper
    }
}