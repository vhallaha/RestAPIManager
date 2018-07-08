using Service.MemberMgr.ViewModels.Base;

namespace API.MemberMgr.Model.Response
{
    public class MemberResponse
    {

        #region Ctor

        internal MemberResponse()
        {
            // Default Empty Constructor
        }

        internal MemberResponse(MemberVm m)
        { 
            DisplayName = m.DisplayName;
            Email = m.Email;
            Username = m.Username;
            ProviderKey = m.ProviderKey;
            Metadata = m.Metadata;
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
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public dynamic Metadata { get; set; }

        #endregion Properties

    }
}