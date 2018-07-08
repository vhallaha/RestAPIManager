using DataLayer.ResourceMgr.Models;
using Library.Core;

namespace DataLayer.ResourceMgr.Config
{
    internal class ResourceSettingsConfig : BaseTypeConfig<ResourceSettings>
    {

        public ResourceSettingsConfig()
        {
            OverrideTableName("ResourceSettings", ResourceManagerDbContext.TABLE_RESOURCE_PREFIX);
        }

        internal static ResourceSettingsConfig Create()
        {
            return new ResourceSettingsConfig();
        }

        protected override void SetupKeys()
        {
            HasRequired(e => e.Resource).WithOptional(e => e.Settings).WillCascadeOnDelete(true);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Status).IsRequired();
        }

    }
}
