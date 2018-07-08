using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DatabaseMgr.Models.Settings
{
    public class FirebaseSettings
    {
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
