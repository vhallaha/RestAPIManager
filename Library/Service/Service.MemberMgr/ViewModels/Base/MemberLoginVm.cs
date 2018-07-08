using DataLayer.MemberMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Member.Enum;

namespace Service.MemberMgr.ViewModels.Base
{
    public class MemberLoginVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MemberLoginVm()
        {
            // Default Constructor
        }

        internal MemberLoginVm(MemberLogin view = null)
        {
            if (view == null)
                return;

            _providerKey = view.ProviderKey;
            _memberId = view.MemberId;
            _memberManagerId = view.MemberManagerId;
            _createDate = view.CreateDate;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Provider Key
        /// </summary>
        public string ProviderKey
        {
            get { return _providerKey; }
        }
        private string _providerKey;

        /// <summary>
        /// Application Id
        /// </summary>
        public int MemberManagerId
        {
            get { return _memberManagerId; }
        }
        private int _memberManagerId;

        /// <summary>
        /// Member Id
        /// </summary>
        public int MemberId
        {
            get { return _memberId; }
        }
        private int _memberId;

        /// <summary>
        /// Account Status
        /// </summary>
        public MemberStatus Status
        {
            get { return _status; }
        }
        private MemberStatus _status;

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate
        {
            get { return _createDate; }
        }
        private DateTime _createDate;

        #endregion Properties

        #region Method

        /// <summary>
        /// Use to update the database record
        /// </summary>
        /// <param name="view">MemberLogin</param>
        /// <returns>MemberLogin</returns>
        internal MemberLogin ToEntity(MemberLogin view = null)
        {
            if (view == null)
                view = new MemberLogin();

            return view;
        }

        /// <summary>
        /// Use to create a new database record
        /// </summary>
        /// <param name="memberId">Member Id</param>
        /// <param name="memberManagerId">Member Manager Id</param>
        /// <returns>MemberLogin</returns>
        internal MemberLogin ToEntityCreate(int memberId, int memberManagerId)
        {
            var view = ToEntity();

            view.ProviderKey = Guid.NewGuid().ToString("N");
            view.CreateDate = DateTime.UtcNow;
            view.MemberManagerId = memberManagerId;
            view.MemberId = memberId;

            _status = MemberStatus.Pending;

            view.Status = _status;

            return view;
        }

        #endregion Methods

    }
}
