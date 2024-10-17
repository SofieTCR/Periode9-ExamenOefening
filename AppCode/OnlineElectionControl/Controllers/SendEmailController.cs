using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly EmailService _emailService;

        public SendEmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsGovernment) return RedirectToAction("Index", "Home");
            return View(new SendEmailModel());
        }

        [HttpPost]
        public async Task<IActionResult> AttemptSendEmail(List<int> pIds)
        {
            if (!Current.UserIsLoggedIn || !Current.LoggedInUser!.UserIsGovernment || pIds == null || pIds.Count == 0) return RedirectToAction("Index", "Home");
            var tmpElections = Election.GetList(pElectionIds: pIds);
            List<(string Subject, string Body, string ToEmail)> tmpEmails = new List<(string Subject, string Body, string ToEmail)>();

            if (tmpElections.Count != pIds.Count)
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { "Er is iets fout gegaan bij het ophalen van de verkiezingen." };
                return RedirectToAction("Index");
            }

            foreach (var tmpElection in tmpElections)
            {
                switch (tmpElection.Status)
                {
                    case ElectionStatus.Scheduled:
                    case ElectionStatus.InProgress:
                        var tmpUsers = Classes.User.GetList(pReferenceDate: tmpElection.Date, pIsEligible: true);
                        foreach (var user in tmpUsers)
                        {
                            if (!Current.UserCanVote(pElectionId: (int) tmpElection.ElectionId!, pUser: user)) continue;
                            if ((!user.Email.Contains("zadkine") && !user.Email.Contains("tcrmbo"))
                            //  || !user.Email.Contains("9019232")
                            ) continue; // don't send emails to random fucking people please!
                            (string Subject, string Body, string ToEmail) tmpEmailTuple;

                            tmpEmailTuple.ToEmail = user.Email;
                            tmpEmailTuple.Subject = $"Let op! U kunt {(tmpElection.Status == ElectionStatus.InProgress ? "vandaag" : "op " + tmpElection.Date.ToString("dd-MM" + (tmpElection.Date.Year == DateTime.Today.Year ? "" : "-yyyy")))} uw stem uitbrengen voor de {tmpElection.Name}!";
                            tmpEmailTuple.Body = $@"<body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; margin: 0; padding: 0;"">
                                                      <div style=""background-color: #ffffff; width: 100%; max-width: 600px; margin: 0 auto; border: 1px solid #cccccc;"">
                                                        <div style=""background-color: #3E548C; color: white; padding: 20px; text-align: center;"">
                                                          <img src=""https://cdn.worldvectorlogo.com/logos/rijksoverheid.svg"" alt=""Logo Rijksoverheid"" style=""max-width: 120px; background-color: #d5d4d4"">
                                                        </div>
                                                        <div style=""padding: 20px; color: #333;"">
                                                          <h1 style=""font-size: 24px; color: #3E548C;"">Verkiezingsoproep</h1>
                                                          <p style=""font-size: 16px; line-height: 1.6;"">Beste <strong>{user.FirstName} {user.LastName}</strong>,</p>
                                                          <p style=""font-size: 16px; line-height: 1.6;"">U bent uitgenodigd om deel te nemen aan de komende verkiezing. Hieronder vindt u de details:</p>
                                                          <ul style=""font-size: 16px; line-height: 1.6; padding-left: 20px;"">
                                                            <li><strong>Verkiezing:</strong> {tmpElection.Name}</li>
                                                            <li><strong>Datum:</strong> {tmpElection.Date.ToString("dd-MM-yyyy")}</li>
                                                          </ul>
                                                          <p style=""font-size: 16px; line-height: 1.6;"">U kunt stemmen door <a href=""rijksoverheid.nl"" style=""color: #003082; text-decoration: underline;"">hier te klikken</a>.</p>
                                                          <p style=""font-size: 16px; line-height: 1.6;"">Bedankt voor uw deelname aan het democratisch proces.</p>
                                                          <p style=""font-size: 16px; line-height: 1.6;"">Met vriendelijke groet,</p>
                                                          <p style=""font-size: 16px; line-height: 1.6;""><strong>Rijksoverheid Nederland</strong></p>
                                                        </div>
                                                        <div style=""background-color: #f2f2f2; padding: 10px; text-align: center; font-size: 12px; color: #666;"">
                                                          <p>Dit is een automatisch gegenereerde e-mail. Gelieve niet te antwoorden.</p>
                                                          <p>© Rijksoverheid Nederland</p>
                                                        </div>
                                                      </div>
                                                    </body>";

                            tmpEmails.Add(tmpEmailTuple);
                        }
                        break;
                    default:
                        throw new Exception($"You may not send emails for Election {tmpElection.Name} ({tmpElection.ElectionId}) with status: {tmpElection.Status.ToString()}");
                }
            }

            foreach(var tmpEmail in tmpEmails)
            {
                await _emailService.SendEmailAsync(tmpEmail.ToEmail, tmpEmail.Subject, tmpEmail.Body);
            }

            TempData["Vml"] = new string[] { $"{tmpEmails.Count} email(s) succesvol verzonden!" };
            return RedirectToAction("Index", "Home");
        }
    }
}
