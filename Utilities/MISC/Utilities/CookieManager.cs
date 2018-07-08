using System;
using System.Web;

namespace Utilities
{
    /// <summary>
    /// Cookie Manager
    /// </summary>
    public class CookieManager
    {
        #region Create

        /// <summary>
        /// Creates a new .net cookie
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sValue"></param>
        /// <param name="bOverwrite"></param>
        public static void Create(string sName, string sValue, bool bOverwrite)
        {
            Create(sName, sValue, bOverwrite, DateTime.Now.AddMinutes(30));
        }

        /// <summary>
        /// Creates a new .net cookie
        /// </summary>
        /// <param name="sName">Name</param>
        /// <param name="sValue">Value</param>
        /// <param name="bOverwrite">Overwrite Cookie</param>
        /// <param name="tExpiration">Expiration Date</param>
        public static void Create(string sName, string sValue, bool bOverwrite, DateTime tExpiration)
        {
            object oCookie = HttpContext.Current.Response.Cookies[sName];

            if (oCookie != null && bOverwrite)
                HttpContext.Current.Response.Cookies[sName].Expires = DateTime.Now.AddDays(-1);

            HttpCookie oNewCookie = new HttpCookie(sName, sValue);
            oNewCookie.Expires = tExpiration;
            HttpContext.Current.Response.Cookies.Add(oNewCookie);
        }

        #endregion Create

        #region Get

        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="sName">Name</param>
        /// <returns>string</returns>
        public static string Get(string sName)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[sName] != null)
                    return HttpContext.Current.Request.Cookies[sName].Value;
                else
                    return "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Get

        #region Remove

        /// <summary>
        /// Removes the .net cookie
        /// </summary>
        /// <param name="sName"></param>
        public static void Remove(string sName)
        {
            try
            {
                HttpContext.Current.Response.Cookies[sName].Expires = DateTime.Now.AddDays(-1);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Remove

        #region Check

        /// <summary>
        /// Check if the cookie already exists on the client.
        /// </summary>
        /// <param name="sName">Name</param>
        /// <returns></returns>
        public static bool Exists(string sName)
        {
            try
            {
                bool bExists = false;

                HttpCookie oCookie = HttpContext.Current.Response.Cookies[sName];

                if (oCookie != null)
                    bExists = false;

                return bExists;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion Check
    }
}
