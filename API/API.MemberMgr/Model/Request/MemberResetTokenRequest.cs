using System.ComponentModel.DataAnnotations;
using Utilities.Member.Enum;

namespace API.MemberMgr.Model.Request
{
    public class MemberResetTokenRequest
    {

        #region Properties

        /// <summary>
        /// Provider Key
        /// </summary>
        [Required]
        public string ProviderKey { get; set; }

        /// <summary>
        /// Reset Token Type
        /// </summary>
        [Required]
        public MemberResetTokenType Type { get; set; }

        #endregion Properties

    }
}