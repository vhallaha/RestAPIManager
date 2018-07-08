using Service.RestProject.ViewModels.Base;
using Service.RestProject.ViewModels.Forms;
using System.Collections.Generic;
using Utilities.Member.Enum;

namespace Service.RestProject.Service
{
    public class UserSvc : ServiceBase
    {
        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connStr">Connection String</param>
        public UserSvc(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Get

        /// <summary>
        /// Get single member
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns><![CDATA[ (UserVm User, string Message) ]]></returns>
        public (UserVm User, string Message) Get(string username, string password)
        {
            var resp = Post<UserVm>(API_MEMBER_BASE + "GetByLoginCredentials/", new { username = username, password = password });
            return (resp.Response, resp.Message);
        }

        public (UserVm User, string Message) GetByProviderKey(string loginProviderKey)
        {
            var resp = Get<UserVm>(API_MEMBER_BASE + $"GetByProviderKey/{loginProviderKey}");
            return (resp.Response, resp.Message);
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <returns><![CDATA[ (IEnumerable<UserVm> Users, string Message) ]]></returns>
        public (IEnumerable<UserVm> Users, string Message) GetList()
        {
            var resp = Get<IEnumerable<UserVm>>(API_MEMBER_BASE + "List/");
            return (resp.Response, resp.Message);
        }

        /// <summary>
        /// Generates a new Reset Token to be use to re-verify email or reset password.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="type">Member Reset Token Type</param>
        /// <returns>Token</returns>
        public (string Token, bool IsSuccess) RequestToken(string loginProviderKey, MemberResetTokenType type)
        {
            var resp = Post<string>(API_MEMBER_BASE + "RequestResetToken/", new
            {
                ProviderKey = loginProviderKey,
                Type = type
            });
            return (resp.Response, resp.IsSuccess);
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Create new Rest users.
        /// </summary>
        /// <param name="user">Signup View Model</param>
        /// <returns>UserVm</returns>
        public (UserVm User, string Message) Create(SignupVm user)
        {

            if (string.IsNullOrWhiteSpace(user.DisplayName))
                return (null, "Display name length must have atleast 1 character in it.");

            if (user.Password != user.ConfirmPassword)
                return (null, "Confirm password did not match.");

            var resp = Post<UserVm>(API_MEMBER_BASE + "CreateMember/", new
            {
                username = user.Username,
                password = user.Password,
                email = user.Username,
                displayname = user.DisplayName
            });
            return (resp.Response, resp.Message);

        }

        /// <summary>
        /// Update the record of the existing member
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public (UserVm User, bool IsSuccess, string Message) Update(UserVm view)
        {
            var resp = Post<UserVm>(API_MEMBER_BASE + "UpdateMember/", new
            {
                ResourceKey = RPSettings.APIMemberResKey,
                ProviderKey = view.ProviderKey,
                Email = view.Email,
                DisplayName = view.DisplayName,
                Metadata = view.Metadata
            });

            return resp;
        }

        /// <summary>
        /// Verify Email Address
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="token">Email Token</param>
        /// <returns>Message</returns>
        public (string Message, bool IsSuccess) ValidateEmail(string loginProviderKey, string token)
        {
            var resp = Post<string>(API_MEMBER_BASE + "ValidateEmail/", new
            {
                ProviderKey = loginProviderKey,
                Token = token
            });

            return (resp.Response, resp.IsSuccess);
        }

        /// <summary>
        /// Change the password of the existing user globaly
        /// </summary>
        /// <param name="loginProviderKey">loginProviderKey</param>
        /// <param name="changePassword">ChangePasswordVm</param>
        /// <returns>Message</returns>
        public (string Message, bool IsSuccess) ChangePassword(string loginProviderKey, ChangePasswordVm changePassword)
        {

            if (string.IsNullOrWhiteSpace(changePassword.NewPassword))
                return ("Password needs to have alphanumeric characters.", false);

            if (changePassword.NewPassword != changePassword.ConfirmPassword)
                return ("Confirm password did not match.", false);

            var resp = Post<string>(API_MEMBER_BASE + "ChangePassword/", new
            {
                ProviderKey = loginProviderKey,
                OldPassword = changePassword.OldPassword,
                NewPassword = changePassword.NewPassword,
                ConfirmPassword = changePassword.ConfirmPassword
            });

            return (resp.Response, resp.IsSuccess);
        }

        /// <summary>
        /// Change the password of the existing user globaly
        /// </summary>
        /// <param name="loginProviderKey">loginProviderKey</param>
        /// <param name="changePassword">ChangePasswordVm</param>
        /// <returns>Message</returns>
        public (string Message, bool IsSuccess) ChangeUsername(string loginProviderKey, string username)
        {
            var resp = Post<string>(API_MEMBER_BASE + "ChangeUserName/", new
            {
                ProviderKey = loginProviderKey,
                Username = username
            });

            return (resp.Response, resp.IsSuccess);
        }

        /// <summary>
        /// Reset the password using reset token.
        /// </summary>
        /// <param name="loginProviderKey">Login Provider Key</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmPassword">Confirm Password</param>
        /// <param name="resetToken">Reset Token</param>
        /// <returns></returns>
        public (string Message, bool IsSucess) ResetPassword(string loginProviderKey, string newPassword, string confirmPassword, string resetToken)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                return ("Password needs to have alphanumeric characters.", false);

            if (newPassword != confirmPassword)
                return ("Confirm password did not match.", false);

            var resp = Post<string>(API_MEMBER_BASE + "ResetPassword/", new
            {
                ProviderKey = loginProviderKey,
                NewPassword = newPassword,
                resetToken = resetToken
            });

            return (resp.Response, resp.IsSuccess);
        }

        #endregion Set

    }
}
