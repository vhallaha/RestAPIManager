using DataLayer.RestProject.Config;
using System.Data.Entity;

namespace DataLayer.RestProject
{
    public class RestProjectDbContext : DbContext
    {

        #region Const Vars

        /// <summary>
        /// Rest Table Prefix
        /// </summary>
        internal const string TABLE_REST_PREFIX = "rst_";

        #endregion Const Vars

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        public RestProjectDbContext(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Initialize User Config
            modelBuilder.Configurations.Add(UserConfig.Create());
        }

        #endregion Overrides

        #region Methods

        public static RestProjectDbContext Create(string defaultConnString = "name=DbConnection")
        {
            return new RestProjectDbContext(defaultConnString);
        }

        #endregion Methods

    }
}
