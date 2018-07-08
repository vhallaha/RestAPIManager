using System.Web.Mvc;

namespace UI.Presentation.Areas.Account.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        // GET: Account/Security
        public ActionResult Index()
        {
            return View();
        }
    }
}