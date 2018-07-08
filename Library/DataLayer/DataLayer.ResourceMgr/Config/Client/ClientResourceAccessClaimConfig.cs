using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ClientResourceAccessClaimConfig : BaseTypeConfig<ClientResourceAccessClaim>
    {

        public ClientResourceAccessClaimConfig()
        {
            OverrideTableName("ClientResourceAccessClaim", ResourceManagerDbContext.TABLE_CLIENT_PREFIX);
        }

        internal static ClientResourceAccessClaimConfig Create()
        {
            return new ClientResourceAccessClaimConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Access).IsRequired();
        }

    }
}
