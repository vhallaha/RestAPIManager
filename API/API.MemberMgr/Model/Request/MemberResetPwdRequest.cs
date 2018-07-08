using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class MemberResetPwdRequest
    {
        /// <summary>
        /// Provider Key
        /// </summary>
        [Required]
        public string ProviderKey { get; set; }

        /// <summary>
        /// New Password
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// Reset Token
        /// </summary>
        [Required]
        public string ResetToken { get; set; }

    }
}