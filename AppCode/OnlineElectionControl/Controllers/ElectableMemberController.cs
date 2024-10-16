using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Controllers
{
    public class ElectableMemberController : Controller
    {
        public IActionResult Index(int? pId)
        {
            if (pId == null || !Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsPartyLeader) return RedirectToAction("Index", "Home");
            try
            {
                var tmpElection = new Election(pId: (int) pId);
                if (tmpElection.Status != ElectionStatus.Scheduled)
                {
                    TempData.Clear();
                    TempData["Vml"] = new string[] { $"Verkiesbare leden voor verkiezing \"{tmpElection.Name}\" mogen niet meer gemuteerd worden!." };
                    return RedirectToAction("Index", "Home");
                }

                return View(new ElectableMemberModel(pPartyId: (int) Current.LoggedInUser!.LeadingParty_PartyId!
                                                   , pElection: tmpElection));
            }
            catch
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { $"Verkiezing met Id {pId} niet gevonden." };
                return RedirectToAction("Index", "Home");
            }
            //bool val;
            //bool save;

            //var tmpmember = new ElectableMember(pUserId: 11, pElectionId: 2, pOrdering: 1);
            //val = tmpmember.ValidateObject();
            //if (val) save = tmpmember.Save();
            //var tmpmember2 = new ElectableMember(pUserId: 12, pElectionId: 2, pOrdering: 0);
            //val = tmpmember2.ValidateObject();
            //if (val) save = tmpmember2.Save();

            //var tmpModel = new ElectableMemberModel(pPartyId: 1, pElectionId: 2);
            //var tmpmember3 = new ElectableMember(pUserId: 11, pElectionId: 3, pOrdering: 0);
            //val = tmpmember3.ValidateObject();
            //if (val) save = tmpmember3.Save();
            //var tmpmember4 = new ElectableMember(pUserId: 12, pElectionId: 3, pOrdering: 1);
            //val = tmpmember4.ValidateObject();
            //if (val) save = tmpmember4.Save();

            //var tmpmember5 = new ElectableMember(pUserId: 15, pElectionId: 3, pOrdering: 0);
            //val = tmpmember5.ValidateObject();
            //if (val) save = tmpmember5.Save();
            //var tmpmember6 = new ElectableMember(pUserId: 16, pElectionId: 3, pOrdering: 1);
            //val = tmpmember6.ValidateObject();
            //if (val) save = tmpmember6.Save();

            //var datamember = new ElectableMember(pUserId: 15, pElectionId: 3);
            //datamember.Ordering = 5;
            //val = datamember.ValidateObject();
            //if (val) save = datamember.Save();

            //var tmpmembers = ElectableMember.GetList(pIncludingParty: true, pIncludingUser: true);

            //var user = tmpmembers.FirstOrDefault()?.Party;
            //var tmpmembers2 = ElectableMember.GetList(pIncludingParty: true, pIncludingUser: true, pElectionIds: new List<int> { 2, 3 });
            //var tmpmembers3 = ElectableMember.GetList(pIncludingParty: true, pIncludingUser: true, pElectionIds: new List<int> { 3 });
            //var tmpmembers4 = ElectableMember.GetList(pIncludingParty: true, pIncludingUser: true, pPartyIds: new List<int> { 1 });
            //var tmpmembers6 = ElectableMember.GetList(pIncludingParty: true, pIncludingUser: true, pElectionIds: new List<int> { 3 }, pPartyIds: new List<int> { 1 });

            //return View();
        }

        [HttpPost]
        public IActionResult AttemptSaveElectableMember(List<int> pIds)
        {
            return RedirectToAction("Index");
        }
    }
}
