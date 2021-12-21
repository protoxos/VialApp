using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VialApp.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
