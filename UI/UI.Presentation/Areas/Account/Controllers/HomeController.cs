using Service.RestProject;
using Service.RestProject.ViewModels.Base;
using Service.RestProject.ViewModels.Forms;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UI.Presentation.Plumbing;
using Utilities.Member.Settings;

namespace UI.Presentation.Areas.Account.Controllers
{
    public class HomeController : BaseController
    {

        #region Get

        /// <summary>
        /// GET : Login View 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            // If user is already authenticated
            if (IsAuthenticated)
                return RedirectToAction("Index", "Home", new { area = "dashboard" });
             
            return View();
        }

        /// <summary>
        /// GET : Logout users.
        /// </summary>
        public void Logout()
        {
            FormsAuthentication.SignOut(); 
            Response.Redirect("/");
        }

        /// <summary>
        /// GET : Signup View
        /// </summary>
        /// <returns></returns>
        public ActionResult Signup()
        {
            // If user is already authenticated
            if (IsAuthenticated)
                return RedirectToAction("Index", "Home", new { area = "dashboard" });

            return View();
        }

        #endregion Get

        #region Post

        [HttpPost]
        public string Login(LoginVm form)
        {

            if (!ModelState.IsValid)
                return PostReturnVals.Failed;

            var resp = RestUnitOfWork.UserSvc.Get(form.Username, form.Password);
            if (resp.User == null)
                return PostReturnVals.Failed;

            if (resp.User != null)
            {
                CreateToken(resp.User);
                return PostReturnVals.Success;
            }

            return PostReturnVals.Failed;
        }
         
        [HttpPost]
        public string SignUp(SignupVm form)
        {

            if (!ModelState.IsValid)
                return PostReturnVals.Failed;

            var resp = RestUnitOfWork.UserSvc.Create(form);
            if (resp.User == null) 
                return resp.Message;

            if (resp.User != null)
            {
                CreateToken(resp.User);
                return PostReturnVals.Success;
            }

            if (resp.Message == MemberManagerMessages.Error.USERNAME_EXISTS)
                return MemberManagerMessages.Error.USERNAME_EXISTS;

            return PostReturnVals.Failed;
        }

        #endregion Post

        #region Methods

        /// <summary>
        /// Creates a Forms authentication cooke to authenticate user
        /// using the ASP.NET Forms Authentication.
        /// </summary>
        /// <param name="user">User View Model</param>
        public void CreateToken(UserVm user)
        { 
            FormsAuthenticationTicket ticket;
            string cookieString = string.Empty;
            HttpCookie cookie;

            ticket = new FormsAuthenticationTicket(1,
                                                   user.DisplayName,
                                                   DateTime.UtcNow,
                                                   DateTime.UtcNow.AddHours(RPSettings.CookieTimeoutMinute),
                                                   true,
                                                   user.ProviderKey);
            cookieString = FormsAuthentication.Encrypt(ticket);
            cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieString);

            cookie.Expires = ticket.Expiration;
            cookie.Path = FormsAuthentication.FormsCookiePath;

            Response.Cookies.Add(cookie);

        }

        #endregion Methods

    }
}