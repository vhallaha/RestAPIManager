using DataLayer.ResourceMgr.Config;
using DataLayer.ResourceMgr.Models;
using System.Data.Entity;

namespace DataLayer.ResourceMgr
{
    public class ResourceManagerDbContext : DbContext
    {

        #region Const Vars

        /// <summary>
        /// Resource Table Prefix
        /// </summary>
        internal const string TABLE_RESOURCE_PREFIX = "sys_";

        /// <summary>
        /// Client Table Prefix
        /// </summary>
        internal const string TABLE_CLIENT_PREFIX = "api_";

        #endregion Const Vars

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        public ResourceManagerDbContext(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Initialize Resource Config.
            modelBuilder.Configurations.Add(ResourceConfig.Create());
            modelBuilder.Configurations.Add(ResourceSettingsConfig.Create());
            modelBuilder.Configurations.Add(ResourceClaimConfig.Create());

            // Initialize Client Config.
            modelBuilder.Configurations.Add(ClientConfig.Create());
            modelBuilder.Configurations.Add(ClientKeyConfig.Create());
            modelBuilder.Configurations.Add(ClientResourceAccessConfig.Create());
            modelBuilder.Configurations.Add(ClientResourceAccessClaimConfig.Create());
        }

        #endregion Overrides

        #region Methods

        public static ResourceManagerDbContext Create(string defaultConnString = "name=DbConnection")
        {
            return new ResourceManagerDbContext(defaultConnString);
        }

        #endregion Methods

        #region DbContext Tables

        /*--------------------------------------------------------------------------------
         *  RESOURCE DBSETS
        --------------------------------------------------------------------------------*/
        public DbSet<Resource> Resource { get; set; }
        public DbSet<ResourceClaim> ResourceClaim { get; set; }
        public DbSet<ResourceSettings> ResourceSettings { get; set; }

        /*--------------------------------------------------------------------------------
         *  CLIENT DBSETS
        --------------------------------------------------------------------------------*/
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientKey> ClientKey { get; set; }
        public DbSet<ClientResourceAccess> ClientResourceAccess { get; set; }
        public DbSet<ClientResourceAccessClaim> ClientResourceAccessClaim { get; set; }

        #endregion DbContext Tables

    }
}
