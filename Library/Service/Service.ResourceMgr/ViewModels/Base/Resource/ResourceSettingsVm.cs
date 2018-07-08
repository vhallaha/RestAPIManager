using DataLayer.ResourceMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Resource.Enums;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ResourceSettingsVm
    {
        private int _id;

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResourceSettingsVm()
        { }

        internal ResourceSettingsVm(ResourceSettings view = null)
        {
            if (view == null)
                return;

            _id = view.Id;

            Status = view.Status;

        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return _id; } }

        /// <summary>
        /// Status
        /// </summary>
        public ResourceStatus Status { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Use to update a database record
        /// </summary>
        /// <param name="view">Resource Settings</param>
        /// <returns>ResourceSettings</returns>
        internal virtual ResourceSettings ToEntity(ResourceSettings view = null)
        {
            if (view == null)
                view = new ResourceSettings();

            view.Status = Status;

            return view;
        }

        /// <summary>
        /// Use to create a new database record
        /// </summary>
        /// <returns>ResourceSettings</returns>
        internal virtual ResourceSettings ToEntityCreate(Resource res)
        {
            var view = ToEntity();
            view.Resource = res;

            return view;
        }

        #endregion Methods

    }
}
