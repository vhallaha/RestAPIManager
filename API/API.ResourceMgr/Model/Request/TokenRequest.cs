using System.ComponentModel.DataAnnotations;

namespace API.ResourceMgr.Model.Request
{
    public class TokenRequest
    {

        #region Properties

        /// <summary>
        /// API Key
        /// </summary>
        [Required]
        public string APIKey { get; set; }

        /// <summary>
        /// API Secret
        /// </summary>
        [Required]
        public string APISecret { get; set; }

        #endregion Properties

    }
}