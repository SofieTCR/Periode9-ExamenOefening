using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Controllers
{
    public class VoteController : Controller
    {
        public IActionResult Index(int? pId)
        {
            if (pId == null || !Current.UserIsLoggedIn) RedirectToAction("Index", "Home");
            try
            {
                var tmpElection = Election.GetList(pElectionIds: new List<int> { (int) pId! })[0];
                if (Current.UserCanVote(pElectionId: (int) pId!)) RedirectToAction("Index", "Home");
                return View(new VoteModel(pElection: tmpElection));
            }
            catch
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { $"Verkiezing met Id {pId} niet gevonden." };
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
