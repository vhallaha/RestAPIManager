using System.Data.Entity.Infrastructure;

namespace DataLayer.ResourceMgr
{
    /// <summary>
    /// Implementation of Resource Manager Db Context Factory
    /// </summary>
    internal class ResourceManagerDbContextFactory : IDbContextFactory<ResourceManagerDbContext>
    {

        public ResourceManagerDbContext Create()
        {
            return ResourceManagerDbContext.Create();
        }

    }
}
