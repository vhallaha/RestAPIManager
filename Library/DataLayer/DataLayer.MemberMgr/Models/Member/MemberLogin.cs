using System;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Member.Enum;

namespace DataLayer.MemberMgr.Models
{
    public class MemberLogin
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public string ProviderKey { get; set; }

        //----------------------------------------
        // Member Login Required Properites
        //----------------------------------------

        /// <summary>
        /// MemberManager Id FK
        /// </summary>
        public int MemberManagerId { get; set; }

        /// <summary>
        /// Member Lookup
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Account Current Status
        /// </summary>
        public MemberStatus Status { get; set; }

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        /// <summary>
        /// Member
        /// </summary>
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        /// <summary>
        /// Application
        /// </summary>
        [ForeignKey("MemberManagerId")]
        public virtual MemberManager Manager { get; set; }

    }
}
