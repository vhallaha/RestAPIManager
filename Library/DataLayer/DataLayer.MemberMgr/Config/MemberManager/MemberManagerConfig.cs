using DataLayer.MemberMgr.Models;
using Library.Core;

namespace DataLayer.MemberMgr.Config
{
    internal class MemberManagerConfig : BaseTypeConfig<MemberManager>
    {

        public MemberManagerConfig()
        {
            OverrideTableName("MemberManager", MemberManagerDbContext.TABLE_MEMBER_PREFIX);
        }

        internal static MemberManagerConfig Create()
        {
            return new MemberManagerConfig();
        }

        protected override void SetupKeys()
        {
            HasKey(e => e.Id);
        }

        protected override void SetupColumns()
        {
            Property(e => e.Name).IsRequired();
            Property(e => e.CreateDate).IsRequired();

            Property(e => e.UpdateDate).IsOptional();
        }

    }
}
