using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;

namespace DisgaeaPatcher.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        public void OpenInfo()
        {
            Electron.WindowManager.CreateWindowAsync(
                $"http://localhost:{BridgeSettings.WebPort}/Credits");
        }

    }
}
