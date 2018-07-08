using DataLayer.RestProject.Models.User;
using Library.Core;

namespace DataLayer.RestProject.Repositories
{
    public class UserDataAccess : CoreDataAccess<RestProjectDbContext>
    {

        #region Generic

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connStr">Connection String</param>
        public UserDataAccess(string connStr)
            : base(connStr)
        {

        }

        public UserDataAccess(RestProjectDbContext dbContext)
            : base(dbContext)
        {

        }

        #endregion Generic

        #region Private Vars

        private Repository<User> _user;

        #endregion Private Vars

        #region Public Vars

        public Repository<User> User => _user ?? (_user = new Repository<User>(_dbContext));

        #endregion Public Vars

        #region Methods

        public override void Dispose()
        {
            _user = null; 
        }

        #endregion Methods

    }
}
