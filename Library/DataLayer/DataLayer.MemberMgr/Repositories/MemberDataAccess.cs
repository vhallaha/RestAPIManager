using DataLayer.MemberMgr.Models;
using Library.Core;

namespace DataLayer.MemberMgr.Repositories
{
    public class MemberDataAccess : CoreDataAccess<MemberManagerDbContext>
    {
        #region Generics
         
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connString">Connection String name</param>
        public MemberDataAccess(string connString)
            : base(connString)
        {
        }

        public MemberDataAccess(MemberManagerDbContext dbContext)
            : base(dbContext)
        {
        }

        #endregion Generics

        #region Private Vars

        //-------------------------------------------------
        // Member Manager
        //-------------------------------------------------
        private Repository<MemberManager> _memberManager;
        private Repository<MemberManagerSettings> _memberManagerSettings;

        //-------------------------------------------------
        // Member
        //-------------------------------------------------
        private Repository<Member> _member;
        private Repository<MemberOptions> _memberOptions;
        private Repository<MemberLogin> _memberLogin;

        #endregion Private Vars

        #region Repositories

        //-------------------------------------------------
        // Member Manager
        //-------------------------------------------------
        public Repository<MemberManager> MemberManager => _memberManager ?? (_memberManager = new Repository<MemberManager>(_dbContext));
        public Repository<MemberManagerSettings> MemberManagerSettings => _memberManagerSettings ?? (_memberManagerSettings = new Repository<MemberManagerSettings>(_dbContext));

        //-------------------------------------------------
        // Member
        //-------------------------------------------------
        public Repository<Member> Member => _member ?? (_member = new Repository<Member>(_dbContext));
        public Repository<MemberOptions> MemberOption => _memberOptions ?? (_memberOptions = new Repository<MemberOptions>(_dbContext));
        public Repository<MemberLogin> MemberLogin => _memberLogin ?? (_memberLogin = new Repository<MemberLogin>(_dbContext));

        #endregion Repositories

    }
}
