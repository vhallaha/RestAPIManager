using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ClientConfig : BaseTypeConfig<Client>
    {

        public ClientConfig()
        {
            OverrideTableName("Client", ResourceManagerDbContext.TABLE_CLIENT_PREFIX);
        }

        internal static ClientConfig Create()
        {
            return new ClientConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Id).IsRequired();
            Property(e => e.Name).IsRequired();
            Property(e => e.CreateDate).IsRequired(); 

            Property(e => e.UpdateDate).IsOptional();
        }

    }
}
