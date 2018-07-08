using Service.RestProject.ViewModels.Base;
using System.Web.Mvc;
using UI.Presentation.Plumbing;

namespace UI.Presentation.Areas.Member.Controllers
{
    public class ManagerController : BaseController
    {


        #region Get
        
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Update(string id)
        {
            var resp = RestUnitOfWork.UserMgrSvc.Get(CurrentUserProviderToken, id);
            if (resp.Manager == null)
                return RedirectToAction("ManagerDashboard", "Home");

            return View("Update", resp.Manager);
        }

        #endregion Get

        #region Post

        [HttpPost]
        public string Create(UserManagerVm data)
        {
            if (!ModelState.IsValid)
                return PostReturnVals.Failed;

            var resp = RestUnitOfWork.UserMgrSvc.Create(CurrentUserProviderToken, data);

            if (resp.IsSuccess)
                return PostReturnVals.Success;

            return resp.Message;
        }

        [HttpPut]
        public string Update(UserManagerVm data)
        {
            if (!ModelState.IsValid)
                return PostReturnVals.Failed;
             
            var resp = RestUnitOfWork.UserMgrSvc.Update(CurrentUserProviderToken, data);

            if (resp.IsSuccess)
                return PostReturnVals.Success;

            return resp.Message;
        }


        #endregion Post

    }
}