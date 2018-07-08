using DataLayer.ResourceMgr.Models;
using System;
using Utilities.Resource.Enums;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ClientKeyVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClientKeyVm()
        { }

        public ClientKeyVm(ClientKey view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _clientId = view.ClientId;
            _apiKey = view.APIKey;
            _apiSecret = view.APISecret;
            _createDate = view.CreateDate;
            _updateDate = view.UpdateDate;

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
        /// Client Id
        /// </summary>
        public int ClientId { get { return _clientId; } }
        private int _clientId;

        /// <summary>
        /// API Key
        /// </summary>
        public string APIKey { get { return _apiKey; } }
        private string _apiKey;

        /// <summary>
        /// API Secret
        /// </summary>
        public string APISecret { get { return _apiSecret; } }
        private string _apiSecret;

        /// <summary>
        /// Status
        /// </summary>
        public ClientKeyStatus Status { get; set; }

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
        /// Use to update the database record.
        /// </summary>
        /// <param name="view">ClientKey</param>
        /// <returns>ClientKey</returns>
        internal virtual ClientKey ToEntity(ClientKey view = null)
        {
            if (view == null)
                view = new ClientKey();

            view.Status = Status;

            if (_createDate == new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        /// <summary>
        /// Use to create a new database record
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <returns>ClientKey</returns>
        internal virtual ClientKey ToEntityCreate(int clientId)
        {
            var view = ToEntity();

            view.ClientId = clientId;
            view.APIKey = Guid.NewGuid().ToString("N");
            view.APISecret = Guid.NewGuid().ToString("N");
            view.CreateDate = DateTime.UtcNow;

            return view;
        }

        #endregion Methods

    }
}
