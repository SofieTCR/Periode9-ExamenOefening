using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Controllers
{
    public class LoginController : OECBaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (Current.UserIsLoggedIn) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (Current.UserIsLoggedIn) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult AttemptLogin(string pUsername, string pPassword)
        {
            if (Current.UserIsLoggedIn) return RedirectToAction("Index", "Home");
            TempData["pUsername"] = pUsername;
            var tmpQuery = "SELECT Id FROM `user` WHERE Username = @pUsername";
            var tmpParameters = new Dictionary<string, object> { { "@pUsername", pUsername } };

            var tmpResult = Database.ExecuteQuery(pQuery: tmpQuery, pParameters: tmpParameters);

            if (tmpResult.Count != 1)
            {
                TempData["Vml"] = new string[] { "Onbekende gebruikersnaam!" };
                return RedirectToAction("Index");
            }

            var tmpUser = new User(pId: (int) tmpResult[0]["Id"]);

            if (!tmpUser.VerifyPassword(pPassword))
            {
                TempData["Vml"] = new string[] { "Onjuist Wachtwoord!" };
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetInt32("LoggedInUserId", (int) tmpUser.UserId!);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult AttemptRegister(string pUsername
                                           , string pPassword
                                           , string pFirstName
                                           , string pLastName
                                           , DateTime pBirthdate
                                           , string pCity
                                           , string pEmail)
        {
            if (Current.UserIsLoggedIn) return RedirectToAction("Index", "Home");
            TempData["pUsername"] = pUsername;
            TempData["pFirstName"] = pFirstName;
            TempData["pLastName"] = pLastName;
            TempData["pBirthdate"] = pBirthdate;
            TempData["pCity"] = pCity;
            TempData["pEmail"] = pEmail;

            var tmpUser = new User(pUsername: pUsername
                                 , pFirstName: pFirstName
                                 , pLastName: pLastName
                                 , pBirthdate: pBirthdate
                                 , pCity: pCity
                                 , pEmail: pEmail);

            if (!tmpUser.SetPassword(pPassword))
            {
                TempData["Vml"] = tmpUser.Vml;
                return RedirectToAction("Register");
            }

            if (tmpUser.ValidateObject()) tmpUser.Save();
            else
            {
                TempData["Vml"] = tmpUser.Vml;
                return RedirectToAction("Register");
            }

            HttpContext.Session.SetInt32("LoggedInUserId", (int) tmpUser.UserId!);

            return RedirectToAction("Index", "Home");
        }
    }
}