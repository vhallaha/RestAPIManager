using DataLayer.MemberMgr.Models;
using Library.Core;

namespace DataLayer.MemberMgr.Config
{
    internal class MemberManagerSettingsConfig : BaseTypeConfig<MemberManagerSettings>
    {
        public MemberManagerSettingsConfig()
        {
            OverrideTableName("MemberManagerSettings", MemberManagerDbContext.TABLE_MEMBER_PREFIX);
        }

        internal static MemberManagerSettingsConfig Create()
        {
            return new MemberManagerSettingsConfig();
        }

        protected override void SetupKeys()
        {
            HasRequired(e => e.MemberManager).WithOptional(e => e.Settings).WillCascadeOnDelete(true);
        }

        protected override void SetupColumns()
        {
            Property(e => e.AutoValidateUser).IsRequired();
            Property(e => e.RestrictEmail).IsRequired();
        }

    }
}
