using Service.MemberMgr.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Member.Enum;

namespace API.MemberMgr.Model.Response
{
    public class ManagerSettingsResponse
    {

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        internal ManagerSettingsResponse()
        {
            // Default Constructor
        }

        internal ManagerSettingsResponse(string identity,MemberManagerSettingsVm m)
        {
            Identity = identity;
            AutoValidateUser = m.AutoValidateUser;
            RestrictEmail = m.RestrictEmail;
            Status = m.Status;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Identity
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Auto Validate User
        /// </summary>
        public bool AutoValidateUser { get; set; }

        /// <summary>
        /// Restrict Email Address
        /// </summary>
        public bool RestrictEmail { get; set; }

        /// <summary>
        /// Application current Status
        /// </summary>
        public MemberManagerStatus Status { get; set; }

        #endregion Properties

    }
}