using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.MemberMgr.Service;
using Service.MemberMgr.ViewModels.Base;
using Utilities.Member.Enum;

namespace LogicTest.MemberMgr
{
    [TestClass]
    public class MemberManagerSvcTest
    {
        private MemberManagerVm _managerVm = null;
        private MemberManagerSettingsVm _managerSettingsVm = null; 
         
        private MemberManagerSvc _managerSvc = null;
        MemberManagerSvc ManagerSvc => _managerSvc ?? (_managerSvc = new MemberManagerSvc("name=DbConnection"));

        #region Test PlayList

        [TestMethod()]
        public void MemberManagerSvc_Create_App()
        {
            RegisterManager();
            FindById();
            FindByIdentity();
            CleanDb();
        }
         
        [TestMethod]
        public void MemberManagerSvc_Update_App()
        {
            RegisterManager();
            UpdateManager();
            CleanDb();
        }

        [TestMethod]
        public void MemberManagerSvc_Update_Setting()
        {
            RegisterManager();
            UpdateSettings();
            CleanDb();
        }
         
        #endregion Test Playlist
         
        #region TestMethods

        private void FindById()
        {
            if (_managerVm == null)
                Assert.Fail("FindById: Test Failed to  create a new manager.");

            var dbApp = ManagerSvc.Get(_managerVm.Id);
            Assert.AreEqual(_managerVm.Id, dbApp.Id);
        }

        private void FindByIdentity()
        {
            if (_managerVm == null)
                Assert.Fail("FindById: Test Failed to  create a new manager.");

            var dbApp = ManagerSvc.Get(_managerVm.Identity);
            Assert.AreEqual(_managerVm.Identity, dbApp.Identity);
        }

        public void RegisterManager()
        {

            #region Master 
            var app = new MemberManagerVm()
            {
                Name = "Test Manager"
            };

            var settings = new MemberManagerSettingsVm()
            {
                AutoValidateUser = false,
                RestrictEmail = true,
                Status = MemberManagerStatus.Pending
            };
             
            var dbRes = ManagerSvc.Create(1, app, settings);

            _managerVm = dbRes.Manager;
            _managerSettingsVm = dbRes.Settings; 
            #endregion Master 

            Assert.AreNotEqual(dbRes.Manager.Id, 0);
        }
         
        private void UpdateManager()
        {

            _managerVm.Name = "Test App (Updated)";
            var dbRes = ManagerSvc.UpdateApplication(_managerVm);
            Assert.AreEqual(dbRes.Manager.Name, _managerVm.Name);

        }

        private void UpdateSettings()
        {

            _managerSettingsVm.AutoValidateUser = false;
            var dbRes = ManagerSvc.UpdateSettings(_managerSettingsVm);
            Assert.AreEqual(_managerSettingsVm.AutoValidateUser, dbRes.Settings.AutoValidateUser);

        }
         
        private void CleanDb()
        {
            (bool IsSuccess, string Message) appDelete;
             
            if (_managerVm != null)
                appDelete = ManagerSvc.Delete(_managerVm.Id);
             
        }

        #endregion Test Methods
    }
}
