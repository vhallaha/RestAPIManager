using Library.Core;
using System;
using System.Collections.ObjectModel;

namespace DataLayer.MemberMgr.Models
{
    public class Member : CoreModel
    {
        //----------------------------------------
        // Member Properites
        //----------------------------------------
         
        public string CryptoKey { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Users Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Member Metadata
        /// </summary> 
        public string Metadata { get; set; }

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------
         
        public virtual MemberOptions Options { get; set; }

        /// <summary>
        /// User Logins
        /// </summary>
        public virtual Collection<MemberLogin> Logins { get; set; }

    }
}
