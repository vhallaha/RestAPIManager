using System;
using System.ComponentModel.DataAnnotations;

namespace API.ResourceMgr.Model.Response
{
    public class TokenResponse
    {
         
        #region Properties

        /// <summary>
        /// Json Web Token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Token Expire Date
        /// </summary>
        [Required]
        public DateTime ExpireDate { get; set; }

        #endregion Properties

    }
}