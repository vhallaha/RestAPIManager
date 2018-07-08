using Utilities.Member.Enum;

namespace API.MemberMgr.Model.Request
{
    public class ManagerSettingsUpdateRequest
    {
        /// <summary>
        /// Auto Validate User
        /// </summary>
        public bool AutoValidateUser { get; set; }

        /// <summary>
        /// Restrict Email
        /// </summary>
        public bool RestrictEmail { get; set; }

        /// <summary>
        /// Application current Status
        /// </summary>
        public MemberManagerStatus? Status { get; set; }
    }
}