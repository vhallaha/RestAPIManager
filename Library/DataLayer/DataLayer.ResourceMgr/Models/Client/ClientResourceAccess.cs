using Library.Core;
using System;
using Utilities.Resource.Enums;

namespace DataLayer.ResourceMgr.Models
{
    public class ClientResourceAccess : CoreModel
    {
        //----------------------------------------
        // Client Resource Access Properites
        //----------------------------------------

        /// <summary>
        /// Resource Key
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Client Id
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Resource Id
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Resource Value
        /// </summary>
        public int ResourceValue { get; set; }

        /// <summary>
        /// Current Status
        /// </summary>
        public ClientResourceAccessStatus Status { get; set; }

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
        /// Client
        /// </summary>
        public virtual Client Client { get; set; }

        /// <summary>
        /// Resource
        /// </summary>
        public virtual Resource Resource { get; set; }

    }
}
