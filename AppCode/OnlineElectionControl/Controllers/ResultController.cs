using Microsoft.AspNetCore.Mvc;
using OnlineElectionControl.Classes;
using OnlineElectionControl.Models;

namespace OnlineElectionControl.Controllers
{
    public class ResultController : Controller
    {
        public IActionResult Index(int? pId)
        {
            if (pId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                return View(new ResultModel(pElectionId: (int)pId!));
            }
            catch
            {
                TempData.Clear();
                TempData["Vml"] = new string[] { $"Election with Id {pId} not found." };
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
