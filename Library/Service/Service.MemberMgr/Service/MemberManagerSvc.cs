using Service.MemberMgr.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Utilities.Member.Settings;

namespace Service.MemberMgr.Service
{
    public class MemberManagerSvc : ServiceBase
    {

        #region Ctor

        public MemberManagerSvc(string connStr)
            : base(connStr)
        {

        }

        #endregion Ctor

        #region Get

        /// <summary>
        /// Get Manager by Manager Id
        /// </summary>
        /// <param name="id">Manager Id</param>
        /// <returns>MemberManagerVm</returns>
        public MemberManagerVm Get(int id)
        {
            var manager = DataAccess.MemberManager.GetById(id);

            if (manager == null)
                return null;

            return new MemberManagerVm(manager);
        }

        /// <summary>
        /// Get Manager by Manager Identity Key
        /// </summary>
        /// <param name="key">Manager Identity</param>
        /// <returns>MemberManagerVm</returns>
        public MemberManagerVm Get(string identity)
        {
            var manager = DataAccess.MemberManager.Find(f => f.Identity == identity);

            if (manager == null)
                return null;

            return new MemberManagerVm(manager);
        }

        /// <summary>
        /// Get Manager by Manager Identity Key
        /// </summary>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="identity">Identity</param>
        /// <returns></returns>
        public MemberManagerVm Get(int ownerId, string identity)
        {
            var manager = DataAccess.MemberManager.Find(f => f.OwnerId == ownerId && f.Identity == identity);

            if (manager == null)
                return null;

            return new MemberManagerVm(manager);
        }

        /// <summary>
        /// List of Manager
        /// </summary>
        /// <param name="total">Total Page</param>
        /// <param name="index">Page</param>
        /// <param name="size">Display</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> List(out int total, int index = 0, int size = 50)
        {
            return from manager in DataAccess.MemberManager.Get(null, f => f.OrderByDescending(o => o.CreateDate), out total, index: index, size: size).ToList()
                   select new MemberManagerVm(manager);
        }

        /// <summary>
        /// Search Manager by User Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> FindByUserId(int id)
        {
            return from manager in DataAccess.MemberManager.Filter(f => f.OwnerId == id).ToList()
                   select new MemberManagerVm(manager);
        }

        /// <summary>
        /// Search Manager by User Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="total">Total Page</param>
        /// <param name="index">Page</param>
        /// <param name="size">Display</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> FindByUserId(int id, out int total, int index = 0, int size = 50)
        {
            return from manager in DataAccess.MemberManager.Get(f => f.OwnerId == id, f => f.OrderByDescending(o => o.CreateDate), out total, index: index, size: size).ToList()
                   select new MemberManagerVm(manager);
        }

        /// <summary>
        /// Search Manager by manager name
        /// </summary>
        /// <param name="name">Manager Name</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> FindByName(string name)
        {
            name = name.ToLower();
            return from manager in DataAccess.MemberManager.Filter(f => f.Name.ToLower().Contains(name)).ToList()
                   select new MemberManagerVm(manager);
        }

        /// <summary>
        /// Search Manager by Manager name
        /// </summary>
        /// <param name="name">Manager Name</param>
        /// <param name="total">Total Page</param>
        /// <param name="index">Page</param>
        /// <param name="size">Display</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> FindByName(string name, out int total, int index = 0, int size = 50)
        {
            name = name.ToLower();
            return from manager in DataAccess.MemberManager.Get(f => f.Name.ToLower().Contains(name), f => f.OrderByDescending(o => o.CreateDate), out total, index: index, size: size).ToList()
                   select new MemberManagerVm(manager);
        }

        /// <summary>
        /// Search Manager by Manager Name and User Id
        /// </summary>
        /// <param name="name">Manager Name</param>
        /// <param name="id">User Id</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> FindByName(string name, int id)
        {
            name = name.ToLower();
            return from manager in DataAccess.MemberManager.Filter(f => f.Name.ToLower().Contains(name) && f.OwnerId == id).ToList()
                   select new MemberManagerVm(manager);
        }

        /// <summary>
        /// Search Manager by Manager Name and User Id
        /// </summary>
        /// <param name="name">Manager Name</param>
        /// <param name="id">User Id</param>
        /// <param name="total">Total Page</param>
        /// <param name="index">Page</param>
        /// <param name="size">Display</param>
        /// <returns><![CDATA[ IEnumerable<MemberManagerVm> ]]></returns>
        public IEnumerable<MemberManagerVm> FindByName(string name, int id, out int total, int index = 0, int size = 50)
        {
            name = name.ToLower();
            return from manager in DataAccess.MemberManager.Get(f => f.Name.ToLower().Contains(name) && f.OwnerId == id, f => f.OrderByDescending(o => o.CreateDate), out total, index: index, size: size).ToList()
                   select new MemberManagerVm(manager);
        }

        #endregion Get

        #region Set

        /// <summary>
        /// Create a new Manager
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="manager">Manager</param>
        /// <param name="settings">Settings</param> 
        /// <returns><![CDATA[ (MemberManagerVm Application, MemberManagerSettingsVm Settings) ]]></returns>
        public (MemberManagerVm Manager, MemberManagerSettingsVm Settings, bool IsSuccess, string Message) Create(int userId, MemberManagerVm manager, MemberManagerSettingsVm settings)
        {
            try
            {

                // Check if the Manager Name is already been used by the user.
                var checkData = DataAccess.MemberManager.Any(f => f.Name.ToLower().Trim() == manager.Name.ToLower().Trim()
                                                               && f.OwnerId == userId);

                if (checkData)
                    return (null, null, false, MemberManagerMessages.Error.MANAGER_ALREADY_EXISTS);
                 
                // Create the Application 
                var dbView = manager.ToEntityCreate(userId);
                dbView = DataAccess.MemberManager.Create(dbView);

                // Create the Application Properties
                var dbSettings = settings.ToEntityCreate(dbView);
                dbSettings = DataAccess.MemberManagerSettings.Create(dbSettings);
                 
                DataAccess.Save();

                manager = new MemberManagerVm(dbView);
                settings = new MemberManagerSettingsVm(dbSettings); 

                return (manager, settings, true, MemberManagerMessages.Success.MANAGER_CREATED);
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
        /// Update an existing Manager
        /// </summary>
        /// <param name="manager">MemberManagerVm</param>
        /// <returns><![CDATA[ (MemberManagerVm Application, bool IsSuccess, string Message) ]]></returns>
        public (MemberManagerVm Manager, bool IsSuccess, string Message) UpdateApplication(MemberManagerVm manager)
        {
            var dbManager = DataAccess.MemberManager.GetById(manager.Id);

            if (dbManager == null)
                return (manager, false, MemberManagerMessages.Error.MANAGER_UPDATE);

            dbManager = manager.ToEntity(dbManager);

            DataAccess.MemberManager.Update(dbManager);
            DataAccess.Save();

            manager = new MemberManagerVm(dbManager);
            return (manager, true, MemberManagerMessages.Success.MANAGER_UPDATE);
        }

        /// <summary>
        /// Update the settings of an existing Application
        /// </summary>
        /// <param name="settings">MemberManagerSettingsVm</param>
        /// <returns><![CDATA[ (MemberManagerSettingsVm Settings, bool IsSuccess, string Message) ]]></returns>
        public (MemberManagerSettingsVm Settings, bool IsSuccess, string Message) UpdateSettings(MemberManagerSettingsVm settings)
        {
            var dbSettings = DataAccess.MemberManagerSettings.GetById(settings.Id);

            if (dbSettings == null)
                return (settings, false, MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            dbSettings = settings.ToEntity(dbSettings);

            DataAccess.MemberManagerSettings.Update(dbSettings);
            DataAccess.Save();

            settings = new MemberManagerSettingsVm(dbSettings);
            return (settings, true, MemberManagerMessages.Success.MANAGER_SETTINGS_UPDATE);
        }
         
        /// <summary>
        /// Delete an existing Application
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><![CDATA[ Tuple<bool, string> ]]></returns>
        public (bool IsSuccess, string Message) Delete(int id)
        {
            var dbMember = DataAccess.MemberManager.GetById(id);

            if (dbMember == null)
                return (false, MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            DataAccess.MemberManager.Delete(dbMember);
            DataAccess.Save();

            return (true, MemberManagerMessages.Success.MANAGER_SETTINGS_UPDATE);
        }

        /// <summary>
        /// Delete an existing Application
        /// </summary>
        /// <param name="identity">Application Identity</param>
        /// <returns><![CDATA[ (bool IsSuccess, string Message) ]]></returns>
        public (bool IsSuccess, string Message) Delete(string identity)
        {
            var dbManager = DataAccess.MemberManager.Find(f => f.Identity == identity);

            if (dbManager == null)
                return (false, MemberManagerMessages.Error.MANAGER_DOES_NOT_EXISTS);

            DataAccess.MemberManager.Delete(dbManager);
            DataAccess.Save();

            return (true, MemberManagerMessages.Success.MANAGER_SETTINGS_UPDATE);
        }

        #endregion Set

    }
}
