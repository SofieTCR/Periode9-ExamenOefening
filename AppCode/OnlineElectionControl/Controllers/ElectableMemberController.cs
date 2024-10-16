using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;
using System.Security.Cryptography;

namespace OnlineElectionControl.Controllers
{
    public class ElectableMemberController : Controller
    {
        public IActionResult Index(int? pId)
        {
            if (pId == null || !Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsPartyLeader) return RedirectToAction("Index", "Home");
            try
            {
                var tmpElection = Election.GetList(pElectionIds: new List<int> { (int) pId })[0];
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
        }

        [HttpPost]
        public IActionResult AttemptSaveElectableMember(int? pElectionId, List<int> pIds)
        {
            if (pElectionId == null || !Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsPartyLeader) return RedirectToAction("Index", "Home");
            try
            {
                var tmpElection = Election.GetList(pElectionIds: new List<int> { (int) pElectionId })[0];
                if (tmpElection.Status != ElectionStatus.Scheduled)
                {
                    TempData.Clear();
                    TempData["Vml"] = new string[] { $"Verkiesbare leden voor verkiezing \"{tmpElection.Name}\" mogen niet meer gemuteerd worden!." };
                    return RedirectToAction("Index", "Home");
                }
                var tmpModel = new ElectableMemberModel(pPartyId: (int)Current.LoggedInUser!.LeadingParty_PartyId!
                                                      , pElection: tmpElection);
                var tmpToSaveElectableMembers = new List<ElectableMember>();
                var tmpToRemoveElectableMembers = new List<ElectableMember>();
                var tmpVml = new List<string>();

                foreach (var member in tmpModel.SortedMembers)
                {
                    if (pIds.Contains((int) member.user.UserId!))
                    {
                        ElectableMember tmpElectableMemberObject;
                        if (member.electableMember != null)
                        {
                            tmpElectableMemberObject = member.electableMember;
                            tmpElectableMemberObject.Ordering = pIds.IndexOf((int) member.user.UserId!);
                        }
                        else tmpElectableMemberObject = new ElectableMember(pUserId: (int) member.user.UserId!
                                                                          , pElectionId: (int) pElectionId
                                                                          , pOrdering: pIds.IndexOf((int) member.user.UserId!));
                        tmpToSaveElectableMembers.Add(tmpElectableMemberObject);
                    }
                    else if (!pIds.Contains((int) member.user.UserId!) && member.electableMember != null)
                    {
                        tmpToRemoveElectableMembers.Add(member.electableMember);
                    }
                }

                foreach (var em in tmpToSaveElectableMembers)
                {
                    if (!em.ValidateObject())
                    {
                        tmpVml.Add($"ElectableMember with Id ({em.User_UserId}) failed to validate!");
                        tmpVml.AddRange(em.Vml);
                    }
                }
                if (tmpVml.Count == 0)
                {
                    foreach (var em in tmpToSaveElectableMembers)
                    {
                        if (!em.Save())
                        {
                            tmpVml.Add($"ElectableMember with Id ({em.User_UserId}) failed to save!");
                            tmpVml.AddRange(em.Vml);
                        }
                    }
                    foreach (var em in tmpToRemoveElectableMembers)
                    {
                        if (!em.Delete())
                        {
                            tmpVml.Add($"ElectableMember with Id ({em.User_UserId}) failed to delete!");
                        }
                    }
                }
            }
            catch
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { $"Verkiezing met Id {pElectionId} niet gevonden." };
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
