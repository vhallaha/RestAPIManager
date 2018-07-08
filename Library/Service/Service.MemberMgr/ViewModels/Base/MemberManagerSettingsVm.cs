using DataLayer.MemberMgr.Models;
using Utilities.Member.Enum;

namespace Service.MemberMgr.ViewModels.Base
{
    public class MemberManagerSettingsVm
    {

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public MemberManagerSettingsVm()
        {
            // Default Constructor
        }

        internal MemberManagerSettingsVm(MemberManagerSettings view = null)
        {
            if (view == null)
                return;

            _id = view.Id;

            AutoValidateUser = view.AutoValidateUser;
            RestrictEmail = view.RestrictEmail;
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
        /// Auto Validate User
        /// </summary>
        public bool AutoValidateUser { get; set; }

        /// <summary>
        /// Restrict Email
        /// </summary>
        public bool RestrictEmail { get; set; }

        /// <summary>
        /// Application current Status
        /// </summary>
        public MemberManagerStatus Status { get; set; }

        #endregion Properties

        #region Methods

        internal MemberManagerSettings ToEntity(MemberManagerSettings view = null)
        {
            view = view ?? (view = new MemberManagerSettings());

            view.AutoValidateUser = AutoValidateUser;
            view.RestrictEmail = RestrictEmail;
            view.Status = Status;

            return view;
        }

        internal MemberManagerSettings ToEntityCreate(MemberManager manager)
        {
            var view = ToEntity();
            view.MemberManager = manager;
            return view;
        }

        #endregion Methods

    }
}
