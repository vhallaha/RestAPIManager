using DataLayer.ResourceMgr.Models;
using Utilities.Resource.Enums;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ClientResourceAccessClaimVm
    {
        private int _id;
        private int _clientResourceAccessId;
        private int _resourceClaimId;
        private string _claimName;

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClientResourceAccessClaimVm()
        { }

        public ClientResourceAccessClaimVm(ClientResourceAccessClaim view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _clientResourceAccessId = view.ClientResourceAccessId;
            _resourceClaimId = view.ResourceClaimId;

            Access = view.Access;
        }

        public ClientResourceAccessClaimVm(ClientResourceAccessClaim view = null, ResourceClaim claim = null) 
        {
            if (view == null)
                return;

            _id = view.Id;
            _clientResourceAccessId = view.ClientResourceAccessId;
            _resourceClaimId = view.ResourceClaimId;
            _claimName = claim.ClaimName;

            Access = view.Access;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return _id; } }

        /// <summary>
        /// Client Resource Access Id
        /// </summary>
        public int ClientResourceAccessId { get { return _clientResourceAccessId; } }

        /// <summary>
        /// Resource Claims Id
        /// </summary>
        public int ResourceClaimId { get { return _resourceClaimId; } }

        public string ClaimName { get { return _claimName; } }

        /// <summary>
        /// Access
        /// </summary>
        public ClientResourceClaimsAccess Access { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Use to update a database record.
        /// </summary>
        /// <param name="view">Client Resource Access Claim</param>
        /// <returns>ClientResourceAccessClaim</returns>
        internal virtual ClientResourceAccessClaim ToEntity(ClientResourceAccessClaim view = null)
        {
            if (view == null)
                view = new ClientResourceAccessClaim();

            view.Access = view.Access;

            return view;
        }

        /// <summary>
        /// Use to cratea a database record
        /// </summary>
        /// <param name="clientResourceId">Client Resource Id</param>
        /// <param name="resourceClaimsId">Resource Claim Id</param>
        /// <returns>ClientResourceAccessClaim</returns>
        internal virtual ClientResourceAccessClaim ToEntityCreate(int clientResourceId, int resourceClaimId)
        {
            var view = ToEntity();

            view.ClientResourceAccessId = clientResourceId;
            view.ResourceClaimId = resourceClaimId;

            return view;
        }

        #endregion Methods

    }
}
