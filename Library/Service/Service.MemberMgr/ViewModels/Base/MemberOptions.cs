using DataLayer.MemberMgr.Models;
using System;

namespace Service.MemberMgr.ViewModels.Base
{
    public class MemberOptionsVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MemberOptionsVm()
        {
            // Default Constructor
        }

        internal MemberOptionsVm(MemberOptions view = null)
        {
            if (view == null)
                return;

            _emailToken = view.EmailToken;
            _resetToken = view.ResetToken;
            _isValidated = view.IsValidated;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Reset Token
        /// </summary>
        public string ResetToken
        {
            get { return _resetToken; }
        }
        private string _resetToken;

        /// <summary>
        /// Email Validation token
        /// </summary>
        public string EmailToken
        {
            get { return _emailToken; }
        }
        private string _emailToken;

        public bool IsValidated
        {
            get { return _isValidated; }
        }
        private bool _isValidated;

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view">MemberOptions</param>
        /// <returns>MemberOptions</returns>
        internal virtual MemberOptions ToEntity(MemberOptions view = null)
        {
            if (view == null)
                view = new MemberOptions();

            return view;
        }

        /// <summary>
        /// Use to create a new database record
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <returns>MemberOptions</returns>
        internal virtual MemberOptions ToEntityCreate(int id)
        {
            var view = ToEntity();

            _resetToken = Guid.NewGuid().ToString("N");
            _emailToken = Guid.NewGuid().ToString("N");

            view.ResetToken = _resetToken;
            view.EmailToken = _emailToken;
            view.IsValidated = IsValidated;

            return view;
        }

        #endregion Methods

    }
}
