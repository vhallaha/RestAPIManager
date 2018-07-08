using Library.Core;
using Utilities.Resource.Enums;

namespace DataLayer.ResourceMgr.Models
{
    public class ClientResourceAccessClaim : CoreModel
    {
        //----------------------------------------
        // Client Resource Access Claims Properites
        //----------------------------------------

        /// <summary>
        /// Client Resource Access Id
        /// </summary>
        public int ClientResourceAccessId { get; set; }

        /// <summary>
        /// Resource Claims Id
        /// </summary>
        public int ResourceClaimId { get; set; }

        /// <summary>
        /// Access
        /// </summary>
        public ClientResourceClaimsAccess Access { get; set; }

        //----------------------------------------
        // Db Virtual Connection
        //----------------------------------------

        public virtual ClientResourceAccess ClientResourceAccesss { get; set; }
                 
    }
}
