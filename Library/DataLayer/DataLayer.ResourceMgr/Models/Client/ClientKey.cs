using Library.Core;
using System;
using Utilities.Resource.Enums;

namespace DataLayer.ResourceMgr.Models
{
    public class ClientKey : CoreModel
    {
        //----------------------------------------
        // Client Key Properites
        //----------------------------------------

        public int ClientId { get; set; }

        public string APIKey { get; set; }

        public string APISecret { get; set; }

        public ClientKeyStatus Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        /// <summary>
        /// Client
        /// </summary>
        public virtual Client Client { get; set; }
    }
}
