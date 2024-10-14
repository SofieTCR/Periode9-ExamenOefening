using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Controllers
{
    public class PartyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewParty(int? pId)
        {
            if (pId == null) return RedirectToAction("Index");
            try
            {
                return View(new PartyModel(pId: (int) pId));
            }
            catch(Exception ex)
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { $"Partij met Id {pId} niet gevonden." };
                return RedirectToAction("Index");
            }
            
        }

        public IActionResult CreateParty()
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsGovernment) return RedirectToAction("Index");
            return View(); 
        }

        [HttpPost]
        public IActionResult AttemptCreateParty(string pPartyName
                                              , string? pPartyDescription
                                              , string? pPartyPositions
                                              , string pPartyLogoLink
                                              , int pPartyLeader_UserId)
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsGovernment) return RedirectToAction("Index");
            TempData["pPartyName"] = pPartyName;
            TempData["pPartyDescription"] = pPartyDescription;
            TempData["pPartyPositions"] = pPartyPositions;
            TempData["pPartyLogoLink"] = pPartyLogoLink;
            TempData["pPartyLeader_UserId"] = pPartyLeader_UserId;

            var tmpParty = new Party(pName: pPartyName
                                   , pDescription: pPartyDescription
                                   , pPositions: pPartyPositions
                                   , pLogoLink: pPartyLogoLink
                                   , pLeader_UserId: pPartyLeader_UserId);

            if (tmpParty.ValidateObject()) tmpParty.Save();
            else
            {
                TempData["Vml"] = tmpParty.Vml;
                return RedirectToAction("Index");
            }

            TempData.Clear();
            TempData["Vml"] = new string[] { $"De partij \"{tmpParty.Name}\" is aangemaakt." };

            return View("CreateParty");
        }
    }
}
