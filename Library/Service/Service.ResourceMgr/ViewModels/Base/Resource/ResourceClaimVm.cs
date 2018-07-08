using DataLayer.ResourceMgr.Models;
using System;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ResourceClaimVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResourceClaimVm()
        { }

        internal ResourceClaimVm(ResourceClaim view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _resourceId = view.ResourceId;
            _createDate = view.CreateDate;
            _updateDate = view.UpdateDate;

            ClaimName = view.ClaimName;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Resource Id
        /// </summary>
        public int ResourceId { get { return _resourceId; } }
        private int _resourceId;

        /// <summary>
        /// Claim Name
        /// </summary>
        public string ClaimName { get; set; }

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
        /// <param name="view">Resource Claim</param>
        /// <returns>ResourceClaim</returns>
        internal virtual ResourceClaim ToEntity(ResourceClaim view = null)
        {
            if (view == null)
                view = new ResourceClaim();

            view.ClaimName = ClaimName;

            if (_createDate != new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        /// <summary>
        /// Use to create a new database record.
        /// </summary>
        /// <param name="resourceId">Resource Id</param>
        /// <returns>ResourceClaim</returns>
        internal virtual ResourceClaim ToEntityCreate(int resourceId)
        {
            var view = ToEntity();

            view.ResourceId = resourceId;
            view.CreateDate = DateTime.UtcNow;

            return view;
        }

        #endregion Methods

    }
}
