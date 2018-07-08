using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace Utilities
{
    /// <summary>
    /// Cache Manager
    /// </summary>
    public class CacheManager
    {
        #region Create

        /// <summary>
        /// Creates a new cache to the server.
        /// </summary>
        /// <param name="sName">Key</param>
        /// <param name="value">Value</param>
        /// <param name="bOverwrite">Overwrite Cache</param>
        public static void Create(string sName, object value, bool bOverwrite)
        {
            Create(sName, value, bOverwrite, DateTime.Now.AddMinutes(30));
        }

        /// <summary>
        /// Creates a new cache to the server.
        /// </summary>
        /// <param name="sName">Key</param>
        /// <param name="sValue">Value</param>
        /// <param name="bOverwrite">Overwrite Cache</param>
        /// <param name="tExpiration">Expiration Date</param>
        public static void Create(string sName, object value, bool bOverwrite, DateTime tExpiration)
        {
            // Check if the cache is already in the server.
            object oCache = HttpContext.Current.Cache[sName];

            // If the cache already exists
            // Check if the user wants to remove the existing cache.
            if (oCache != null && bOverwrite)
                HttpContext.Current.Cache.Remove(sName);

            HttpContext.Current.Cache.Add(
                                          sName,
                                          value,
                                          null,
                                          tExpiration,
                                          Cache.NoSlidingExpiration,
                                          CacheItemPriority.Default,
                                          null
                                         );
        }

        #endregion Create

        #region Get

        /// <summary>
        /// Gets the cache to the server.
        /// </summary>
        /// <param name="sName">Key</param>
        /// <returns>object</returns>
        public static object Get(string sName)
        {
            return HttpContext.Current.Cache.Get(sName);
        }

        /// <summary>
        /// Gets all the cache from the server.
        /// </summary>
        /// <returns><!--List<object>--></returns>
        public static List<object> Get()
        {
            List<object> lstCache = new List<object>();

            HttpContext oContext = HttpContext.Current;
            foreach (var cache in oContext.Cache)
            {
                lstCache.Add(cache);
            }

            return lstCache;
        }

        public static List<object> Filter(string filter)
        {
            List<object> lstCache = new List<object>();

            foreach (DictionaryEntry cache in HttpContext.Current.Cache)
            {
                if (cache.Key.ToString().Contains(filter))
                    lstCache.Add(cache.Value);
            }

            return lstCache;
        }

        #endregion Get

        #region Remove

        /// <summary>
        /// Removes all the cache from the server.
        /// </summary>
        public static void Remove()
        {
            HttpContext oContext = HttpContext.Current;
            foreach (DictionaryEntry cache in oContext.Cache)
            {
                RemoveSpecific(cache.Key.ToString());
            }
        }

        /// <summary>
        /// Removes all the cache from the server with filtering.
        /// </summary>
        /// <param name="sName">Filter</param>
        public static void Remove(string sName)
        {
            HttpContext oContext = HttpContext.Current;
            foreach (DictionaryEntry cache in oContext.Cache)
            {
                if (cache.Key.ToString().Contains(sName))
                    RemoveSpecific(cache.Key.ToString());
            }
        }

        /// <summary>
        /// Removes a specific cache from the server.
        /// </summary>
        /// <param name="sName">Key</param>
        public static void RemoveSpecific(string sName)
        {
            HttpContext.Current.Cache.Remove(sName);
        }

        #endregion Remove

        #region Check

        /// <summary>
        /// Check if the cache already exists on the server.
        /// </summary>
        /// <param name="sName">Key</param>
        /// <returns>bool</returns>
        public static bool Exists(string sName)
        {
            try
            {
                bool bExist = false;

                object oCache = Get(sName);

                if (oCache != null)
                    bExist = true;

                return bExist;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        #endregion Check
    }
}
