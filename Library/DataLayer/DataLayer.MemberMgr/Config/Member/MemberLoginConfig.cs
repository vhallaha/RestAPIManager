using DataLayer.MemberMgr.Models;
using Library.Core;

namespace DataLayer.MemberMgr.Config
{
    internal class MemberLoginConfig : BaseTypeConfig<MemberLogin>
    {

        public MemberLoginConfig()
        {
            OverrideTableName("MemberLogin", MemberManagerDbContext.TABLE_MEMBER_PREFIX);
        }

        internal static MemberLoginConfig Create()
        {
            return new MemberLoginConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.ProviderKey);
        }

        protected override void SetupColumns()
        {
            Property(e => e.MemberManagerId).IsRequired();
            Property(e => e.MemberId).IsRequired();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreateDate).IsRequired();
        }

    }
}
