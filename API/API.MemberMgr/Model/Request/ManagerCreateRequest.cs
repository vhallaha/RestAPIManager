using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class ManagerCreateRequest
    { 
        /// <summary>
        /// Manager Name
        /// </summary>
        [Required]
        public string Name { get; set; }
         
        /// <summary>
        /// Settings
        /// </summary>
        [Required]
        public ManagerSettingsCreateRequest Settings { get; set; }
        
    }
}