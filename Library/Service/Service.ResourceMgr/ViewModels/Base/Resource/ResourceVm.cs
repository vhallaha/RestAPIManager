using DataLayer.ResourceMgr.Models;
using System;
using Utilities.Shared;

namespace Service.ResourceMgr.ViewModels.Base
{
    public class ResourceVm
    {

        #region Ctor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResourceVm()
        { }

        internal ResourceVm(Resource view = null)
        {
            if (view == null)
                return;

            _id = view.Id;
            _type = view.Type;
            _createDate = view.CreateDate;
            _updateDate = view.UpdateDate;

            Name = view.Name;
        }

        #endregion Ctor

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return _id; } }
        private int _id;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public ResourceType Type { get { return _type; } }
        private ResourceType _type;

        /// <summary>
        /// Create Date
        /// </summary>
        public DateTime CreateDate { get { return _createDate; } }
        private DateTime _createDate;

        /// <summary>
        /// Update Date
        /// </summary>
        public DateTime? UpdateDate { get { return _updateDate; } }
        private DateTime? _updateDate;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Use to update database record.
        /// </summary>
        /// <param name="view">Resource</param>
        /// <returns>Resource</returns>
        internal virtual Resource ToEntity(Resource view = null)
        {
            if (view == null)
                view = new Resource();

            view.Name = Name;

            if (_createDate != new DateTime())
                view.UpdateDate = DateTime.UtcNow;

            return view;
        }

        /// <summary>
        /// Use to create database record
        /// </summary>
        /// <param name="type">Resource Type</param>
        /// <returns>Resource</returns>
        internal virtual Resource ToEntityCreate(ResourceType type)
        {
            var view = ToEntity();

            view.Type = type;
            view.CreateDate = DateTime.UtcNow;

            return view;
        }

        #endregion Methods

    }
}
