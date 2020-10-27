using DisgaeaPatcher.Core;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;

namespace DisgaeaPatcher.Controllers
{
    public class HomeController : Controller
    {
        public static Main Core;

        // GET: HomeController
        public ActionResult Index()
        {
            Core = new Main();
            return View(this);
        }

        public void OpenInfo()
        {
            Electron.WindowManager.CreateWindowAsync(
                $"http://localhost:{BridgeSettings.WebPort}/Credits");
        }

        public void OpenGame()
        {
            Core.OpenGame(1);
        }

        public ActionResult CheckGame()
        {
            var result = Core.CheckGame();
            return Json(new {check = result.Item1, msg = result.Item2});
        }

        public ActionResult PatchGame()
        {
            var result = Core.PatchGame();
            return Json(new { check = result.Item1, msg = result.Item2 });
        }

        public ActionResult CheckPiratedGame()
        {
            return Json(Core.GamePirated);
        }
    }
}
