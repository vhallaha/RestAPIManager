using DataLayer.MemberMgr.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;

namespace Service.MemberMgr.ViewModels.Base
{
    public class MemberVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MemberVm()
        {
            // Default Constructor
        }

        internal MemberVm(Member view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _cryptoKey = view.CryptoKey;
            _createDate = view.CreateDate;

            Username = view.Username;
            Password = view.Password;
            Email = view.Email;
            DisplayName = view.DisplayName;

            if (!string.IsNullOrWhiteSpace(view.Metadata))
                Metadata = JsonConvert.DeserializeObject<dynamic>(view.Metadata);
            else
                Metadata = null;

        }

        internal MemberVm(Member view, int managerId)
        {
            if (view == null)
                return;

            _id = view.Id;
            _cryptoKey = view.CryptoKey;
            _createDate = view.CreateDate;

            Username = view.Username;
            Password = view.Password;
            Email = view.Email;
            DisplayName = view.DisplayName;

            if (!string.IsNullOrWhiteSpace(view.Metadata))
                Metadata = JsonConvert.DeserializeObject<dynamic>(view.Metadata);
            else
                Metadata = null;

            ProviderKey = view.Logins.FirstOrDefault(f => f.MemberManagerId == managerId)?.ProviderKey;
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
        public string CryptoKey
        {
            get { return _cryptoKey; }
        }
        private string _cryptoKey;

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate
        {
            get { return _createDate; }
        }
        private DateTime _createDate;

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
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public dynamic Metadata { get; set; }

        /// <summary>
        /// Provider Key
        /// </summary>
        public string ProviderKey { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Use to update the database record
        /// </summary>
        /// <param name="view">Member</param>
        /// <returns>Member</returns>
        internal virtual Member ToEntity(Member view = null)
        {
            if (view == null)
                view = new Member();

            view.Username = Username;
            view.Password = Password;
            view.Email = Email;
            view.DisplayName = DisplayName;
             
            view.Metadata = JsonConvert.SerializeObject(Metadata);

            if (_createDate != new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        /// <summary>
        /// Use to create a new database record
        /// </summary>
        /// <returns>Member</returns>
        internal virtual Member ToEntityCreate()
        {
            var view = ToEntity();

            view.CryptoKey = Guid.NewGuid().ToString("N");
            view.CreateDate = DateTime.UtcNow;
            view.Password = Cryptography.GenerateHash(view.Password);

            return view;
        }

        #endregion Methods

    }
}
