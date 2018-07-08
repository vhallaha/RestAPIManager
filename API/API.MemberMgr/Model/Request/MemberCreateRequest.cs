using Newtonsoft.Json;
using Service.MemberMgr.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class MemberCreateRequest
    {

        #region Properties

        /// <summary>
        /// Username of the account
        /// </summary>
        [Required] 
        public string Username { get; set; }

        /// <summary>
        /// Password of the account
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public dynamic Metadata { get; set; }

        #endregion Properties

        #region Methods

        internal virtual MemberVm ToMemberVm()
        {
            var view = new MemberVm();
            view.Username = Username;
            view.Password = Password;
            view.Email = Email;
            view.DisplayName = DisplayName;
            view.Metadata = Metadata;

            return view;
        }

        #endregion Methods

    }
}