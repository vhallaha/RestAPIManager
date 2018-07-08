using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ClientKeyConfig : BaseTypeConfig<ClientKey>
    {

        public ClientKeyConfig()
        {
            OverrideTableName("ClientKey", ResourceManagerDbContext.TABLE_CLIENT_PREFIX);
        }

        internal static ClientKeyConfig Create()
        {
            return new ClientKeyConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.APIKey).IsRequired();
            Property(e => e.APISecret).IsRequired();
            Property(e => e.CreateDate).IsRequired(); 
            Property(e => e.Status).IsRequired();

            Property(e => e.UpdateDate).IsOptional();
        }

    }
}
