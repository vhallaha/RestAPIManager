using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ResourceConfig : BaseTypeConfig<Resource>
    {

        public ResourceConfig()
        {
            OverrideTableName("Resource", ResourceManagerDbContext.TABLE_RESOURCE_PREFIX);
        }

        internal static ResourceConfig Create()
        {
            return new ResourceConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Name).IsRequired();
            Property(e => e.CreateDate).IsRequired();
            Property(e => e.Type).IsRequired();

            Property(e => e.UpdateDate).IsOptional();
        }

    }
}
