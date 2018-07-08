using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.MemberMgr.ViewModels.Base;
using Service.MemberMgr.Service;
using Utilities.Member.Enum;
using Utilities.Member.Settings;
using Newtonsoft.Json;
using Utilities;

namespace LogicTest.MemberMgr
{
    [TestClass]
    public class MemberSvcTest
    {
        private const string DB_CONN = "name=DbConnection";
        private static string userName = String.Empty;
        private static string emailAddress = String.Empty;

        private static string _passwordToken = String.Empty;

        private MemberVm _member;
        private MemberOptionsVm _memberOptions;
        private MemberLoginVm _memberLogin;

        private MemberManagerSvc _managerSvc = null;
        MemberManagerSvc ManagerSvc => _managerSvc ?? (_managerSvc = new MemberManagerSvc(DB_CONN));

        private MemberSvc _memberSvc = null;
        MemberSvc MemberSvc => _memberSvc ?? (_memberSvc = new MemberSvc(DB_CONN));

        #region Properties

        private MemberManagerVm _managerVm;
        public MemberManagerVm Manager
        {
            get
            {
                if (_managerVm == null)
                {
                    #region Master 
                    var manager = new MemberManagerVm()
                    {
                        Name = "Test Manager"
                    };

                    var settings = new MemberManagerSettingsVm()
                    {
                        AutoValidateUser = false,
                        RestrictEmail = true,
                        Status = MemberManagerStatus.Pending
                    };

                    var dbRes = ManagerSvc.Create(0, manager, settings);

                    _managerVm = dbRes.Manager;
                    #endregion Master 
                }

                return _managerVm;
            }
        }

        private MemberManagerVm _partnerVm;
        public MemberManagerVm Partner
        {
            get
            {
                if (_partnerVm == null)
                {
                    #region Partner 
                    var manager = new MemberManagerVm()
                    {
                        Name = "Test Partner Manager"
                    };

                    var settings = new MemberManagerSettingsVm()
                    {
                        AutoValidateUser = false,
                        RestrictEmail = true,
                        Status = MemberManagerStatus.Pending
                    };

                    var dbRes = ManagerSvc.Create(0, manager, settings);

                    _partnerVm = dbRes.Manager;
                    #endregion Master 
                }

                return _partnerVm;
            }
        }

        #endregion Properties

        #region Test Playlist

        [TestMethod()]
        public void MemberSvc_Create_Member()
        {
            RegisterMember();
            ValidateEmailToken();
            FindMemberById();
            FindMemberByProviderKey();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_Create_Member_Display_Check()
        {
            RegisterMemberWithoutDisplayName();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_Create_MemberLogin()
        {
            RegisterMember();
            ValidateEmailToken();
            AddLogin();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_EmailToken_Invalid()
        {
            RegisterMember();
            InvalidEmailToken();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_EmailExist()
        {
            RegisterMember();
            ValidateEmailToken();
            ExistingEmailValidation();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_UsernameExist()
        {
            RegisterMember();
            ValidateEmailToken();
            ExistingUsernameValidation();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_Request_Reset_Password()
        {
            RegisterMember();
            ValidateEmailToken();
            RequestPasswordToken();
            ResetPassword();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_Reset_Password_Invalid()
        {
            RegisterMember();
            ValidateEmailToken();
            RequestPasswordToken();
            InvalidResetPassword();
            CleanUp();
        }

        [TestMethod()]
        public void MemberSvc_Reset_Password_Expire()
        {
            RegisterMember();
            ValidateEmailToken();
            RequestPasswordToken();
            ExpiredResetPassword();
            CleanUp();
        }

        #endregion Test Playlist

        #region Test Methods

        public void RegisterMember()
        {
            // Generate a random email address
            userName = Guid.NewGuid().ToString().Replace("-", "_");
            emailAddress += userName + "@test.com";

            var member = new MemberVm()
            {
                Email = emailAddress,
                Password = "testPassword123",
                DisplayName = "Sam Sample",
                Username = userName
            };

            var dbMember = MemberSvc.CreateWithApplication(Manager.Id, member);

            _member = dbMember.Member;
            _memberOptions = dbMember.Options;
            _memberLogin = dbMember.Login;

            Assert.AreNotEqual(_member.Id, 0);

        }

        public void RegisterMemberWithoutDisplayName()
        {
            // Generate a random email address
            userName = Guid.NewGuid().ToString().Replace("-", "_");
            emailAddress += userName + "@test.com";

            try
            {
                var member = new MemberVm()
                {
                    Email = emailAddress,
                    Password = "testPassword123",
                    Username = userName
                };

                var dbMember = MemberSvc.CreateWithApplication(Manager.Id, member);

                _member = dbMember.Member;
                _memberOptions = dbMember.Options;
                _memberLogin = dbMember.Login;

                Assert.AreEqual(MemberManagerMessages.Error.MEMBER_DISPLAY_NAME_REQUIRED, dbMember.Message);
            }
            catch (Exception e)
            {
                Assert.AreEqual(MemberManagerMessages.Error.MEMBER_DISPLAY_NAME_REQUIRED, e.Message);
            }

        }

        public void AddLogin()
        {

            if (_member == null)
                Assert.Fail("AddLogin: Test Failed to add new login.");

            var dbLogin = MemberSvc.TryAddLogin(Partner.Id, _member.Id);

            Assert.AreNotEqual(dbLogin, null);

        }

        public void ValidateEmailToken()
        {
            if (_member == null)
                Assert.Fail("ValidateEmailToken: Test failed to create new member.");

            var result = MemberSvc.ValidateEmail(_member.Id, _memberOptions.EmailToken);
            Assert.AreEqual(true, result.IsSuccess);
        }

        public void RequestPasswordToken()
        {
            if (_member == null)
                Assert.Fail("ValidateEmailToken: Test failed to create new member.");

            var result = MemberSvc.CreateResetToken(_member.Id, MemberResetTokenType.Password);
            _passwordToken = result.Token;

            Assert.AreEqual(true, result.IsSuccess);
        }

        public void ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(_passwordToken))
                Assert.Fail("Failed to test the reset token, the token is empty.");

            var result = MemberSvc.ResetPassword(_member.Id, "testNewPasswordReset", _passwordToken);
            Assert.AreEqual(true, result.IsSuccess);
        }

        public void FindMemberById()
        {
            if (_member == null)
                Assert.Fail("FindMemberById : Test Failed to create new user");

            var dbMember = MemberSvc.Get(_member.Id);
            Assert.AreEqual(_member.Id, dbMember.Id);
        }

        public void FindMemberByProviderKey()
        {
            if (_member == null)
                Assert.Fail("FindMemberById : Test Failed to create new user");

            var dbMember = MemberSvc.Get(_member.Id);
            Assert.AreEqual(_member.Id, dbMember.Id);
        }

        public void ExistingEmailValidation()
        {

            var member = new MemberVm()
            {
                Email = emailAddress,
                Password = "testPassword123",
                Username = "testUsername"
            };

            try
            {
                var dbMember = MemberSvc.CreateWithApplication(Manager.Id, member);
                Assert.AreEqual(MemberManagerMessages.Error.EMAIL_EXISTS, dbMember.Message); 
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, MemberManagerMessages.Error.EMAIL_EXISTS);
            }

        }

        public void ExistingUsernameValidation()
        {

            var member = new MemberVm()
            {
                Email = emailAddress,
                Password = "testPassword123",
                Username = userName
            };

            try
            {
                var dbMember = MemberSvc.CreateWithApplication(Manager.Id, member);
                Assert.AreEqual(MemberManagerMessages.Error.USERNAME_EXISTS,dbMember.Message);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, MemberManagerMessages.Error.USERNAME_EXISTS);
            }

        }

        public void InvalidEmailToken()
        {
            if (_member == null)
                Assert.Fail("InvalidEmailToken: Test failed to create new member.");

            string token = Guid.NewGuid().ToString();
            var result = MemberSvc.ValidateEmail(_member.Id, token);
            Assert.AreEqual(false, result.IsSuccess);
        }

        public void InvalidResetPassword()
        {
            if (_member == null)
                Assert.Fail("InvalidResetPassword: Test failed to create new member.");

            var jsnToken = new
            {
                Token = Guid.NewGuid().ToString(),
                Expire = DateTime.UtcNow.AddHours(5)
            };

            string stringToken = JsonConvert.SerializeObject(jsnToken);
            stringToken = Cryptography.Encrypt(stringToken, _member.CryptoKey);
            stringToken = Cryptography.EncryptToUrlFriendly(stringToken);
            var result = MemberSvc.ValidateEmail(_member.Id, stringToken);

            Assert.AreEqual(false, result.IsSuccess);
        }

        public void ExpiredResetPassword()
        {
            if (_member == null)
                Assert.Fail("InvalidResetPassword: Test failed to create new member.");

            string stringToken = Cryptography.DecryptToFromUrlFriendlyToken(_passwordToken);
            stringToken = Cryptography.Decrypt(stringToken, _member.CryptoKey);

            var jsnToken = JsonConvert.DeserializeObject<ResetTokenVm>(stringToken);

            jsnToken.Expire = DateTime.UtcNow.AddHours(50);
            stringToken = JsonConvert.SerializeObject(jsnToken);
            stringToken = Cryptography.Encrypt(stringToken, _member.CryptoKey);
            stringToken = Cryptography.EncryptToUrlFriendly(stringToken);

            var result = MemberSvc.ResetPassword(_member.Id, "myNewSpankingPassword", stringToken);

            Assert.AreEqual(MemberManagerMessages.Error.PASSWORD_TOKEN_EXPIRED, result.Message);
        }

        public void CleanUp()
        {
            var isMemberDelete = false;
            var isApplicationDeleted = false;
            var isPartnerDeleted = false;

            if (_member != null)
                isMemberDelete = MemberSvc.Delete(_member.Id).IsSuccess;

            if (_managerVm != null)
                isApplicationDeleted = ManagerSvc.Delete(_managerVm.Id).IsSuccess;

            if (_partnerVm != null)
                isPartnerDeleted = ManagerSvc.Delete(_partnerVm.Id).IsSuccess;

        }

        #endregion Test Methods
        
    }
}
