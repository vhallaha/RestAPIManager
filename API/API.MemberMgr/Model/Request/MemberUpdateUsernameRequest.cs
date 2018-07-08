using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class MemberUpdateUsernameRequest
    {
        /// <summary>
        /// Provider Key
        /// </summary>
        [Required]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [Required]
        public string Username { get; set; }

    }
}