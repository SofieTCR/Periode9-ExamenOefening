using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Controllers
{
    public class PartyMemberController : Controller
    {
        public IActionResult Index()
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsPartyLeader) return RedirectToAction("Index", "Home");
            try
            {
                return View(new PartyMemberModel(pId: (int) Current.LoggedInUser.LeadingParty_PartyId!));
            }
            catch
            {
                TempData["Vml"] = new string[] { "Something went wrong!" };
                return View("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult AttemptSavePartyMember(List<int> pIds)
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsPartyLeader) return RedirectToAction("Index", "Home");
            TempData["pIds"] = pIds;

            var tmpModel = new PartyMemberModel(pId: (int)Current.LoggedInUser.LeadingParty_PartyId!);
            var tmpPartyId = Current.LoggedInUser.LeadingParty_PartyId;
            var tmpVml = new List<string>();
            var tmpChangedMembers = new List<User>();

            foreach (var member in tmpModel.Members)
            {
                if (member.UserIsPartyMember && !pIds.Contains((int) member.UserId!))
                {
                    member.Party_PartyId = null;
                    tmpChangedMembers.Add(member);
                }
                else if (!member.UserIsPartyMember && pIds.Contains((int) member.UserId!))
                {
                    member.Party_PartyId = tmpPartyId;
                    tmpChangedMembers.Add(member);
                }
            }
            foreach (var changedMember in tmpChangedMembers)
            {
                if (!changedMember.ValidateObject())
                {
                    tmpVml.Add($"User: {changedMember.FirstName} {changedMember.LastName} with Id ({changedMember.UserId}) failed to validate!");
                    tmpVml.AddRange(changedMember.Vml);
                }
            }
            if (tmpVml.Count == 0) foreach (var changedMember in tmpChangedMembers)
            {
                if (!changedMember.Save())
                {
                    tmpVml.Add($"User: {changedMember.FirstName} {changedMember.LastName} with Id ({changedMember.UserId}) failed to save!");
                    tmpVml.AddRange(changedMember.Vml);
                }
            }

            if (tmpVml.Count > 0) TempData["Vml"] = tmpVml.ToArray();
            else if (tmpChangedMembers.Count > 0)
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { "All party members saved succesfully!" };
                Current.DeleteCache();
            }

            return RedirectToAction("Index");
        }
    }
}
