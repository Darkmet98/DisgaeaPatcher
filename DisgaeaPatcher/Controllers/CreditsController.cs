
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisgaeaPatcher.Controllers
{
    public class CreditsController : Controller
    {
        // GET: CreditsController
        public string Version { get; set; }
        public ActionResult Index()
        {
            Version = "v" + (HomeController.Core.LocalVersion != null
                ? HomeController.Core.LocalVersion.PatchVersion
                : "1.0.0");
            return View(this);
        }
    }
}
