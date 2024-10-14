using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;
using System.Diagnostics;

namespace OnlineElectionControl.Controllers
{
    public class CreateElectionController : OECBaseController
    {
        public IActionResult Index()
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsGovernment) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult AttemptCreateElection(string pElectionName
                                                 , string? pElectionDescription
                                                 , DateTime pElectionDate)
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsGovernment) return RedirectToAction("Index", "Home");
            TempData["pElectionName"] = pElectionName;
            TempData["pElectionDescription"] = pElectionDescription;
            TempData["pElectionDate"] = pElectionDate;


            var tmpElection = new Election(pName : pElectionName
                                          ,pDate : pElectionDate
                                          ,pDescription : pElectionDescription);

            if (tmpElection.ValidateObject()) tmpElection.Save();
            else 
            { 
                TempData["Vml"] = tmpElection.Vml;
                return RedirectToAction("Index");
            }

            TempData.Clear();
            TempData["Vml"] = new List<string> { $"De verkiezing \"{tmpElection.Name}\" is aangemaakt." };
            return RedirectToAction("Index");
        }
    }
}
;