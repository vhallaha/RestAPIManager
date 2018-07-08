using DataLayer.ResourceMgr.Models;
using System;
using Utilities.Resource.Enums;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ClientResourceAccessVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClientResourceAccessVm()
        {

        }

        internal ClientResourceAccessVm(ClientResourceAccess view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _clientId = view.ClientId;
            _resourceId = view.ResourceId;
            _createDate = view.CreateDate;
            _updateDate = view.UpdateDate;
            _resourceKey = view.ResourceKey;
            _resourceValue = view.ResourceValue;

            Status = view.Status;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Resource Key
        /// </summary>
        public string ResourceKey { get { return _resourceKey; } }
        private string _resourceKey;

        /// <summary>
        /// Client Id
        /// </summary>
        public int ClientId { get { return _clientId; } }
        private int _clientId;

        /// <summary>
        ///  Resource ID
        /// </summary>
        public int ResourceId { get { return _resourceId; } }
        private int _resourceId;

        /// <summary>
        ///  Resource ID
        /// </summary>
        public int ResourceValue { get { return _resourceValue; } }
        private int _resourceValue;

        /// <summary>
        /// Status
        /// </summary>
        public ClientResourceAccessStatus Status { get; set; }

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
        /// Use to update database record.
        /// </summary>
        /// <param name="view">Client Resource Access</param>
        /// <returns>ClientResourceAccess</returns>
        internal virtual ClientResourceAccess ToEntity(ClientResourceAccess view = null)
        {
            if (view == null)
                view = new ClientResourceAccess();

            view.Status = Status;

            if (_createDate != new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        /// <summary>
        /// Use to create a new record 
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="resourceId">Resource Id</param>
        /// <param name="resourceValue">Resource Value</param>
        /// <returns>ClientResourceAccess</returns>
        internal virtual ClientResourceAccess ToEntityCreate(int clientId, int resourceId, int resourceValue)
        {
            var view = ToEntity();

            view.ClientId = clientId;
            view.ResourceId = resourceId;
            view.ResourceValue = resourceValue;
            view.ResourceKey = Guid.NewGuid().ToString("N");

            view.CreateDate = DateTime.UtcNow;

            return view;
        }

        #endregion Methods

    }
}
