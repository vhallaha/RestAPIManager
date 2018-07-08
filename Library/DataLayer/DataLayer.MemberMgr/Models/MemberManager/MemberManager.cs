using Library.Core;
using System;
using Utilities.Member.Enum;

namespace DataLayer.MemberMgr.Models
{
    public class MemberManager : CoreModel
    {
        //----------------------------------------
        // Member Manager Properites
        //----------------------------------------

        /// <summary>
        /// Member Manager Identity.
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Member Manager Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Owner Id
        /// </summary>
        public int OwnerId { get; set; }
          
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

        public virtual MemberManagerSettings Settings { get; set; }

    }
}
