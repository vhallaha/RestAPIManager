using System.ComponentModel.DataAnnotations;

namespace Service.RestProject.ViewModels.Forms
{
    public class SignupVm
    {

        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required] 
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Confirm Password did not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public bool AcceptTerms { get; set; }
    }
}
