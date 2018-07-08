using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilities.Resource.Enums;

namespace DataLayer.ResourceMgr.Models
{
    public class ResourceSettings
    {
        //----------------------------------------
        // Resource Settings Required Properites
        //----------------------------------------

        [Key, ForeignKey("Resource")]
        public int Id { get; set; }

        public ResourceStatus Status { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        /// <summary>
        /// Resource
        /// </summary>
        public virtual Resource Resource { get; set; }

    }
}
