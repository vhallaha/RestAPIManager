using DataLayer.MemberMgr.Models;
using Library.Core;

namespace DataLayer.MemberMgr.Config
{
    internal class MemberConfig : BaseTypeConfig<Member>
    {

        public MemberConfig()
        {
            OverrideTableName("Member", MemberManagerDbContext.TABLE_MEMBER_PREFIX);
        }

        internal static MemberConfig Create()
        {
            return new MemberConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Username).IsRequired();
            Property(e => e.Password).IsRequired();
            Property(e => e.CreateDate).IsRequired();
            Property(e => e.DisplayName).IsRequired();
        }

    }
}
