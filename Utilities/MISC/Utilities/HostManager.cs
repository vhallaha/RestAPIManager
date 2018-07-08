using System.Web;

namespace Utilities
{
    /// <summary>
    /// Host Manager
    /// </summary>
    public static class HostManager
    {
        /// <summary>
        /// Host Name
        /// </summary>
        public static string HostName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
    }
}