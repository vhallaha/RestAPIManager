using Service.MemberMgr.ViewModels.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.MemberMgr.Model.Response
{
    public class ManagerResponse
    {

        #region Ctor

        internal ManagerResponse()
        {
            // Default Empty Constructor
        }

        internal ManagerResponse(MemberManagerVm m)
        {
            Identity = m.Identity;
            Name = m.Name;
            CreateDate = m.CreateDate;
            UpdateDate = m.UpdateDate;
            Settings = new ManagerSettingsResponse(m.Identity, m.Settings);
        }

        #endregion Ctor

        #region Properties

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
        /// Manager current Settings
        /// </summary>
        [Required]
        public ManagerSettingsResponse Settings { get; set; }

        /// <summary>
        /// Create Date
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Update Date
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        #endregion Properties

    }
}