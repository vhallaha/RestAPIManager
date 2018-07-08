namespace Utilities.Member.Settings
{
    public static class MemberMgrSettings
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

        #endregion Application Hash Key

        #region Application Settings

        /// <summary>
        /// Allow to automatically activates the account whenever a new user signs up
        /// </summary>
        public static bool AutoActivateUser => bool.Parse(ConfigurationManager.Get("AutoActivateUser"));

        /// <summary>
        /// Denies any existing email from getting used again by the new user.
        /// </summary>
        public static bool RestrictEmail => bool.Parse(ConfigurationManager.Get("RestrictEmail"));

        /// <summary>
        /// Default system support email.
        /// </summary>
        public static string SupportEmailAddress => ConfigurationManager.Get("SupportEmail");

        /// <summary>
        /// Default system delimeter
        /// </summary>
        public static char DefaultDelimiter => '|';

        /// <summary>
        /// Age Restriction
        /// </summary>
        public static int RequiredAge => 18;

        /// <summary>
        /// Member Manager Resource Id
        /// </summary>
        public static int MemberManagerResourceId => int.Parse(ConfigurationManager.Get("memServResId"));

        #endregion Application Settings

        #region Account Settings

        /// <summary>
        /// Expiration hours.
        /// </summary>
        public static int ExpireHours => 24;

        /// <summary>
        ///  URL to activate account using the email token
        /// </summary>
        public static string EmailConfirmTokenUrl => ConfigurationManager.Get("EmailConfirmTokenUrl");

        /// <summary>
        ///  Url to reset the password using the password reset token sent by email to the user.
        /// </summary>
        public static string ResetPasswordTokenUrl => ConfigurationManager.Get("ResetPasswordTokenUrl");

        #endregion Account Settings
    }
}
