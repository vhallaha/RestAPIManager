using Library.Core;
using System;
using System.Collections.Generic;
using Utilities.Shared;

namespace DataLayer.ResourceMgr.Models
{
    public class Resource : CoreModel
    {
        //----------------------------------------
        // Resource Properites
        //----------------------------------------

        /// <summary>
        /// Resource Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Resource Type
        /// </summary>
        public ResourceType Type { get; set; }

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
        /// Resource Settings
        /// </summary>
        public virtual ResourceSettings Settings { get; set; }
        
        /// <summary>
        /// Resource Claims
        /// </summary>
        public virtual ICollection<ResourceClaim> Claims { get; set; }

    }
}
