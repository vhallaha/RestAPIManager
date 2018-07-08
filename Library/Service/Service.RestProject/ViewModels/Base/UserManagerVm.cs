using System;
using System.ComponentModel.DataAnnotations;

namespace Service.RestProject.ViewModels.Base
{
    public class UserManagerVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserManagerVm()
        {

        }
         
        #endregion Ctor

        #region Properties

        /// <summary>
        /// Manager Identity
        /// </summary> 
        public string Identity { get; set; }

        /// <summary>
        /// Name
        /// </summary> 
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Manager current Settings
        /// </summary>  
        public UserManagerSettingsVm Settings { get; set; }

        /// <summary>
        /// Create Date
        /// </summary> 
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        #endregion Properties

    }
}
