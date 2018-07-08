using Utilities;

namespace Service.RestProject
{
    public static class RPSettings
    {
        #region Application Hash Key

        /// <summary>
        /// System Hash Key
        /// </summary>
        public static string SystemHashKey => ConfigurationManager.Get("systemHashKey");

        /// <summary>
        /// System IV Key
        /// </summary>
        public static string SystemIVKey => ConfigurationManager.Get("systemIVKey");

        /// <summary>
        ///  System Salt
        /// </summary>
        public static string SystemSalt => ConfigurationManager.Get("systemSaltKey");

        public static int CookieTimeoutMinute => int.Parse(ConfigurationManager.Get("cookieTimeoutMinute"));

        #endregion Application Hash Key

        #region API Settings

        /// <summary>
        /// Base Url
        /// </summary>
        public static string BaseUrl => ConfigurationManager.Get("apiBaseUrl");

        /// <summary>
        /// API Key
        /// </summary>
        public static string APIKey => ConfigurationManager.Get("apiKey");

        /// <summary>
        /// API Secret
        /// </summary>
        public static string APISecret => ConfigurationManager.Get("apiSecret");

        /// <summary>
        /// Key for res member data resource.
        /// </summary>
        public static string APIMemberResKey = ConfigurationManager.Get("apiMemKey");

        #endregion API Settings

        #region DB Settings

        /// <summary>
        /// Database Connection String Name
        /// </summary>
        public static string DbConnString => ConfigurationManager.Get("dbConnStr");

        #endregion DB Settings

    }
}
