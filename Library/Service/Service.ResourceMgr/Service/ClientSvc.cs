using DataLayer.ResourceMgr.Models;
using Service.ResourceMgr.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Utilities.Resource.Enums;
using Utilities.Resource.Settings;
using Utilities.Shared;

namespace Service.ResourceMgr.Service
{
    public class ClientSvc : ServiceBase
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connStr">Connection String</param>
        public ClientSvc(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Get

        /// <summary>
        /// Gets a specific client from the database.
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns>ClientVm</returns>
        public ClientVm Get(int id)
        {
            var client = ClientDataAccess.Client.GetById(id);

            if (client == null)
                return null;

            return new ClientVm(client);
        }

        /// <summary>
        /// Gets a specific client key from the database.
        /// </summary>
        /// <param name="id">Client Key Id</param>
        /// <returns>ClientKeyVm</returns>
        public ClientKeyVm GetKey(int id)
        {
            var key = ClientDataAccess.ClientKey.GetById(id);

            if (key == null)
                return null;

            return new ClientKeyVm(key);
        }

        /// <summary>
        /// Gets a specific client key from database using APIKey.
        /// </summary>
        /// <param name="apiKey">APIKey</param>
        /// <returns>ClientKeyVm</returns>
        public ClientKeyVm GetClientKeyByAPIKey(string apiKey)
        {
            var key = ClientDataAccess.ClientKey.Find(f => f.APIKey == apiKey);

            if (key == null)
                return null;

            return new ClientKeyVm(key);
        }

        /// <summary>
        /// Gets a specific client resource access from the database.
        /// </summary>
        /// <param name="id">Client Resource Access ID</param>
        /// <returns>ClientResourceAccessVm</returns>
        public ClientResourceAccessVm GetResourceAccess(int id)
        {
            var access = ClientDataAccess.ClientResourceAccess.GetById(id);

            if (access == null)
                return null;

            return new ClientResourceAccessVm(access);
        }

        /// <summary>
        /// Gets a specific client resource access from the database.
        /// </summary>
        /// <param name="resKey">Resource Key</param>
        /// <returns></returns>
        public ClientResourceAccessVm GetResourceAccess(string resKey)
        {
            var access = ClientDataAccess.ClientResourceAccess.Find(f => f.ResourceKey == resKey);

            if (access == null)
                return null;

            return new ClientResourceAccessVm(access);
        }

        /// <summary>
        /// Gets a specific client resource access claims
        /// </summary>
        /// <param name="id">Client Resource Access Claim Id</param>
        /// <returns></returns>
        public ClientResourceAccessClaimVm GetResourceAccessClaim(int id)
        {
            var claim = ClientDataAccess.ClientResourceAccessClaim.GetById(id);

            if (claim == null)
                return null;

            return new ClientResourceAccessClaimVm(claim);
        }

        /// <summary>
        /// Gets all the clients that are currently in the database.
        /// </summary>
        /// <returns><![CDATA[ IEnumerable<ClientVm> ]]></returns>
        public IEnumerable<ClientVm> ClientList()
        {
            return (from c in ClientDataAccess.Client.All().ToList()
                    select new ClientVm(c));
        }

        /// <summary>
        /// Gets all the client that the owner has.
        /// </summary>
        /// <param name="id">Owner Id</param>
        /// <returns><![CDATA[ IEnumerable<ClientVm> ]]></returns>
        public IEnumerable<ClientVm> ClientListByOwnerId(int id)
        {
            return (from c in ClientDataAccess.Client.Filter(f => f.OwnerId == id).ToList()
                    select new ClientVm(c));
        }

        /// <summary>
        /// Gets all client key for the client.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns><![CDATA[ IEnumerable<ClientKeyVm> ]]></returns>
        public IEnumerable<ClientKeyVm> GetClientKeys(int id)
        {
            return (from c in ClientDataAccess.ClientKey.Filter(f => f.ClientId == id).ToList()
                    select new ClientKeyVm(c));
        }

        /// <summary>
        /// Gets all the client resource access that the client has.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns><![CDATA[ IEnumerable<ClientResourceAccessVm> ]]></returns>
        public IEnumerable<ClientResourceAccessVm> GetClientResourceAccess(int id)
        {
            return (from c in ClientDataAccess.ClientResourceAccess.Filter(f => f.ClientId == id).ToList()
                    select new ClientResourceAccessVm(c));
        }

        /// <summary>
        /// Gets all the client resource access that the client has.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns><![CDATA[ IEnumerable<ClientResourceAccessVm> ]]></returns>
        public IEnumerable<ClientResourceAccessVm> GetClientResourceAccess(int id, ResourceType type)
        {
            return (from c in ClientDataAccess.ClientResourceAccess.Filter(f => f.ClientId == id && f.Resource.Type == type).ToList()
                    select new ClientResourceAccessVm(c));
        }

        /// <summary>
        /// Gets all the claim that the client resource access has.
        /// </summary>
        /// <param name="id">Client Resource Access Id</param>
        /// <returns><![CDATA[ IEnumerable<ClientResourceAccessClaimVm> ]]></returns>
        public IEnumerable<ClientResourceAccessClaimVm> GetClientResourceClaims(int id)
        {
            return (from c in ClientDataAccess.ClientResourceAccessClaim.Filter(f => f.ClientResourceAccessId == id).ToList()
                    select new ClientResourceAccessClaimVm(c));
        }

        /// <summary>
        /// Gets all the claim that the client resource access has.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="resVal">Resource Value</param>
        /// <returns></returns>
        public IEnumerable<ClientResourceAccessClaimVm> GetClientResourceClaims(int id, int resVal)
        {
            return (from c in ClientDataAccess.ClientResourceAccessClaim.Filter(f => f.ClientResourceAccesss.ClientId == id && f.ClientResourceAccesss.ResourceValue == resVal).ToList()
                    select new ClientResourceAccessClaimVm(c, ResourceDataAccess.ResourceClaim.GetById(c.ResourceClaimId)));
        }

        /// <summary>
        /// Gets all the claim that the client resource access has.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="resKey">Resource Key</param>
        /// <returns></returns>
        public IEnumerable<ClientResourceAccessClaimVm> GetClientResourceClaims(int id, string resKey)
        {
            return (from c in ClientDataAccess.ClientResourceAccessClaim.Filter(f => f.ClientResourceAccesss.ClientId == id && f.ClientResourceAccesss.ResourceKey == resKey).ToList()
                    select new ClientResourceAccessClaimVm(c, ResourceDataAccess.ResourceClaim.GetById(c.ResourceClaimId)));
        }

        #endregion Get

        #region Set
        /*TODO : MAKE SURE THAT ALL THE TRANSACTION ON SET ARE LOGGED IN A TABLE*/

        /// <summary>
        /// Create a new client record.
        /// </summary>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="name">Client Name</param>
        /// <param name="status">Client Status</param>
        /// <returns><![CDATA[ (ClientVm Client, ClientKeyVm Key, bool IsSuccess, String Message) ]]></returns>
        public (ClientVm Client, ClientKeyVm Key, bool IsSuccess, String Message) CreateClient(int ownerId, string name, ClientKeyStatus status)
        {

            try
            {

                var dbCheck = ClientDataAccess.Client.Find(f => f.OwnerId == ownerId &&
                                                                f.Name.ToLower() == name.ToLower());
                if (dbCheck != null)
                    return (null, null, false, ResourceManagerMessages.Error.CLIENT_ADD_ALREADY_EXISTS);

                // Create the Client Resource
                var dbView = (new ClientVm() { Name = name }).ToEntityCreate(ownerId);
                dbView = ClientDataAccess.Client.Create(dbView);

                // Generate a new Client Key
                var dbKey = (new ClientKeyVm() { Status = status }).ToEntityCreate(dbView.Id);
                dbKey = ClientDataAccess.ClientKey.Create(dbKey);

                ClientDataAccess.Save();

                var client = new ClientVm(dbView);
                var key = new ClientKeyVm(dbKey);

                return (client, key, true, ResourceManagerMessages.Success.CLAIM_CREATED);

            }
            catch (DbEntityValidationException ex)
            {
#if(DEBUG)
                // for debuging entity framework
                foreach (var error in ex.EntityValidationErrors.SelectMany(valError => valError.ValidationErrors))
                    Console.WriteLine(error.ErrorMessage);
#endif
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create a new key for the already existing client
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="status">Client Key Status</param>
        /// <returns><![CDATA[ (ClientKeyVm key, bool IsSuccess, String Message) ]]></returns>
        public (ClientKeyVm key, bool IsSuccess, String Message) CreateKey(int id, ClientKeyStatus status)
        {
            try
            {
                var dbCheck = ClientDataAccess.Client.Find(f => f.Id == id);
                if (dbCheck == null)
                    return (null, false, ResourceManagerMessages.Error.CLIENT_NOT_FOUND);

                // Generate a new Client Key
                var dbKey = (new ClientKeyVm() { Status = status }).ToEntityCreate(id);
                dbKey = ClientDataAccess.ClientKey.Create(dbKey);

                ClientDataAccess.Save();

                var key = new ClientKeyVm(dbKey);

                return (key, true, ResourceManagerMessages.Success.CLAIM_KEY_CREATED);
            }
            catch (DbEntityValidationException ex)
            {
#if(DEBUG)
                // for debugging entity framework
                foreach (var error in ex.EntityValidationErrors.SelectMany(valError => valError.ValidationErrors))
                    Console.WriteLine(error.ErrorMessage);
#endif
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create a new resource access for the client.
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="resourceId">Resouce Id</param>
        /// <param name="resourceValue">Resource Value</param>
        /// <param name="status">Client Resource Access Status</param>
        /// <param name="claims">List of Claim Ids</param>
        /// <returns><![CDATA[ (ClientResourceAccessVm Resource, bool IsSuccess, String Message) ]]></returns>
        public (ClientResourceAccessVm Resource, bool IsSuccess, String Message) CreateResourceAccess(int clientId, int resourceId, int resourceValue, ClientResourceAccessStatus status, int[] claims = null)
        {

            try
            {
                // Check to make sure that the client does exists in the database.
                var dbClient = ClientDataAccess.Client.Find(f => f.Id == clientId);
                if (dbClient == null)
                    return (null, false, ResourceManagerMessages.Error.CLIENT_NOT_FOUND);

                // Check to make sure that the resource manager does exists in the database.
                var dbRes = ResourceDataAccess.ResourceManager.Find(f => f.Id == resourceId);
                if (dbRes == null)
                    return (null, false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

                // Check to make sure that the resource access with the same resource value does not exists.
                var dbResVal = ClientDataAccess.ClientResourceAccess.Find(f => f.ResourceId == resourceId && f.ResourceValue == resourceValue);
                if (dbResVal != null)
                    return (null, false, ResourceManagerMessages.Error.RESOURCE_ALREADY_EXISTS);


                /* --------------------------------------------------------
                 * If the caller passesa list of claim Ids
                 * check to see if all the claim ids exists in the database 
                 * before creating a new record in the database. 
                  --------------------------------------------------------- */
                if (claims != null && claims.Length != 0)
                {
                    var hasInvalidClaim = ResourceDataAccess.ResourceClaim.Any(f => f.ResourceId == resourceId && !claims.Contains(f.Id));
                    if (hasInvalidClaim)
                        return (null, false, ResourceManagerMessages.Error.CLAIM_UPDATE_NOT_FOUND);
                }

                var dbResAccess = (new ClientResourceAccessVm() { Status = status }).ToEntityCreate(clientId, resourceId, resourceValue);
                dbResAccess = ClientDataAccess.ClientResourceAccess.Create(dbResAccess);

                if (claims != null)
                {
                    foreach (var id in claims)
                    {
                        ClientDataAccess.ClientResourceAccessClaim.Create(
                                new ClientResourceAccessClaim()
                                {
                                    ResourceClaimId = id,
                                    ClientResourceAccessId = dbResAccess.Id,
                                    Access = ClientResourceClaimsAccess.Allow
                                }
                            );
                    }
                }

                ClientDataAccess.Save();

                var resAccess = new ClientResourceAccessVm(dbResAccess);
                return (resAccess, true, ResourceManagerMessages.Success.ACCESS_CREATED);
            }
            catch (DbEntityValidationException ex)
            {
#if(DEBUG)
                // for debuging entity framework
                foreach (var error in ex.EntityValidationErrors.SelectMany(valError => valError.ValidationErrors))
                    Console.WriteLine(error.ErrorMessage);
#endif
                throw;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Add new claim the an existing client resource access
        /// </summary>
        /// <param name="resourceId">Client Resource Acces Id</param>
        /// <param name="claimId">Resource Claim Id</param>
        /// <param name="access">Client Resource Claims Access</param>
        /// <returns><![CDATA[ (ClientResourceAccessClaimVm Claim, bool IsSuccess, string Message) ]]></returns>
        public (ClientResourceAccessClaimVm Claim, bool IsSuccess, string Message) AddClaim(int resourceId, int claimId, ClientResourceClaimsAccess access)
        {
            try
            {
                // check to make sure that the client resource does exists inside the client.
                var dbResAccess = ClientDataAccess.ClientResourceAccess.GetById(resourceId);
                if (dbResAccess == null)
                    return (null, false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

                var dbClaimCheck = ClientDataAccess.ClientResourceAccessClaim.Find(f => f.ClientResourceAccessId == resourceId && f.ResourceClaimId == claimId);
                if (dbClaimCheck != null)
                    return (null, false, ResourceManagerMessages.Error.CLAIM_ADD_ALREADY_EXISTS);

                var dbClaim = new ClientResourceAccessClaim()
                {
                    ResourceClaimId = claimId,
                    ClientResourceAccessId = dbResAccess.Id,
                    Access = access,
                };

                ClientDataAccess.ClientResourceAccessClaim.Create(dbClaim);
                ClientDataAccess.Save();

                return (new ClientResourceAccessClaimVm(dbClaim), true, ResourceManagerMessages.Success.CLAIM_CREATED);
            }
            catch (DbEntityValidationException ex)
            {
#if(DEBUG)
                // for debuging entity framework
                foreach (var error in ex.EntityValidationErrors.SelectMany(valError => valError.ValidationErrors))
                    Console.WriteLine(error.ErrorMessage);
#endif
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update the already existing key
        /// </summary>
        /// <param name="id">Client Key Id</param>
        /// <param name="status">Client Key Status</param>
        /// <returns><![CDATA[ (bool IsSuccess, String Message) ]]></returns>
        public (bool IsSuccess, String Message) UpdateKeyStatus(int id, ClientKeyStatus status)
        {

            try
            {
                // Check if the record exist in the database.
                var dbView = ClientDataAccess.ClientKey.Find(f => f.Id == id);
                if (dbView == null)
                    return (false, ResourceManagerMessages.Error.CLIENT_KEY_NOT_FOUND);

                // Update the existing record from the database.
                dbView.Status = status;
                dbView.UpdateDate = DateTime.UtcNow;

                ClientDataAccess.ClientKey.Update(dbView);
                ClientDataAccess.Save();

                return (true, ResourceManagerMessages.Success.CLAIM_KEY_UPDATED);

            }
            catch (DbEntityValidationException ex)
            {
#if(DEBUG)
                // for debuging entity framework
                foreach (var error in ex.EntityValidationErrors.SelectMany(valError => valError.ValidationErrors))
                    Console.WriteLine(error.ErrorMessage);
#endif
                throw;
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Update the already existing resource access claim
        /// </summary>
        /// <param name="id">Resource Access Claim Id</param>
        /// <param name="access">Client Resource Claims Access</param>
        /// <returns><![CDATA[ (bool IsSuccess, String Message) ]]></returns>
        public (bool IsSuccess, String Message) UpdateClaimAccess(int id, ClientResourceClaimsAccess access)
        {
            try
            {
                var dbClaim = ClientDataAccess.ClientResourceAccessClaim.GetById(id);
                if (dbClaim == null)
                    return (false, ResourceManagerMessages.Error.CLAIM_UPDATE_NOT_FOUND);

                dbClaim.Access = access;

                ClientDataAccess.ClientResourceAccessClaim.Update(dbClaim);
                ClientDataAccess.Save();

                return (true, ResourceManagerMessages.Success.CLAIM_UPDATED);
            }
            catch (DbEntityValidationException ex)
            {
#if(DEBUG)
                // for debuging entity framework
                foreach (var error in ex.EntityValidationErrors.SelectMany(valError => valError.ValidationErrors))
                    Console.WriteLine(error.ErrorMessage);
#endif
                throw;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a specific client out of the database.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns></returns>
        public (bool IsSuccess, String Message) DeleteClient(int id)
        {
            var dbView = ClientDataAccess.Client.GetById(id);

            if (dbView == null)
                return (false, ResourceManagerMessages.Error.CLIENT_NOT_FOUND);

            ClientDataAccess.Client.Delete(dbView);
            ClientDataAccess.Save();

            return (true, ResourceManagerMessages.Success.CLIENT_DELETED);
        }

        /// <summary>
        /// Delete a specific client key out of the database.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns></returns>
        public (bool IsSuccess, String Message) DeleteClientKey(int id)
        {
            var dbView = ClientDataAccess.ClientKey.GetById(id);

            if (dbView == null)
                return (false, ResourceManagerMessages.Error.CLIENT_KEY_NOT_FOUND);

            ClientDataAccess.ClientKey.Delete(dbView);
            ClientDataAccess.Save();

            return (true, ResourceManagerMessages.Success.CLIENT_KEY_DELETED);
        }

        /// <summary>
        /// Delete a specific resource access out of the database.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns></returns>
        public (bool IsSuccess, String Message) DeleteResourceAccess(int id)
        {
            var dbView = ClientDataAccess.ClientResourceAccess.GetById(id);

            if (dbView == null)
                return (false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

            ClientDataAccess.ClientResourceAccess.Delete(dbView);
            ClientDataAccess.Save();

            return (true, ResourceManagerMessages.Success.RESOURCE_DELETED);
        }

        /// <summary>
        /// Delete a specific resource access claim out of the database.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns></returns>
        public (bool IsSuccess, String Message) DeleteClaim(int id)
        {
            var dbView = ClientDataAccess.ClientResourceAccessClaim.GetById(id);

            if (dbView == null)
                return (false, ResourceManagerMessages.Error.CLAIM_NOT_FOUND);

            ClientDataAccess.ClientResourceAccessClaim.Delete(dbView);
            ClientDataAccess.Save();

            return (true, ResourceManagerMessages.Success.CLAIM_DELETED);
        }

        #endregion Set

    }
}
