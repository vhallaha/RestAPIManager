using Newtonsoft.Json;

namespace Service.RestProject.ViewModels.Base
{
    public class UserVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserVm()
        {

        }

        internal UserVm(dynamic m)
        {
            ProviderKey = m.ProviderKey;
            Username = m.Username;
            Email = m.Email;
            DisplayName = m.DisplayName; 
            
            if (!string.IsNullOrWhiteSpace(m.Metabase))
                Metadata = JsonConvert.DeserializeObject<UserMetadataVm>(m.Metabase);
            else
                Metadata = new UserMetadataVm();
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Provider Key
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Additional Information
        /// </summary>
        public UserMetadataVm Metadata { get; set; }
         
        #endregion Properties

    }
}
