using System.Data.Entity.Infrastructure;

namespace DataLayer.RestProject
{
    /// <summary>
    /// Implementation of Rest Project Db Context Factory
    /// </summary>
    internal class RestProjectDbContextFactory : IDbContextFactory<RestProjectDbContext>
    {

        public RestProjectDbContext Create()
        {
            return RestProjectDbContext.Create();
        }

    }
}
