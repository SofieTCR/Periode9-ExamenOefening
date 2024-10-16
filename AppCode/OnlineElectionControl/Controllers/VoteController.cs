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
                if (tmpElection.Status != ElectionStatus.InProgress || !Current.UserCanVote(pElectionId: (int) pId!)) RedirectToAction("Index", "Home");
                return View(new VoteModel(pElection: tmpElection));
            }
            catch
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { $"Verkiezing met Id {pId} niet gevonden." };
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult AttemptSaveVote(int? pElectionId, int? pId)
        {
            if (pElectionId == null || pId == null || !Current.UserIsLoggedIn) RedirectToAction("Index", "Home");
            try
            {
                var tmpElection = Election.GetList(pElectionIds: new List<int> { (int) pElectionId! })[0];
                if (tmpElection.Status != ElectionStatus.InProgress || !Current.UserCanVote(pElectionId: (int) pId!)) RedirectToAction("Index", "Home");

                var tmpVote = new Vote(pVoter_UserId: (int) Current.LoggedInUserId!
                                     , pVoted_ElectionId: (int) pElectionId
                                     , pElectedMember_UserId: (int) pId!);
                if (tmpVote.ValidateObject()) tmpVote.Save();
                else
                {
                    TempData["Vml"] = tmpVote.Vml;
                    return RedirectToAction("Index");
                }

                TempData.Clear();
                TempData["Vml"] = new string[] { "Uw stem is geregistreerd!" };
                return RedirectToAction("Index", "Home");
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
