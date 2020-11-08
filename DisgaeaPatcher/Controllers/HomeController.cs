using System;
using System.Linq;
using DisgaeaPatcher.Core;
using ElectronNET.API;
using ElectronNET.API.Entities;
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
            ElectronBootstrap();
           
        }

        private async void ElectronBootstrap()
        {
            var browserWindow = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 1260,
                Height = 700,
                Show = false,
                MinWidth = 1260,
                MinHeight = 700,
            });
            browserWindow.LoadURL($"http://localhost:{BridgeSettings.WebPort}/Credits");
            browserWindow.OnReadyToShow += () =>
            {
                browserWindow.Show();
                Electron.WindowManager.BrowserWindows.Last().RemoveMenu();
            };
        }

        public void OpenGame(string type)
        {
            Core.OpenGame(Convert.ToInt32(type));
        }

        public void OpenUrl(string url)
        {
            Electron.Shell.OpenExternalAsync(url);
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
