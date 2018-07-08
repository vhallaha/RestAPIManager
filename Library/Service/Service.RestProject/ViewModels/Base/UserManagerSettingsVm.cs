using Utilities.Member.Enum;

namespace Service.RestProject.ViewModels.Base
{
    public class UserManagerSettingsVm
    {

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public UserManagerSettingsVm()
        {
            // Default Constructor
        }
         
        #endregion Ctor

        #region Properties

        /// <summary>
        /// Identity
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Auto Validate User
        /// </summary>
        public bool AutoValidateUser { get; set; }

        /// <summary>
        /// Restrict Email Address
        /// </summary>
        public bool RestrictEmail { get; set; }

        /// <summary>
        /// Application current Status
        /// </summary>
        public MemberManagerStatus Status { get; set; }

        #endregion Properties

    }
}
