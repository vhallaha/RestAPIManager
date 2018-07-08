using Library.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DatabaseMgr.Models
{
    public class DatabaseLog : CoreModel
    {
        //----------------------------------------
        // Database Properites
        //----------------------------------------

        /// <summary>
        /// Database Lookup
        /// </summary>
        public int DatabaseId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate { get; set; }
         
        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        /// <summary>
        /// Database
        /// </summary>
        [ForeignKey("DatabaseId")]
        public virtual Database Database { get; set; }

    }
}
