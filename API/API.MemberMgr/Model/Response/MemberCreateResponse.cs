using Service.MemberMgr.ViewModels.Base;
using System;
using Utilities.Member.Enum;

namespace API.MemberMgr.Model.Response
{
    public class MemberCreateResponse
    {

        #region Ctor

        public MemberCreateResponse(MemberVm member, MemberLoginVm login)
        {
            ProviderKey = login.ProviderKey;
            Status = login.Status;

            Username = member.Username;
            Email = member.Email;
            DisplayName = member.DisplayName;
            Metadata = member.Metadata;

            CreateDate = member.CreateDate;
        }

        #endregion Ctor
        
        #region Properties

        /// <summary>
        /// Member Login Provider Key
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Member Status
        /// </summary>
        public MemberStatus Status { get; set; }

        /// <summary>
        /// Username of the account
        /// </summary> 
        public string Username { get; set; }

        /// <summary>
        /// Email Address
        /// </summary> 
        public string Email { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public dynamic Metadata { get; set; }

        /// <summary>
        /// Create Date (UTC)
        /// </summary>
        public DateTime CreateDate { get; set; }

        #endregion Properties

    }

}