using Library.Core;
using System;
using System.Collections.Generic;

namespace DataLayer.ResourceMgr.Models
{
    public class Client : CoreModel
    {
        //----------------------------------------
        // Client Required Properites
        //----------------------------------------

        /// <summary>
        /// Owner of the client.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Client Name
        /// </summary>
        public string Name { get; set; }

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
        /// Client Keys
        /// </summary>
        public virtual ICollection<ClientKey> Keys { get; set; }
    }
}
