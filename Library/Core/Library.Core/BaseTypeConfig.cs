using System;
using System.Data.Entity.ModelConfiguration;

namespace Library.Core
{
    /// <summary>
    /// Allows configuration to be performed for an entity type in a model. An EntityTypeConfiguration
    /// can be obtained via the Entity method on System.Data.Entity.DbModelBuilder or
    /// a custom type derived from EntityTypeConfiguration can be registered via the
    /// Configurations property on System.Data.Entity.DbModelBuilder.    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseTypeConfig<T> : EntityTypeConfiguration<T> where T : class
    {
        private string _tableName = String.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseTypeConfig()
        {
            Init();
        }

        private void Init()
        {
            SetupName();
            SetupKeys();
            SetupColumns();
        }

        /// <summary>
        /// Rename Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="prefix"></param>
        protected void OverrideTableName(string tableName, string prefix)
        {
            _tableName = prefix + tableName;
            SetupName();
        }

        #region Methods

        /// <summary>
        /// Setup Keys
        /// </summary>
        protected virtual void SetupKeys()
        {
            throw new NotImplementedException("You need to implement SetupKeys.");
        }

        /// <summary>
        /// Setup Column
        /// </summary>
        protected virtual void SetupColumns()
        {
            throw new NotImplementedException("You need to implement SetupColumns.");
        }

        /// <summary>
        /// Setup Name
        /// </summary>
        protected void SetupName()
        {
            if (String.IsNullOrEmpty(_tableName))
            {
                ToTable(typeof(T).Name);
            }
            else
            {
                ToTable(_tableName);
            }
        }

        #endregion Methods
    }
}
