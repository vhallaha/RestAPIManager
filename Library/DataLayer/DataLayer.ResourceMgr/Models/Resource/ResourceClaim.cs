using Library.Core;
using System;

namespace DataLayer.ResourceMgr.Models
{
    public class ResourceClaim : CoreModel
    {
        //----------------------------------------
        // Resource Claims Properites
        //----------------------------------------

        /// <summary>
        /// Resource Id
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Claim Name
        /// </summary>
        public string ClaimName { get; set; }

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

        /// <summary>
        /// Resource
        /// </summary>
        public virtual Resource Resource { get; set; }
    }
}
