using Newtonsoft.Json;
using Service.MemberMgr.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Request
{
    public class MemberUpdateRequest 
    {

        #region Properties

        [Required]
        public string ResourceKey { get; set; }

        [Required]
        public string ProviderKey { get; set; }
         
        /// <summary>
        /// Email Address
        /// </summary> 
        public string Email { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public object Metadata { get; set; }

        #endregion Properties

        #region Methods

        internal virtual MemberVm ToMemberVm(MemberVm view)
        {  
            view.Email = Email;
            view.DisplayName = DisplayName;
            view.Metadata = Metadata;

            return view;
        }

        #endregion Methods

    }
}