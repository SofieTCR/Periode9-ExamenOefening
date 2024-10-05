using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;
using System.Diagnostics;

namespace OnlineElectionControl.Controllers
{
    public class HomeController : OECBaseController
    {
        public IActionResult Index()
        {
            return View(new HomeModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedInUserId");
            Current.LoggedInUserId = null;

            TempData["Vml"] = new List<string> { "You have been logged out succesfully!" };

            return RedirectToAction("Index");
        }
    }
}
