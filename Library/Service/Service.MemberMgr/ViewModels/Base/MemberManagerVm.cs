using DataLayer.MemberMgr.Models;
using System;

namespace Service.MemberMgr.ViewModels.Base
{
    public class MemberManagerVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MemberManagerVm()
        {
            // Default Constructor
        }

        internal MemberManagerVm(MemberManager view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _identity = view.Identity;
            _ownerId = view.OwnerId;
            _createDate = view.CreateDate;
            _updateDate = view.UpdateDate;
            _settings = new MemberManagerSettingsVm(view.Settings);

            Name = view.Name;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return _id; }
        }
        private int _id;

        /// <summary>
        /// Identity
        /// </summary>
        public string Identity
        {
            get { return _identity; }
        }
        private string _identity;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Settings
        /// </summary>
        public MemberManagerSettingsVm Settings { get { return _settings; } }
        private MemberManagerSettingsVm _settings;

        /// <summary>
        /// Owner Id
        /// </summary>
        public int OwnerId { get { return _ownerId; } }
        private int _ownerId;
         
        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate { get { return _createDate; } }
        private DateTime _createDate;

        /// <summary>
        /// Update Date
        /// </summary>
        public DateTime? UpdateDate { get { return _updateDate; } }
        private DateTime? _updateDate;

        #endregion Properties

        #region Methods

        internal virtual MemberManager ToEntity(MemberManager view = null)
        {
            view = view ?? (view = new MemberManager());

            view.Name = Name; 

            if (view.CreateDate != new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        internal virtual MemberManager ToEntityCreate(int ownerId)
        {
            var view = ToEntity();
            view.OwnerId = ownerId;

            view.Identity = Guid.NewGuid().ToString("N");
            view.CreateDate = DateTime.UtcNow;

            return view;
        }

        #endregion Methods

    }
}
