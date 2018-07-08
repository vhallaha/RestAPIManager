using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ClientResourceAccessConfig : BaseTypeConfig<ClientResourceAccess>
    {

        public ClientResourceAccessConfig()
        {
            OverrideTableName("ClientResourceAccess", ResourceManagerDbContext.TABLE_CLIENT_PREFIX);
        }

        internal static ClientResourceAccessConfig Create()
        {
            return new ClientResourceAccessConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Status).IsRequired();
            Property(e => e.CreateDate).IsRequired();
            Property(e => e.ResourceKey).IsRequired();

            Property(e => e.UpdateDate).IsOptional();
        }

    }
}
