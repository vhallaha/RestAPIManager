using DataLayer.ResourceMgr.Defaults;
using System.Data.Entity.Migrations;

namespace DataLayer.ResourceMgr.Migrations
{
    internal class Configuration : DbMigrationsConfiguration<ResourceManagerDbContext>
    { 
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

#if(DEBUG)
            AutomaticMigrationDataLossAllowed = true;
#endif
        }

        protected override void Seed(ResourceManagerDbContext context)
        {
            base.Seed(context);

            MemberResDefaults.SetDefault(context);
        }

    }
}
