using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class ManagerUpdateRequest
    {
        /// <summary>
        /// Manager Identity
        /// </summary>
        [Required]
        public string Identity { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Settings
        /// </summary>
        [Required]
        public ManagerSettingsUpdateRequest Settings { get; set; }

    }
}