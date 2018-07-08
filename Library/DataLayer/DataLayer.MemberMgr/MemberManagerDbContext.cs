using DataLayer.MemberMgr.Config;
using DataLayer.MemberMgr.Models;
using System.Data.Entity;

namespace DataLayer.MemberMgr
{
    public class MemberManagerDbContext : DbContext
    {

        #region Const Vars

        /// <summary>
        /// Member Resource Prefix
        /// </summary>
        internal const string TABLE_MEMBER_PREFIX = "res_";

        #endregion Const Vars

        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String</param>
        public MemberManagerDbContext(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Initialize Member Manager Config
            modelBuilder.Configurations.Add(MemberManagerConfig.Create());
            modelBuilder.Configurations.Add(MemberManagerSettingsConfig.Create());

            // Initialize Member Config
            modelBuilder.Configurations.Add(MemberConfig.Create());
            modelBuilder.Configurations.Add(MemberLoginConfig.Create());
            modelBuilder.Configurations.Add(MemberOptionsConfig.Create());

        }

        #endregion Overrides

        #region Methods

        public static MemberManagerDbContext Create(string defaultConnString = "name=DbConnection")
        {
            return new MemberManagerDbContext(defaultConnString);
        }

        #endregion Methods

        #region DbContext Tables

        /*--------------------------------------------------------------------------------
         *  MEMBER MANAGER DBSETS
        --------------------------------------------------------------------------------*/
        public DbSet<MemberManager> MemberManager { get; set; }
        public DbSet<MemberManagerSettings> MemberManagerSettings { get; set; }

        /*--------------------------------------------------------------------------------
         *  MEMBER DBSETS
        --------------------------------------------------------------------------------*/
        public DbSet<Member> Member { get; set; }
        public DbSet<MemberOptions> MemberOption { get; set; }
        public DbSet<MemberLogin> MemberLogin { get; set; }

        #endregion DbContext Tables

    }
}
