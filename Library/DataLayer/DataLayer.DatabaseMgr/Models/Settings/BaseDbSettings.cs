using Library.Core;
using Utilities.Shared.Enum;

namespace DataLayer.DatabaseMgr.Models.Settings
{
    public class BaseDbSettings : CoreModel
    {
        //----------------------------------------
        // Database Properites
        //----------------------------------------
         
        public int DatabaseId { get; set; }
         
        public DatabaseStatus Status { get; set; }
		
    }
}
