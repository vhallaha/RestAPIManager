using System.Web.Mvc;
using UI.Presentation.Plumbing;

namespace UI.Presentation.Areas.Dashboard.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        #region Get
        public ActionResult Index()
        {
            return View();
        }

        #endregion Get

    }
}