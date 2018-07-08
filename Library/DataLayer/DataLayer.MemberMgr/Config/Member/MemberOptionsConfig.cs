using DataLayer.MemberMgr.Models;
using Library.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.MemberMgr.Config
{
    internal class MemberOptionsConfig : BaseTypeConfig<MemberOptions>
    {

        public MemberOptionsConfig()
        {
            OverrideTableName("MemberOptions", MemberManagerDbContext.TABLE_MEMBER_PREFIX);
        }

        internal static MemberOptionsConfig Create()
        {
            return new MemberOptionsConfig();
        }

        protected override void SetupKeys()
        {
            HasRequired(e => e.Member).WithOptional(e => e.Options).WillCascadeOnDelete(true);
        }

        protected override void SetupColumns()
        {
            Property(e => e.IsValidated).IsRequired();
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

    }
}
