using DataLayer.RestProject;
using DataLayer.RestProject.Repositories;
using Library.Core;
using Newtonsoft.Json;
using System;
using Utilities.Shared.ApiClient;

namespace Service.RestProject.Service
{
    public class ServiceBase : GenericClassBase, IDisposable
    {
        #region Private Vars

        private RestProjectDbContext _dbContext = null;
        private UserDataAccess _userDataAccess = null;

        private APIClient _client = null;

        #endregion Private Vars
         
        #region Const

        internal const string API_MEMBER_BASE = "/API/MemberManager/Member/";
        internal const string API_MEMBER_MANAGER_BASE = "/API/MemberManager/Member/{0}/Manager/";

        #endregion Const

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connStr">Connection String Name</param>
        protected ServiceBase(string connStr)
        {
            _dbContext = new RestProjectDbContext(connStr);
        }

        #endregion Ctor

        #region Properties

        protected UserDataAccess UserDataAccess => _userDataAccess ?? (_userDataAccess = new UserDataAccess(_dbContext));

        protected APIClient Client => _client ?? (_client = new APIClient(new Uri(RPSettings.BaseUrl),
                                                                          RPSettings.APIKey,
                                                                          RPSettings.APISecret));

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            if (_client != null)
                _client = null;

            if (_dbContext != null)
                _dbContext.Dispose();

            if (_userDataAccess != null)
                _userDataAccess.Dispose();

        }

        #endregion Methods

        #region Helper

        /// <summary>
        /// Getter
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="url">End Point Url</param>
        /// <returns></returns>
        internal (T Response, bool IsSuccess, string Message) Get<T>(string url)
        {
            ErrorMsgVm error = null;
            var resp = Client.Get<T>(RPSettings.APIMemberResKey,
                                     url, out error);

            if (error != null && !String.IsNullOrWhiteSpace(error.Message))
                return (default(T), false, error.Message);
             
            return (resp, true, "success");
        }

        /// <summary>
        /// Poster
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="url">End Point Url</param>
        /// <param name="param">Data</param>
        /// <returns></returns>
        internal (T Response, bool IsSuccess, string Message) Post<T>(string url, object param)
        {
            ErrorMsgVm error = null;
            var resp = Client.Post<T>(RPSettings.APIMemberResKey, url, param, out error);

            if (error != null && !String.IsNullOrWhiteSpace(error.Message))
                return (default(T), false, error.Message);

            return (resp, true, "success");
        }

        /// <summary>
        /// Updater
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="url">End Point Url</param>
        /// <param name="param">Data</param>
        /// <returns></returns>
        internal (T Response, bool IsSuccess, string Message) Put<T>(string url, object param)
        {
            ErrorMsgVm error = null;
            var resp = Client.Put<T>(RPSettings.APIMemberResKey, url, param, out error);

            if (error != null && !String.IsNullOrWhiteSpace(error.Message))
                return (default(T), false, error.Message);

            return (resp, true, "success");
        }

        internal (T Response, bool IsSuccess, string Message) Delete<T>(string url)
        {
            ErrorMsgVm error = null;
            var resp = Client.Delete<T>(RPSettings.APIMemberResKey,
                                     url, out error);

            if (error != null && !String.IsNullOrWhiteSpace(error.Message))
                return (default(T), false, error.Message);

            return (resp, true, "success");
        }

        #endregion Helper

    }
}
