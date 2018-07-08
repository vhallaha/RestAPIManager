using Service.ResourceMgr.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Utilities.Resource.Settings;
using Utilities.Shared;
using Utilities.Resource.Enums;

namespace Service.ResourceMgr.Service
{
    public class ResourceManagerSvc : ServiceBase
    {

        #region Ctor

        public ResourceManagerSvc(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Get

        /// <summary>
        /// Get a specific Resource Manager
        /// </summary>
        /// <param name="id">Resource Manager ID</param>
        /// <returns>ResourceVm</returns>
        public ResourceVm Get(int id)
        {
            var res = ResourceDataAccess.ResourceManager.GetById(id);

            if (res == null)
                return null;

            return new ResourceVm(res);
        }

        /// <summary>
        /// Get settings a specific resource
        /// </summary>
        /// <param name="id">Resource Id</param>
        /// <returns>ResourceSettingsVm</returns>
        public ResourceSettingsVm GetSettings(int id)
        {
            var settings = ResourceDataAccess.ResourceSettings.GetById(id);

            if (settings == null)
                return null;

            return new ResourceSettingsVm(settings);
        }

        /// <summary>
        /// Get a specific resource claim
        /// </summary>
        /// <param name="id">Resource Claim Id</param>
        /// <returns>ResourceClaimVm</returns>
        public ResourceClaimVm GetClaim(int id)
        {
            var claim = ResourceDataAccess.ResourceClaim.GetById(id);

            if (claim == null)
                return null;

            return new ResourceClaimVm(claim);
        }

        /// <summary>
        /// Gets all resource manager in the database.
        /// </summary>
        /// <returns><![CDATA[ IEnumerable<ResourceVm> ]]></returns>
        public IEnumerable<ResourceVm> List()
        {
            return (from res in ResourceDataAccess.ResourceManager.All().ToList()
                    select new ResourceVm(res));
        }

        /// <summary>
        /// Get a list of resource with the same type
        /// </summary>
        /// <param name="type">Resource Type</param>
        /// <returns><![CDATA[ IEnumerable<ResourceVm> ]]></returns>
        public IEnumerable<ResourceVm> GetByType(ResourceType type)
        {
            return (from res in ResourceDataAccess.ResourceManager.Filter(f => f.Type == type).ToList()
                    select new ResourceVm(res));
        }

        public IEnumerable<ResourceVm> GetByTypeAndStatus(ResourceType type, ResourceStatus status)
        {
            return (from res in ResourceDataAccess.ResourceManager.Filter(f => f.Type == type && f.Settings.Status == status).ToList()
                    select new ResourceVm(res));
        }

        /// <summary>
        /// Grabs all the claims of the resource manager
        /// </summary>
        /// <param name="id">Resource Manager Id</param>
        /// <returns><![CDATA[ IEnumerable<ResourceClaimVm> ]]></returns>
        public IEnumerable<ResourceClaimVm> GetClaims(int id)
        {
            return (from claim in ResourceDataAccess.ResourceClaim.Filter(f => f.ResourceId == id).ToList()
                    select new ResourceClaimVm(claim));
        }

        #endregion Get

        #region Set
        /*TODO : MAKE SURE THAT ALL THE TRANSACTION ON SET ARE LOGGED IN A TABLE*/
         
        /// <summary>
        /// Create a new Resource Manager
        /// </summary>
        /// <param name="name">Resource Name</param>
        /// <param name="type">Resource Type</param>
        /// <param name="settings">Settings</param>
        /// <returns><![CDATA[ (ResourceVm Resource, ResourceSettingsVm Settings) ]]></returns>
        public (ResourceVm Resource, ResourceSettingsVm Settings, bool IsSuccess, String Message) Create(string name, ResourceType type, ResourceSettingsVm settings)
        {
            try
            {
                // Check if the claim already exists
                var dbCheck = ResourceDataAccess.ResourceClaim.Find(f => f.ClaimName.ToLower() == name.ToLower());
                if (dbCheck != null)
                    return (null, null, false, ResourceManagerMessages.Error.RESOURCE_ALREADY_EXISTS);
                 
                // Create the Resource
                var dbView = (new ResourceVm() { Name = name }).ToEntityCreate(type);
                dbView = ResourceDataAccess.ResourceManager.Create(dbView);

                // Create the Resource Settings
                var dbSettings = settings.ToEntityCreate(dbView);
                dbSettings = ResourceDataAccess.ResourceSettings.Create(dbSettings);

                ResourceDataAccess.Save();

                var res = new ResourceVm(dbView);
                settings = new ResourceSettingsVm(dbSettings);

                return (res, settings, true, ResourceManagerMessages.Success.RESOURCE_CREATED);

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
        /// Add new claims to the resource manager
        /// </summary>
        /// <param name="id">Resource Id</param>
        /// <param name="name">Claim Name</param>
        /// <returns><![CDATA[ (ResourceClaimVm Claim, bool IsSuccess,String Message) ]]></returns>
        public (ResourceClaimVm Claim, bool IsSuccess,String Message) AddClaim(int id, string name)
        {
            try
            {

                // Check if the claim already exists
                var dbCheck = ResourceDataAccess.ResourceClaim.Find(f => f.ResourceId == id && f.ClaimName.ToLower() == name.ToLower());
                if (dbCheck != null)
                    return (null, false, ResourceManagerMessages.Error.CLAIM_ADD_ALREADY_EXISTS);


                var dbClaim = (new ResourceClaimVm() { ClaimName = name }).ToEntityCreate(id);
                dbClaim = ResourceDataAccess.ResourceClaim.Create(dbClaim);

                ResourceDataAccess.Save();

                return (new ResourceClaimVm(dbClaim), true, ResourceManagerMessages.Success.CLAIM_CREATED);
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
        }

        /// <summary>
        /// Update an existing resource
        /// </summary>
        /// <param name="resource">ResourceVm</param>
        /// <returns><![CDATA[ (ResourceVm Resource, bool IsSuccess, String Message) ]]></returns>
        public (ResourceVm Resource, bool IsSuccess, String Message) UpdateResource(ResourceVm resource)
        {
            // Check and make sure that the data exisits.
            var dbRes = ResourceDataAccess.ResourceManager.GetById(resource.Id);

            if (dbRes == null)
                return (null, false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

            // Update the resource manager.
            dbRes = resource.ToEntity(dbRes);
            ResourceDataAccess.ResourceManager.Update(dbRes);
            ResourceDataAccess.Save();

            resource = new ResourceVm(dbRes);
            return (resource, true, ResourceManagerMessages.Success.RESOUCE_UPDATED);
        }

        /// <summary>
        /// Update the existing resource settings
        /// </summary>
        /// <param name="settings">ResourceSettingsVm</param>
        /// <returns><![CDATA[ (ResourceSettingsVm Settings, bool IsSuccess, String Message) ]]></returns>
        public (ResourceSettingsVm Settings, bool IsSuccess, String Message) UpdateSettings(ResourceSettingsVm settings)
        {
            // Check and make sure that the data exisits.
            var dbSettings = ResourceDataAccess.ResourceSettings.GetById(settings.Id);

            if (dbSettings == null)
                return (null, false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

            // Update the resource manager.
            dbSettings = settings.ToEntity(dbSettings);
            ResourceDataAccess.ResourceSettings.Update(dbSettings);
            ResourceDataAccess.Save();

            settings = new ResourceSettingsVm(dbSettings);
            return (settings, true, ResourceManagerMessages.Success.RESOUCE_UPDATED);
        }

        /// <summary>
        /// Update an existing resource claim
        /// </summary>
        /// <param name="claim">Resource Claim</param>
        /// <returns><![CDATA[ (ResourceClaimVm Claim, bool IsSuccess, String Message) ]]></returns>
        public (ResourceClaimVm Claim, bool IsSuccess, String Message) UpdateClaim(ResourceClaimVm claim)
        {
            // Check and make sure that the data exisits.
            var dbClaim = ResourceDataAccess.ResourceClaim.GetById(claim.Id);

            if (dbClaim == null)
                return (null, false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

            // Update the resource manager.
            dbClaim = claim.ToEntity(dbClaim);
            ResourceDataAccess.ResourceClaim.Update(dbClaim);
            ResourceDataAccess.Save();

            claim = new ResourceClaimVm(dbClaim);
            return (claim, true, ResourceManagerMessages.Success.RESOUCE_UPDATED);
        }

        /// <summary>
        /// Delete an existing resource manager
        /// </summary>
        /// <param name="id">Resource Manager Id</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) DeleteManager(int id)
        {
            var dbResource = ResourceDataAccess.ResourceManager.GetById(id);

            if (dbResource == null)
                return (false, ResourceManagerMessages.Error.RESOURCE_NOT_FOUND);

            ResourceDataAccess.ResourceManager.Delete(dbResource);
            ResourceDataAccess.Save();

            return (true, ResourceManagerMessages.Success.RESOURCE_DELETED);
        }

        /// <summary>
        /// Delete an existing resource claim
        /// </summary>
        /// <param name="id">Claim Id</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) DeleteClaim(int id)
        {
            var dbClaim = ResourceDataAccess.ResourceClaim.GetById(id);

            if (dbClaim == null)
                return (false, ResourceManagerMessages.Error.CLAIM_UPDATE_NOT_FOUND);

            ResourceDataAccess.ResourceClaim.Delete(dbClaim);
            ResourceDataAccess.Save();

            return (true, ResourceManagerMessages.Success.CLAIM_DELETED);
        }

        #endregion Set

    }
}
