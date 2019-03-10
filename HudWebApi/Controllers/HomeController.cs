using System.Web.Mvc;

namespace Hud.Application.Service.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
