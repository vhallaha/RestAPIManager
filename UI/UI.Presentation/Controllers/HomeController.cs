using System.Web.Mvc;
using UI.Presentation.Plumbing;

namespace UI.Presentation.Controllers
{
    public class HomeController : BaseController
    {
         
        public ActionResult Index()
        {
            return View();
        }
		
    }
}