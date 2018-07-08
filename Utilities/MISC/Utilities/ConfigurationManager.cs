using System;

namespace Utilities
{
    /// <summary>
    /// Configuration Manager
    /// </summary>
    public class ConfigurationManager
    {
        public ConfigurationManager()
        {
            // DEFAULT CONSTRUCTOR
        }

        #region Get

        /// <summary>
        /// Gets the value of the configuration key
        /// </summary>
        /// <param name="sName">Key</param>
        /// <returns>string</returns>
        public static string Get(string sName)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[sName];
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the value of the configuration key
        /// </summary>
        /// <param name="sName">Key</param>
        /// <param name="sDefault">Default</param>
        /// <returns>string</returns>
        public static string Get(string sName, string sDefault)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[sName];
            }
            catch
            {
                return sDefault;
            }
        }

        /// <summary>
        /// Gets the value of the configuration key
        /// </summary>
        /// <param name="sName">Key</param>
        /// <param name="bCache">Allow Caching</param>
        /// <returns>string</returns>
        public static string Get(string sName, bool bCache)
        {
            try
            {
                string sValue = String.Empty;
                if (bCache)
                {
                    if (CacheManager.Exists(sName))
                    {
                        sValue = CacheManager.Get(sName).ToString();
                    }
                    else
                    {
                        sValue = Get(sName);
                        CacheManager.Create(sName, sValue, true);
                    }
                }
                else
                    sValue = System.Configuration.ConfigurationManager.AppSettings[sName];

                return sValue;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        #endregion Get
    }
}
