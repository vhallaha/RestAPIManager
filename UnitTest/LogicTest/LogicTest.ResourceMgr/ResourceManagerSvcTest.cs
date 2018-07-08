using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.ResourceMgr.Service;
using Service.ResourceMgr.ViewModels.Base;
using Utilities.Resource.Enums;
using Utilities.Shared;

namespace LogicTest.ResourceMgr
{
    [TestClass]
    public class ResourceManagerSvcTest
    {

        private const string DB_CONN = "name=DbConnection";

        private ResourceVm _resVm;
        private ResourceSettingsVm _resSettingsVm;
        private ResourceClaimVm _resClaimVm;

        private ResourceManagerSvc _managerSvc = null;
        ResourceManagerSvc ManagerSvc => _managerSvc ?? (_managerSvc = new ResourceManagerSvc(DB_CONN));

        #region Test Playlist

        [TestMethod()]
        public void ResourceManagerSvc_Create_App()
        {
            CreateManager();
            FindById();
            FindSettings();
            CleanDb();
        }

        [TestMethod()]
        public void ResourceManagerSvc_Create_Claim()
        {
            CreateManager();
            AddNewClaim();
            FindClaim();
            CleanDb();
        }

        [TestMethod()]
        public void ResourceManagerSvc_Update_App()
        {
            CreateManager();
            UpdateResource();
            CleanDb();
        }

        [TestMethod()]
        public void ResourceManagerSvc_Update_Setting()
        {
            CreateManager();
            UpdateSettings();
            CleanDb();
        }

        [TestMethod()]
        public void ResourceManagerSvc_Update_Claim()
        {
            CreateManager();
            AddNewClaim();
            UpdateClaim();
            CleanDb();
        }
        #endregion Test Playlist

        #region Test Methods

        private void FindById()
        {
            if (_resVm == null)
                Assert.Fail("FindById: Test Failed to create a new manager.");

            var dbView = ManagerSvc.Get(_resVm.Id);
            Assert.IsNotNull(dbView);
        }

        private void FindSettings()
        {
            if (_resSettingsVm == null)
                Assert.Fail("FindSettings: Test Failed to create a new settings.");

            var dbView = ManagerSvc.GetSettings(_resSettingsVm.Id);
            Assert.IsNotNull(dbView);
        }

        private void FindClaim()
        {
            if (_resClaimVm == null)
                Assert.Fail("FindClaim: Test Failed to create a new claim.");

            var dbView = ManagerSvc.GetClaim(_resClaimVm.Id);
            Assert.IsNotNull(dbView);
        }

        private void CreateManager()
        {

            #region Set Settings

            var dbSettings = new ResourceSettingsVm()
            {
                Status = ResourceStatus.Pending
            };

            #endregion Set Settings

            var dbRes = ManagerSvc.Create("Test Manager", ResourceType.Member, dbSettings);

            _resVm = dbRes.Resource;
            _resSettingsVm = dbRes.Settings;

            Assert.IsTrue(dbRes.IsSuccess);
        }

        private void AddNewClaim()
        {
            if (_resVm == null)
                Assert.Fail("AddNewClaim: Test Failed to create a new manager.");

            var dbRes = ManagerSvc.AddClaim(_resVm.Id, "/sample/claim");

            _resClaimVm = dbRes.Claim;
            Assert.IsTrue(dbRes.IsSuccess);
        }

        private void UpdateResource()
        {
            if (_resVm == null)
                Assert.Fail("UpdateResource: Test Failed to create a new manager.");

            var dbEdited = _resVm;
            dbEdited.Name = "Test Manager Edited";

            var dbRes = ManagerSvc.UpdateResource(dbEdited);
            _resVm = dbRes.Resource;

            Assert.IsTrue(dbRes.IsSuccess);
        }

        private void UpdateSettings()
        {
            if (_resSettingsVm == null)
                Assert.Fail("UpdateSettings: Test Failed to create a new settings.");

            var dbEdited = _resSettingsVm;
            dbEdited.Status = ResourceStatus.Live;

            var dbRes = ManagerSvc.UpdateSettings(dbEdited);
            _resSettingsVm = dbRes.Settings;

            Assert.IsTrue(dbRes.IsSuccess);
        }

        private void UpdateClaim()
        {
            if (_resClaimVm == null)
                Assert.Fail("UpdateClaim: Test Failed to create a new claim.");

            var dbEdited = _resClaimVm;
            dbEdited.ClaimName = "/sample/claimedited";

            var dbRes = ManagerSvc.UpdateClaim(dbEdited);


            Assert.IsTrue(dbRes.IsSuccess);
        }

        private void CleanDb()
        {
            if (_resVm != null)
                ManagerSvc.DeleteManager(_resVm.Id);
        }

        #endregion Test Methods

    }
}
