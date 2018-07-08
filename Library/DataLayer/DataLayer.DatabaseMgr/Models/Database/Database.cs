using Library.Core;
using System;
using Utilities.Shared;

namespace DataLayer.DatabaseMgr.Models
{
    public class Database : CoreModel
    {
        //----------------------------------------
        // Database Properites
        //----------------------------------------

        /// <summary>
        /// Friendly Name
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Database Identity
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Database Type
        /// </summary>
        public DatabaseType Type { get; set; }

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
