using System.ComponentModel.DataAnnotations;

namespace Service.RestProject.ViewModels.Forms
{
    public class ResetPasswordVm
    {
        /// <summary>
        /// New Password
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confrim Password
        /// </summary>
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm password did not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetToken { get; set; }

    }
}
