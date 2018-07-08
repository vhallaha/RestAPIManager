namespace Utilities.Member.Settings
{
    public static class MemberManagerMessages
    {

        public static class Error
        {
            public const string MANAGER_DOES_NOT_EXISTS = "The Member Manager that you are trying to access does not exists.";
            public const string EMAIL_EXISTS = "The email that you are trying to use already exists.";
            public const string USERNAME_EXISTS = "The user name has already been taken.";
            public const string USERNAME = "Failed to updare username.";
            public const string USERNAME_NOT_MATCH = "The user name does not match.";

            public const string INVALID_EMAIL_TOKEN = "The email token provided is invalid.";
            public const string MEMBER_IS_ALREADY_ACTIVE = "The member account is already active.";

            public const string PASSWORD_TOKEN_EXPIRED = "The Password token has already been expired.";
            public const string PASSWORD_TOKEN_INVALID = "The Password token is invalid.";

            public const string PASSWORD_CHANGE_FAILED = "Failed to change the password.";
            public const string PASSWORD_CHANGE_OLD_CURRENT_NOT_MATCHED = "Failed to change the password, the old password did not match the current password.";
            public const string PASSWORD_CHANGE_OLD_NEW_MATCHED= "Failed to change the password, old and new password matched.";
            public const string PASSWORD_CHANGE_CONFIRM_NOT_MATCHED = "Failed to change the password, confirm password did not match.";

            public const string MEMBER_NOT_FOUND = "The member you are trying to access does not exists."; 
            public const string MEMBER_DISPLAY_NAME_REQUIRED = "The Display name is required.";

            public const string PROFILE_DOES_NOT_EXISTS = "The Profile does not exists.";
            public const string PROFILE_FIRST_NAME_REQUIRED = "The Firstname is required.";
            public const string PROFILE_LAST_NAME_REQUIRED = "The Firstname is required.";
            public const string PROFILE_LOCATION_REQUIRED = "The Location is required.";
            public const string PROFILE_DOB_REQUIRED = "The Date of birth is required.";
            public const string PROFILE_DOB_AGE = "You need to be atleast {0} yrs old.";

            public const string MANAGER_UPDATE = "The manager does not exist in the database.";
            public const string MANAGER_ALREADY_EXISTS = "The name is already been used.";
        }

        public static class Success
        {

            public const string MEMBER_CREATE = "Member account has been successfully created.";
            public const string MEMBER_ACTIVATED = "Member account has been successfully activated.";
            public const string MEMBER_UPDATE = "Member account has been successfully updated.";
            public const string PASSWORD_RESET = "Member password has been sucessfully reseted.";
            public const string PASSWORD_CHANGE = "Member password has been sucessfully changed.";
            public const string USERNAME = "Member username has been successfully updated.";

            public const string MANAGER_CREATED = "Successfully created a new manager.";
            public const string MANAGER_UPDATE = "Successfully updated the manager record.";
            public const string MANAGER_SETTINGS_UPDATE = "Successfully updated the member manager settings record.";

            public const string MANAGER_DELETE = "Successfully deleted the member manager.";
        }
    }
}
