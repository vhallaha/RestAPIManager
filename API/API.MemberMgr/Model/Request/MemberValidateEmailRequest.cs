using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class MemberValidateEmailRequest
    {
        /// <summary>
        /// Provider Key
        /// </summary>
        [Required]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [Required]
        public string Token { get; set; }

    }
}