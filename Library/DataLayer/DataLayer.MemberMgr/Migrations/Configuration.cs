using DataLayer.MemberMgr.Defaults;
using System.Data.Entity.Migrations;

namespace DataLayer.MemberMgr.Migrations
{
    internal class Configuration : DbMigrationsConfiguration<MemberManagerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

#if(DEBUG)
            AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(MemberManagerDbContext context)
        {
            base.Seed(context);

            MemberDefaults.SetDefault(context);
        }

    }
}
