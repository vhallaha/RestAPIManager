using Service.RestProject.ViewModels.Base;
using System.Collections.Generic;
using Utilities.Member.Enum;
using Utilities.Member.Settings;

namespace Service.RestProject.Service
{
    public class UserMgrSvc : ServiceBase
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connStr"></param>
        public UserMgrSvc(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Get

        /// <summary>
        /// Get the list of member manager of a specific user.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <returns><![CDATA[ (IEnumerable<UserManagerVm> Managers, string Message) ]]></returns>
        public (IEnumerable<UserManagerVm> Managers, string Message) GetList(string loginProviderKey)
        {
            var resp = Get<IEnumerable<UserManagerVm>>(string.Format(API_MEMBER_MANAGER_BASE, loginProviderKey) + "List");
            return (resp.Response, resp.Message);
        }

        /// <summary>
        /// Get a specific member manager.
        /// </summary>
        /// <param name="identity">Identity</param>
        /// <returns><![CDATA[ (UserManagerVm Manager, string Message) ]]></returns>
        public (UserManagerVm Manager, string Message) Get(string loginProviderKey, string identity)
        {
            var resp = Get<UserManagerVm>(string.Format(API_MEMBER_MANAGER_BASE, loginProviderKey) + $"GetByIdentity/{identity}");
            return (resp.Response, resp.Message);
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Create a new User Manager
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="manager">User Manager View Model</param
        /// <param name="status">ADMIN OVERRIDE : Status</param>
        /// <returns><![CDATA[ (UserManagerVm Manager, bool IsSuccess, string Message) ]]></returns>
        public (UserManagerVm Manager, bool IsSuccess, string Message) Create(string loginProviderKey, UserManagerVm manager)
        {
            if (string.IsNullOrWhiteSpace(manager.Name))
                return (null, false, "Manager name length must have atleast 1 character in it.");

            var resp = Post<UserManagerVm>(string.Format(API_MEMBER_MANAGER_BASE, loginProviderKey) + "CreateManager",
                    new
                    {
                        Name = manager.Name,
                        Settings = manager.Settings
                    });

            return (resp.Response, resp.IsSuccess, resp.Message);
        }

        /// <summary>
        /// Update an existing User Manager.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="manager">User Manager View Model</param>
        /// <returns><![CDATA[ (UserManagerVm Manager, bool IsSuccess, string Message) ]]></returns>
        public (UserManagerVm Manager, bool IsSuccess, string Message) Update(string loginProviderKey, UserManagerVm manager, MemberManagerStatus? status = null)
        {
            if (string.IsNullOrWhiteSpace(manager.Name))
                return (null, false, "Manager name length must have atleast 1 character in it.");

            /* DEV NOTE : Make sure that the member manager exists in the server database 
                          before even doing the update, this also block the users from
                          from setting the status when the status is on PENDING
            ---------------------------------------------------------------------*/
            var servManager = Get(loginProviderKey, manager.Identity);
            if (servManager.Manager == null)
                return (manager, false, MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            if (servManager.Manager.Settings.Status == MemberManagerStatus.Pending ||
                servManager.Manager.Settings.Status == MemberManagerStatus.Banned)
                manager.Settings.Status = servManager.Manager.Settings.Status;

            // This will be an admin overriding the status of the manager to make it active or otherwise
            // un-ban the manager form the server.
            if (status != null)
                manager.Settings.Status = (MemberManagerStatus)status;

            var resp = Put<UserManagerVm>(string.Format(API_MEMBER_MANAGER_BASE, loginProviderKey) + "UpdateManager",
                    new
                    {
                        Identity = manager.Identity,
                        Name = manager.Name,
                        Settings = manager.Settings
                    });

            return (resp.Response, resp.IsSuccess, resp.Message);
        }

        /// <summary>
        /// Delete an existing User Manager
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="identity">Manager Identity</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message)]]></returns>
        public (bool IsSuccess, string Message) Delete(string loginProviderKey, string identity)
        {
            var resp = Delete<string>(string.Format(API_MEMBER_MANAGER_BASE + $"DeleteManager/{identity}", loginProviderKey));
            return (resp.IsSuccess, resp.Message);
        }

        #endregion Set

    }
}