using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class MemberChangePasswordRequest
    {

        /// <summary>
        /// Provider Key
        /// </summary>
        [Required]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Current Accounts Password
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// New Password
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm Password
        /// </summary>
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password did not match.")]
        public string ConfirmPassword { get; set; }

    }
}