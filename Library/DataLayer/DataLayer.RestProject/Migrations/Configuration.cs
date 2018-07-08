using System.Data.Entity.Migrations;

namespace DataLayer.RestProject.Migrations
{
    public class Configuration : DbMigrationsConfiguration<RestProjectDbContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

    }
}
