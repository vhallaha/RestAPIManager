using DataLayer.ResourceMgr.Models;
using System;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ClientVm
    {
        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClientVm()
        { }

        internal ClientVm(Client view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _createDate = view.CreateDate;
            _updateDate = view.UpdateDate;
            _ownerId = view.OwnerId;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        public int OwnerId { get { return _ownerId; } }
        private int _ownerId;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

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

        /// <summary>
        /// Use to update the database record
        /// </summary>
        /// <param name="view">Client</param>
        /// <returns>Client</returns>
        internal virtual Client ToEntity(Client view = null)
        {
            if (view == null)
                view = new Client();

            view.Name = Name;

            if (_createDate != new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        /// <summary>
        /// Use to create a new database record
        /// </summary>
        /// <returns>Client</returns>
        internal virtual Client ToEntityCreate(int ownerId)
        {
            var view = ToEntity();

            view.OwnerId = ownerId;
            view.CreateDate = DateTime.UtcNow;

            return view;
        }

        #endregion Methods

    }
}
