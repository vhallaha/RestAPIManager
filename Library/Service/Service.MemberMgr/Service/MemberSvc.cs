using DataLayer.MemberMgr.Models;
using Newtonsoft.Json;
using Service.MemberMgr.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using Utilities.Member.Enum;
using Utilities.Member.Settings;

namespace Service.MemberMgr.Service
{
    public class MemberSvc : ServiceBase
    {

        #region Ctor

        public MemberSvc(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Get

        /// <summary>
        /// Get a specific member
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <returns>MemberVm</returns>
        public MemberVm Get(int id)
        {
            var member = DataAccess.Member.GetById(id);

            if (member == null)
                return null;

            return new MemberVm(member);
        }

        /// <summary>
        /// Get a specific member by Provider Key (Login Provider Key)
        /// </summary>
        /// <param name="providerKey"></param>
        /// <returns>MemberVm</returns>
        public MemberVm GetByLoginProviderKey(string providerKey)
        {
            var member = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey);

            if (member == null)
                return null;

            return new MemberVm(member.Member);
        }

        /// <summary>
        /// Get a specific member by provider key and identity
        /// </summary>
        /// <param name="identity">Manager Identity</param>
        /// <param name="providerKey">Provider Key</param>
        /// <returns>MemberVm</returns>
        public MemberVm GetByLoginProviderKeyAndManagerIdentity(string identity, string providerKey)
        {
            identity = identity.Trim();
            providerKey = providerKey.Trim();
            var member = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey && f.Manager.Identity == identity);

            if (member == null)
                return null;

            return new MemberVm(member.Member);
        }

        /// <summary>
        /// Get a specific member by provider key and identity
        /// </summary>
        /// <param name="memberId">Manager Id</param>
        /// <param name="providerKey">Provider Key</param>
        /// <returns>MemberVm</returns>
        public MemberVm GetByLoginProviderKeyAndManagerId(int memberId, string providerKey)
        {
            providerKey = providerKey.Trim();
            var member = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey && f.Manager.Id == memberId);

            if (member == null)
                return null;

            return new MemberVm(member.Member);
        }

        /// <summary>
        /// Get the Login information of the member.
        /// </summary>
        /// <param name="providerKey">providerKey</param>
        /// <returns>MemberLoginVm</returns>
        public MemberLoginVm GetLoginProvider(string providerKey)
        {
            var login = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey);

            if (login == null)
                return null;

            return new MemberLoginVm(login);
        }

        /// <summary>
        /// Find the member using the Username and Password stored in the database.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <returns>MemberVm</returns>
        public MemberVm FindMember(string userName, string password)
        {
            password = Cryptography.GenerateHash(password);
            var member = DataAccess.Member.Find(f => f.Username == userName && f.Password == password);

            if (member == null)
                return null;

            return new MemberVm(member);
        }

        /// <summary>
        /// Find the member using the Username and Password stored in the database.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="managerId">Manager Id</param>
        /// <returns>MemberVm</returns>
        public MemberVm FindMember(string userName, string password, int managerId)
        {
            password = Cryptography.GenerateHash(password);
            var member = DataAccess.Member.Find(f => f.Username == userName && f.Password == password);

            if (member == null)
                return null;

            return new MemberVm(member, managerId);
        }

        /// <summary>
        /// List of members
        /// </summary>
        /// <param name="total">Total Page</param>
        /// <param name="index">Page</param>
        /// <param name="size">Display</param> 
        /// <returns><![CDATA[ IEnumerable<MemberVm> ]]></returns>
        public IEnumerable<MemberVm> List(out int total, int index = 0, int size = 50)
        {
            return from member in DataAccess.Member.Get(null, f => f.OrderByDescending(o => o.CreateDate), out total, index: index, size: size).ToList()
                   select new MemberVm(member);
        }

        /// <summary>
        /// Find Members of the application
        /// </summary>
        /// <param name="id">Manager Id</param>
        /// <returns><![CDATA[ IEnumerable<MemberVm> ]]></returns>
        public IEnumerable<MemberVm> FindByMemberManagerId(int id)
        {
            return from member in DataAccess.MemberLogin.Filter(f => f.MemberManagerId == id).ToList()
                   select new MemberVm(member.Member);
        }

        /// <summary>
        /// Find Members of the application
        /// </summary>
        /// <param name="id">Manager Id</param>
        /// <returns><![CDATA[ IEnumerable<MemberVm> ]]></returns>
        public IEnumerable<MemberVm> FindByMemberManagerId(int id, int managerId)
        {
            return from member in DataAccess.MemberLogin.Filter(f => f.MemberManagerId == id).ToList()
                   select new MemberVm(member.Member, managerId);
        }

        /// <summary>
        /// Find Members of the application
        /// </summary>
        /// <param name="id">Manager Id</param>
        /// <param name="total">Total Page</param>
        /// <param name="index">Page</param>
        /// <param name="size">Display</param>
        /// <returns><![CDATA[ IEnumerable<MemberVm> ]]></returns>
        public IEnumerable<MemberVm> FindByMemberManagerId(int id, out int total, int index = 0, int size = 50)
        {
            return from member in DataAccess.MemberLogin.Get(f => f.MemberManagerId == id, f => f.OrderByDescending(o => o.CreateDate), out total, index: index, size: size).ToList()
                   select new MemberVm(member.Member);
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Create a new member under specific application.
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <param name="view">Member view</param>
        /// <returns><![CDATA[ (MemberVm Member, MemberOptionsVm Options, MemberLoginVm Login) ]]></returns>
        public (MemberVm Member, MemberOptionsVm Options, MemberLoginVm Login, bool IsSuccess, string Message) CreateWithApplication(int id, MemberVm view)
        {
            string errorMessage = String.Empty;

            var results = CheckRestrictions(id, view, out errorMessage);
            if (!results.Allow)
                return (null, null, null, false, errorMessage);

            // Create the member
            var dbMember = view.ToEntityCreate();
            dbMember = DataAccess.Member.Create(dbMember);

            // Create the Options for the member based on the application.
            var dbMemberOptions = results.Options.ToEntityCreate(view.ToEntityCreate().Id);
            dbMemberOptions.Member = dbMember;
            dbMemberOptions = DataAccess.MemberOption.Create(dbMemberOptions);

            // Create Member Login for SSO
            var dbMemberLogin = new MemberLoginVm().ToEntityCreate(view.ToEntityCreate().Id, id);
            dbMemberLogin.Member = dbMember;
            dbMemberLogin = DataAccess.MemberLogin.Create(dbMemberLogin);

            // Save all transactions.
            DataAccess.Save();

            var member = new MemberVm(dbMember);
            var options = new MemberOptionsVm(dbMemberOptions);
            var login = new MemberLoginVm(dbMemberLogin);

            return (member, options, login, true, MemberManagerMessages.Success.MEMBER_CREATE);
        }

        /// <summary>
        /// Update specific Member record
        /// </summary>
        /// <param name="id">Member Manager Id</param>
        /// <param name="view">Member View Model</param>
        /// <returns></returns>
        public (MemberVm member, bool IsSuccess, string Message) UpdateMember(int id, MemberVm view)
        {
            string errorMessage = String.Empty;

            var results = CheckUpdateRestrictions(id, view, out errorMessage);
            if (!results.Allow)
                return (null, false, errorMessage);

            // Update the member

            var dbMember = DataAccess.Member.GetById(view.Id);
            dbMember = view.ToEntity(dbMember);
            DataAccess.Member.Update(dbMember);

            // Save transaction.
            DataAccess.Save();

            var member = new MemberVm(dbMember);
            return (member, true, MemberManagerMessages.Success.MEMBER_UPDATE);
        }

        /// <summary>
        /// Adds a new Login to the already existing member
        /// </summary>
        /// <param name="id">ApplicationId</param>
        /// <param name="memberId">Member Id</param> 
        /// <returns>MemberLoginVm</returns>
        public MemberLoginVm TryAddLogin(int managerId, int memberId)
        {

            var dbExists = DataAccess.MemberLogin.Find(f => f.MemberId == memberId && f.MemberManagerId == managerId);
            if (dbExists != null)
                return new MemberLoginVm(dbExists);

            // Create a new member login
            MemberLoginVm login = new MemberLoginVm();
            var dbMember = login.ToEntityCreate(memberId, managerId);
            dbMember = DataAccess.MemberLogin.Create(dbMember);

            // Save all transactions.
            DataAccess.Save();

            return new MemberLoginVm(dbMember);
        }

        /// <summary>
        /// Create a reset token for email and password.
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <param name="type">Member Reset Token Type</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Token) ]]></returns>
        public (bool IsSuccess, string Token) CreateResetToken(int id, MemberResetTokenType type)
        {

            var member = DataAccess.Member.GetById(id);

            if (member == null)
                return (false, "");

            return GenerateResetToken(member, type);
        }

        /// <summary>
        /// Create a reset token for email and password.
        /// </summary>
        /// <param name="providerKey">Provider KEy</param>
        /// <param name="type">Member Reset Token Type</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Token) ]]></returns>
        public (bool IsSuccess, string Token) CreateResetToken(string providerKey, MemberResetTokenType type)
        {

            var member = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey)?.Member;

            if (member == null)
                return (false, "");

            return GenerateResetToken(member, type);
        }

        /// <summary>
        /// Create a reset token for email and password.
        /// </summary>
        /// <param name="providerKey">Provider Key</param>
        /// <param name="type">Member Reset Token Type</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) ValidateEmail(string providerKey, string token)
        {
            var dbMember = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey)?.Member;

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoValidateEmail(dbMember, token);
        }

        /// <summary>
        /// Create a reset token for email and password.
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <param name="type">Member Reset Token Type</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) ValidateEmail(int id, string token)
        {
            var dbMember = DataAccess.Member.GetById(id);

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoValidateEmail(dbMember, token);
        }

        /// <summary>
        /// Reset the password with the new one.
        /// </summary>
        /// <param name="providerKey">Provider Key</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="token">Reset Token</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) ResetPassword(string providerKey, string newPassword, string token)
        {
            var dbMember = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey)?.Member;

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoResetPassword(dbMember, newPassword, token);
        }

        /// <summary>
        /// Reset the password with the new one.
        /// </summary>
        /// <param name="id"><MemberId</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="token">Reset Token</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) ResetPassword(int id, string newPassword, string token)
        {
            var dbMember = DataAccess.Member.GetById(id);

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoResetPassword(dbMember, newPassword, token);
        }

        /// <summary>
        /// Change Password using Provider Key
        /// </summary>
        /// <param name="providerKey">Client Provider Key</param>
        /// <param name="oldPassword">Old Password</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmPassword">Confirm Password</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) ChangePassword(string providerKey, string oldPassword, string newPassword, string confirmPassword)
        {
            var dbMember = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey)?.Member;

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoChangePassword(dbMember, oldPassword, newPassword, confirmPassword);
        }

        /// <summary>
        /// Change Password using Provider Key
        /// </summary>
        /// <param name="id"><MemberId</param>
        /// <param name="oldPassword">Old Password</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmPassword">Confirm Password</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) ChangePassword(int id, string oldPassword, string newPassword, string confirmPassword)
        {
            var dbMember = DataAccess.Member.GetById(id);

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoChangePassword(dbMember, oldPassword, newPassword, confirmPassword);
        }

        /// <summary>
        /// Update to a new username
        /// </summary>
        /// <param name="providerKey">Provider Key</param>
        /// <param name="managerId"><Manager Id</param>
        /// <param name="newUserName">User Name</param>
        /// <returns></returns>
        public (bool IsSuccess, string Message) ChangeUserName(string providerKey, int managerId, string newUserName)
        {
            var dbMember = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey)?.Member;

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoChangeUserName(dbMember, managerId, newUserName);
        }
         
        /// <summary>
        /// Update to a new username
        /// </summary>
        /// <param name="providerKey">Member Id</param>
        /// <param name="managerId"><Manager Id</param>
        /// <param name="newUserName">User Name</param>
        /// <returns></returns>
        public (bool IsSuccess, string Message) ChangeUserName(int id, int managerId, string newUserName)
        {
            var dbMember = DataAccess.Member.GetById(id);

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MEMBER_NOT_FOUND);

            return DoChangeUserName(dbMember, managerId, newUserName);
        }
         
        /// <summary>
        /// Delete a specific member login.
        /// </summary>
        /// <param name="managerId">Application Id</param>
        /// <param name="memberId">Member Id</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) DeleteLogin(int managerId, int memberId)
        {
            var dbLogin = DataAccess.MemberLogin.Find(f => f.MemberManagerId == managerId && f.MemberId == memberId);
            return DeleteLogin(dbLogin.ProviderKey);
        }

        /// <summary>
        /// Delete a specific member login.
        /// </summary>
        /// <param name="providerKey">Provider Key</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) DeleteLogin(string providerKey)
        {
            var dbLogin = DataAccess.MemberLogin.Find(f => f.ProviderKey == providerKey);
            var memberId = dbLogin.MemberId;

            if (dbLogin == null)
                return (false, "Cannot find the record with that login info in the database.");

            DataAccess.MemberLogin.Delete(dbLogin);
            DataAccess.Save();

            // If for some reason the member does not have any more manager accessing the 
            // record... automatically delete the member out of the database.
            var dbMember = DataAccess.Member.GetById(memberId);
            if (!dbMember.Logins.Any())
            {
                DataAccess.Member.Delete(dbMember);
                DataAccess.Save();
            }

            return (true, "Successfully deleted the login from the database.");
        }

        /// <summary>
        /// Delete a specific member login.
        /// </summary>
        /// <param name="id">Member Id</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) Delete(int id)
        {
            var dbLogin = DataAccess.Member.GetById(id);

            if (dbLogin == null)
                return (false, "Cannot find the record with that Id in the database.");

            DataAccess.Member.Delete(dbLogin);
            DataAccess.Save();

            return (true, "Successfully deleted the member from the database.");
        }

        #endregion Set

        #region Methods

        /// <summary>
        /// Checks the application restriction to make sure that it 
        /// follows the restriction that the user sets for the application.
        /// </summary>
        /// <param name="id">Member Manager Id</param>
        /// <param name="member">Member View</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        private (bool Allow, MemberVm Member, MemberOptionsVm Options) CheckRestrictions(int id, MemberVm member, out string message)
        {
            message = String.Empty;
            var memberOptions = new MemberOptions();

            var managerSettings = DataAccess.MemberManagerSettings.GetById(id);

            #region Application Check

            // Check the the application even exists before doing the checks to the member table.
            if (managerSettings == null)
            {
                message = MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS;
                return (false, member, new MemberOptionsVm(memberOptions));
            }

            #endregion Application Check

            #region Username Check

            // Always check for username to make sure that the username only exists once per application.
            var dbUserNameCheck = DataAccess.MemberLogin.Find(f => f.MemberManagerId == managerSettings.Id && f.Member.Username.ToLower() == member.Username.ToLower());
            if (dbUserNameCheck != null)
            {
                message = MemberManagerMessages.Error.USERNAME_EXISTS;
                return (false, member, new MemberOptionsVm(memberOptions));
            }

            #endregion Username Check

            #region Email Restriction Check

            //check for email restrictions
            if (managerSettings.RestrictEmail)
            {
                var dbMember = DataAccess.MemberLogin.Find(f => f.MemberManagerId == managerSettings.Id && f.Member.Email == member.Email);
                if (dbMember != null)
                {
                    message = MemberManagerMessages.Error.EMAIL_EXISTS;
                    return (false, member, new MemberOptionsVm(memberOptions));
                }
            }

            #endregion Email Restriction Check

            #region Display Name Check

            if (String.IsNullOrWhiteSpace(member.DisplayName))
            {
                message = MemberManagerMessages.Error.MEMBER_DISPLAY_NAME_REQUIRED;
                return (false, member, new MemberOptionsVm(memberOptions));
            }

            #endregion Display Name Check

            // automatically validate the new user based on the application settings.
            memberOptions.IsValidated = managerSettings.AutoValidateUser;

            return (true, member, new MemberOptionsVm(memberOptions));
        }

        /// <summary>
        /// Checks the application restriction to make sure that it 
        /// follows the restriction that the user sets for the application.
        /// </summary>
        /// <param name="id">Member Manager Id</param>
        /// <param name="member">Member View</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        private (bool Allow, MemberVm Member, MemberOptionsVm Options) CheckUpdateRestrictions(int id, MemberVm member, out string message)
        {
            message = String.Empty;
            var memberOptions = new MemberOptions();

            var managerSettings = DataAccess.MemberManagerSettings.GetById(id);

            #region Application Check

            // Check the the application even exists before doing the checks to the member table.
            if (managerSettings == null)
            {
                message = MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS;
                return (false, member, new MemberOptionsVm(memberOptions));
            }

            #endregion Application Check

            #region Username Check

            // Always check for username to make sure that the username only exists once per application.
            var dbUserNameCheck = DataAccess.MemberLogin.Find(f => f.MemberManagerId == managerSettings.Id && f.Member.Username.ToLower() == member.Username.ToLower());
            if (dbUserNameCheck != null && dbUserNameCheck.MemberId != member.Id)
            {
                message = MemberManagerMessages.Error.USERNAME_EXISTS;
                return (false, member, new MemberOptionsVm(memberOptions));
            }

            #endregion Username Check

            #region Email Restriction Check

            //check for email restrictions
            if (managerSettings.RestrictEmail)
            {
                var dbMember = DataAccess.MemberLogin.Find(f => f.MemberManagerId == managerSettings.Id && f.Member.Email == member.Email);
                if (dbMember != null && dbUserNameCheck.MemberId != member.Id)
                {
                    message = MemberManagerMessages.Error.EMAIL_EXISTS;
                    return (false, member, new MemberOptionsVm(memberOptions));
                }
            }

            #endregion Email Restriction Check

            #region Display Name Check

            if (String.IsNullOrWhiteSpace(member.DisplayName))
            {
                message = MemberManagerMessages.Error.MEMBER_DISPLAY_NAME_REQUIRED;
                return (false, member, new MemberOptionsVm(memberOptions));
            }

            #endregion Display Name Check

            // automatically validate the new user based on the application settings.
            memberOptions.IsValidated = managerSettings.AutoValidateUser;

            return (true, member, new MemberOptionsVm(memberOptions));
        }

        /// <summary>
        /// Validate email to activate the member.
        /// </summary>
        /// <param name="member">Member</param>
        /// <param name="token">Token</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        private (bool IsSuccess, string Message) DoValidateEmail(Member member, string token)
        {

            if (member.Options.IsValidated)
                return (false, MemberManagerMessages.Error.MEMBER_IS_ALREADY_ACTIVE);

            if (member.Options.EmailToken != token)
                return (false, MemberManagerMessages.Error.INVALID_EMAIL_TOKEN);

            DataAccess.MemberOption.Update(member.Options);
            DataAccess.Save();

            return (true, MemberManagerMessages.Success.MEMBER_ACTIVATED);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="member">Member</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="token">Token</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        private (bool IsSuccess, string Message) DoResetPassword(Member member, string newPassword, string token)
        {

            try
            {
                var stringToken = Cryptography.DecryptToFromUrlFriendlyToken(token);
                stringToken = Cryptography.Decrypt(stringToken, member.CryptoKey);

                var jsonToken = JsonConvert.DeserializeObject<ResetTokenVm>(stringToken);
                if (jsonToken.Expire > DateTime.UtcNow.AddMinutes(MemberMgrSettings.ExpireHours))
                    return (false, MemberManagerMessages.Error.PASSWORD_TOKEN_EXPIRED);

                var stringOriginalToken = Cryptography.DecryptToFromUrlFriendlyToken(member.Options.ResetToken);
                stringOriginalToken = Cryptography.Decrypt(stringOriginalToken, member.CryptoKey);

                var jsonOriginalToken = JsonConvert.DeserializeObject<ResetTokenVm>(stringOriginalToken);
                if (jsonToken.Token != jsonOriginalToken.Token)
                    return (false, MemberManagerMessages.Error.PASSWORD_TOKEN_INVALID);

                member.Password = Cryptography.GenerateHash(newPassword);
                DataAccess.Member.Update(member);
                DataAccess.Save();

                return (true, MemberManagerMessages.Success.PASSWORD_RESET);
            }
            catch
            {
                return (false, MemberManagerMessages.Error.PASSWORD_TOKEN_INVALID);
            }

        }

        /// <summary>
        /// Change the password of the known user.
        /// </summary>
        /// <param name="member">Member</param>
        /// <param name="oldPassword">Old Password</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmPassword">Confirm Password</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        private (bool IsSuccess, string Message) DoChangePassword(Member member, string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword != confirmPassword)
                    return (false, MemberManagerMessages.Error.PASSWORD_CHANGE_CONFIRM_NOT_MATCHED);

                oldPassword = Cryptography.GenerateHash(oldPassword);
                if (oldPassword != member.Password)
                    return (false, MemberManagerMessages.Error.PASSWORD_CHANGE_OLD_CURRENT_NOT_MATCHED);

                newPassword = Cryptography.GenerateHash(newPassword);
                if (newPassword == member.Password)
                    return (false, MemberManagerMessages.Error.PASSWORD_CHANGE_OLD_NEW_MATCHED);

                member.Password = newPassword;
                DataAccess.Member.Update(member);
                DataAccess.Save();

                return (true, MemberManagerMessages.Success.PASSWORD_CHANGE);
            }
            catch
            {
                return (false, MemberManagerMessages.Error.PASSWORD_CHANGE_FAILED);
            }
        }

        private (bool IsSuccess, string Message) GenerateResetToken(Member member, MemberResetTokenType type)
        {

            string token = Guid.NewGuid().ToString();
            if (type == MemberResetTokenType.Email)
                member.Options.EmailToken = token;
            else if (type == MemberResetTokenType.Password)
            {
                var tokener = new ResetTokenVm
                {
                    Token = token,
                    Expire = DateTime.UtcNow
                };

                string jsonString = JsonConvert.SerializeObject(tokener);
                string stringToken = Cryptography.Encrypt(jsonString, member.CryptoKey);
                stringToken = Cryptography.EncryptToUrlFriendly(stringToken);

                member.Options.ResetToken = stringToken;

                token = stringToken;
            }

            DataAccess.MemberOption.Update(member.Options);
            DataAccess.Save();

            return (true, token);
        }

        /// <summary>
        /// Change the password of the known user.
        /// </summary>
        /// <param name="member">Member</param>
        /// <param name="managerId">Manager Id</param>
        /// <param name="newUserName">New User Name</param> 
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        private (bool IsSuccess, string Message) DoChangeUserName(Member member, int managerId, string newUserName)
        {
            try
            {
                if (member.Username == newUserName)
                    return (false, MemberManagerMessages.Error.USERNAME_NOT_MATCH);

                var dbUserNameCheck = DataAccess.MemberLogin.Find(f => f.MemberManagerId == managerId && f.Member.Username.ToLower() == member.Username.ToLower());
                if (dbUserNameCheck != null)
                    return (false, MemberManagerMessages.Error.USERNAME_EXISTS);

                member.Username = newUserName;
                DataAccess.Member.Update(member);
                DataAccess.Save();

                return (true, MemberManagerMessages.Success.USERNAME);
            }
            catch
            {
                return (false, MemberManagerMessages.Error.USERNAME);
            }
        }

        #endregion Methods
    }
}
