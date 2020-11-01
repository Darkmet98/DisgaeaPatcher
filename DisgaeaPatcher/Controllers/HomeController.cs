using System;
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

        public void OpenGame(string type)
        {
            Core.OpenGame(Convert.ToInt32(type));
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

        public ActionResult CheckInternet()
        {
            return Json(new { check = Core.Internet });
        }

        public ActionResult CheckUpdate()
        {
            var update = Core.CheckUpdate();
            
            return Json(new { check = update, msg = Core.OnlineVersion.Changelog });
        }

        public ActionResult CheckPiratedGame()
        {
            return Json(Core.GamePirated);
        }
    }
}
