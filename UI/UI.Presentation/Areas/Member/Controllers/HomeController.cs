using Service.RestProject.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UI.Presentation.Plumbing;

namespace UI.Presentation.Areas.Member.Controllers
{
    /* DEV NOTE : Member Manager Dashboards
    -------------------------------------------------------- */
    public class HomeController : BaseController
    {

        #region Get

        public ActionResult ManagerDashboard(string id)
        {
            /* DEV NOTE: Grab the first user owned manager if the 
             * id is not supplied, but if the id has value
             * then grab that specific manager to be display 
             * in the dashboard.
            --------------------------------------*/
            var resp = RestUnitOfWork.UserMgrSvc.GetList(CurrentUserProviderToken);
            UserManagerVm currentManager = null;

            if (!string.IsNullOrWhiteSpace(id))
                currentManager = resp.Managers?.FirstOrDefault(f => f.Identity == id);
            else
                currentManager = resp.Managers?.FirstOrDefault();

            var model = new Tuple<IEnumerable<UserManagerVm>, UserManagerVm>(resp.Managers, currentManager);
            return View("ManagerDashboard", model);
        }

        #endregion Get

    }
}