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
