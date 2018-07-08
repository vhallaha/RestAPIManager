using DataLayer.RestProject.Models.User;
using Library.Core;

namespace DataLayer.RestProject.Config
{
    internal class UserConfig : BaseTypeConfig<User>
    {

        public UserConfig()
        {
            OverrideTableName("User", RestProjectDbContext.TABLE_REST_PREFIX);
        }

        internal static UserConfig Create()
        {
            return new UserConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.ProviderKey).IsRequired();
            Property(e => e.DisplayName).IsRequired();
        }

    }
}
