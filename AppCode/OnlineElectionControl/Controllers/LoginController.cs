using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Controllers
{
    public class LoginController : OECBaseController
    {
        public IActionResult Index()
        {
            if (Current.UserIsLoggedIn) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult AttemptLogin(string pUsername, string pPassword)
        {

            return RedirectToAction("Index", "Home");
        }
    }
}
