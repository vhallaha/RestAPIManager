using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ResourceClaimConfig : BaseTypeConfig<ResourceClaim>
    {

        public ResourceClaimConfig()
        {
            OverrideTableName("ResourceClaim", ResourceManagerDbContext.TABLE_RESOURCE_PREFIX);
        }

        internal static ResourceClaimConfig Create()
        {
            return new ResourceClaimConfig();
        }

        protected override void SetupColumns()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupKeys()
        {
            Property(e => e.ClaimName).IsRequired();
            Property(e => e.CreateDate).IsRequired(); 
        }

    }
}
