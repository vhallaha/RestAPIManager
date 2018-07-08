using Service.RestProject.ViewModels.Base;
using Service.RestProject.ViewModels.Forms;
using System.Web.Mvc;
using System.Web.Security;
using UI.Presentation.Plumbing;

namespace UI.Presentation.Areas.Account.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {

        #region Get

        public ActionResult Index()
        {
            var user = RestUnitOfWork.UserSvc.GetByProviderKey(CurrentUserProviderToken);
            return View(user.User);
        }

        #endregion  Get

        #region Post

        [HttpPost]
        public string UpdateProfile(UserVm view)
        {

            if (!ModelState.IsValid)
                return PostReturnVals.Failed;

            view.ProviderKey = CurrentUserProviderToken;
            view.Username = CurrentUser;
            var resp = RestUnitOfWork.UserSvc.Update(view);

            if (resp.IsSuccess) 
                return PostReturnVals.Success; 
            else
                return resp.Message;
        }

        [HttpPost]
        public string ChangePassword(ChangePasswordVm view)
        {
            if (!ModelState.IsValid)
                return PostReturnVals.Failed;

            view.ProviderKey = CurrentUserProviderToken;
            var resp = RestUnitOfWork.UserSvc.ChangePassword(view.ProviderKey, view);

            if (resp.IsSuccess)
            {
                FormsAuthentication.SignOut();
                return PostReturnVals.Success;
            }
            else
                return resp.Message;
        }

        #endregion Post
    }
}