using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.ResourceMgr.ViewModels.Base;
using Service.ResourceMgr.Service;
using Utilities.Resource.Enums;
using Utilities.Shared;
using Utilities.Resource.Settings;

namespace LogicTest.ResourceMgr
{
    [TestClass]
    public class ClientSvcTest
    {

        private const string DB_CONN = "name=DbConnection";
        private string clientName = Guid.NewGuid().ToString("N");

        private ResourceVm _resVm;
        private ResourceClaimVm _resClaim;

        private ClientVm _clientVm;
        private ClientKeyVm _clientKeyVm;
        private ClientResourceAccessVm _clientResourceAccessVm;
        private ClientResourceAccessClaimVm _clientResourceClaimVm;

        private ResourceManagerSvc _resManagerSvc = null;
        ResourceManagerSvc ResManagerSvc => _resManagerSvc ?? (_resManagerSvc = new ResourceManagerSvc(DB_CONN));

        private ClientSvc _clientSvc = null;
        ClientSvc ClientSvc => _clientSvc ?? (_clientSvc = new ClientSvc(DB_CONN));

        ResourceVm ResourceVm
        {
            get
            {

                if (_resVm == null)
                {

                    #region Set Settings

                    var dbSettings = new ResourceSettingsVm()
                    {
                        Status = ResourceStatus.Pending
                    };

                    #endregion Set Settings

                    var dbRes = ResManagerSvc.Create("Test Manager", ResourceType.Member, dbSettings);

                    _resVm = dbRes.Resource;
                }

                return _resVm;
            }
        }
        ResourceClaimVm ResourceClaimVm
        {
            get
            {

                if (_resClaim == null)
                {
                    var dbRes = ResManagerSvc.AddClaim(ResourceVm.Id, "/sample/claim");
                    _resClaim = dbRes.Claim;
                }

                return _resClaim;
            }
        }

        #region Test Playlist

        [TestMethod()]
        public void ClientSvc_Create_Client()
        {
            CreateClient();
            FindById();
            FindClientKeys();
            FindKeyByAPIKey();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_Create_Key()
        {
            CreateClient();
            CreateKey();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_Create_ResourceAccess()
        {
            CreateClient();
            CreateResourceAccess();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_Create_ResourceAccessClaim()
        {
            CreateClient();
            CreateResourceAccess();
            CreateResourceAccessClaim();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_Update_KeyStatus()
        {
            CreateClient();
            CreateKey();
            UpdateKeyStatus();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_UpdateClaimAccess()
        {
            CreateClient();
            CreateResourceAccess();
            CreateResourceAccessClaim();
            UpdateClaimAccess();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_ExistingClient()
        {
            CreateClient();
            ExistingClient();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_InvalidClientKeyCreate()
        {
            InvalidClientKeyCreate();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_DataAccessAlreadyExist()
        {
            CreateClient();
            CreateResourceAccess();
            DataAccessAlreadyExist();
            CleanDb();
        }

        [TestMethod()]
        public void ClientSvc_DataAccessClaimAlreadyExist()
        {
            CreateClient();
            CreateResourceAccess();
            CreateResourceAccessClaim();
            ClaimAlreadyExists();
            CleanDb();
        }

        #endregion Test Playlist

        #region Test Methods

        private void FindById()
        {
            if (_clientVm == null)
                Assert.Fail("FindById: Test Failed to create a new client.");

            var dbView = ClientSvc.Get(_clientVm.Id);
            Assert.IsNotNull(dbView);
        }

        private void FindClientKeys()
        {
            if (_clientVm == null)
                Assert.Fail("FindById: Test Failed to create a new client key.");

            var dbView = ClientSvc.GetClientKeys(_clientVm.Id);
            Assert.IsNotNull(dbView);
        }

        private void FindKeyByAPIKey()
        {
            if (_clientKeyVm == null)
                Assert.Fail("FindById: Test Failed to create a new client key.");

            var dbView = ClientSvc.GetClientKeyByAPIKey(_clientKeyVm.APIKey);
            Assert.IsNotNull(dbView);
        }

        private void FindResourceAccess()
        {
            if (_clientResourceAccessVm == null)
                Assert.Fail("FindById: Test Failed to create a new client resource access.");

            var dbView = ClientSvc.GetClientResourceAccess(_clientResourceAccessVm.Id);
            Assert.IsNotNull(dbView);
        }

        public void FindResourceAccessClaim()
        {
            if (_clientResourceClaimVm == null)
                Assert.Fail("FindById: Test Failed to create a new client resource claim.");

            var dbView = ClientSvc.GetClientResourceClaims(_clientResourceClaimVm.Id);
            Assert.IsNotNull(dbView);
        }

        public void CreateClient()
        {
            var res = ClientSvc.CreateClient(0, clientName, ClientKeyStatus.Pending);

            _clientVm = res.Client;
            _clientKeyVm = res.Key;

            Assert.IsTrue(res.IsSuccess);
        }

        public void CreateKey()
        {

            if (_clientVm == null)
                Assert.Fail("FindById: Test Failed to create a new client.");

            var res = ClientSvc.CreateKey(_clientVm.Id, ClientKeyStatus.Open);

            if (res.IsSuccess)
                ClientSvc.DeleteClientKey(res.key.Id);

            Assert.IsTrue(res.IsSuccess);
        }

        public void CreateResourceAccess()
        {
            if (_clientVm == null)
                Assert.Fail("FindById: Test Failed to create a new client.");

            var dbRes = ClientSvc.CreateResourceAccess(_clientVm.Id, ResourceVm.Id, 0, ClientResourceAccessStatus.Pending);
            _clientResourceAccessVm = dbRes.Resource;

            Assert.IsTrue(dbRes.IsSuccess);

        }

        public void CreateResourceAccessClaim()
        {
            if (_clientResourceAccessVm == null)
                Assert.Fail("FindById: Test Failed to create a new client resource access.");

            var dbRes = ClientSvc.AddClaim(_clientResourceAccessVm.Id, ResourceClaimVm.Id, ClientResourceClaimsAccess.Allow);
            _clientResourceClaimVm = dbRes.Claim;

            Assert.IsTrue(dbRes.IsSuccess);
        }

        public void UpdateKeyStatus()
        {
            if (_clientKeyVm == null)
                Assert.Fail("FindById: Test Failed to create a new client key.");

            var dbRes = ClientSvc.UpdateKeyStatus(_clientKeyVm.Id, ClientKeyStatus.Open);
            Assert.IsTrue(dbRes.IsSuccess);
        }

        public void UpdateClaimAccess()
        {
            if (_clientResourceClaimVm == null)
                Assert.Fail("FindById: Test Failed to create a new resource claim .");

            var dbRes = ClientSvc.UpdateClaimAccess(_clientResourceClaimVm.Id, ClientResourceClaimsAccess.Allow);
            Assert.IsTrue(dbRes.IsSuccess);
        }

        public void ExistingClient()
        {
            var res = ClientSvc.CreateClient(0, clientName, ClientKeyStatus.Pending);
            Assert.AreEqual(res.Message, ResourceManagerMessages.Error.CLIENT_ADD_ALREADY_EXISTS);
        }

        public void InvalidClientKeyCreate()
        {
            var res = ClientSvc.CreateKey(0, ClientKeyStatus.Open);

            if (res.IsSuccess)
                ClientSvc.DeleteClientKey(res.key.Id);

            Assert.AreEqual(res.Message, ResourceManagerMessages.Error.CLIENT_NOT_FOUND);
        }

        public void DataAccessAlreadyExist()
        {
            var dbRes = ClientSvc.CreateResourceAccess(_clientVm.Id, ResourceVm.Id, 0, ClientResourceAccessStatus.Pending);
            _clientResourceAccessVm = dbRes.Resource;

            Assert.AreEqual(dbRes.Message, ResourceManagerMessages.Error.RESOURCE_ALREADY_EXISTS);
        }

        public void ClaimAlreadyExists()
        {
            var dbRes = ClientSvc.AddClaim(_clientResourceAccessVm.Id, ResourceClaimVm.Id, ClientResourceClaimsAccess.Allow);
            _clientResourceClaimVm = dbRes.Claim;

            Assert.AreEqual(dbRes.Message, ResourceManagerMessages.Error.CLAIM_ADD_ALREADY_EXISTS);
        }
         
        public void CleanDb()
        {
            if (_clientResourceClaimVm != null)
                ClientSvc.DeleteClaim(_clientResourceClaimVm.Id);

            if (_clientResourceAccessVm != null)
                ClientSvc.DeleteResourceAccess(_clientResourceAccessVm.Id);

            if (_clientKeyVm != null)
                ClientSvc.DeleteClientKey(_clientKeyVm.Id);

            if (_clientVm != null)
                ClientSvc.DeleteClient(_clientVm.Id);

            if (_resClaim != null)
                ResManagerSvc.DeleteClaim(_resClaim.Id);

            if (_resVm != null)
                ResManagerSvc.DeleteManager(_resVm.Id);
        }

        #endregion TestMethods

    }
}
