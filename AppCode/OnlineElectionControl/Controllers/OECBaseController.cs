using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineElectionControl.Classes;

namespace OnlineElectionControl.Controllers
{
    public class OECBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Current.LoggedInUserId = HttpContext.Session.GetInt32("LoggedInUserId");
            base.OnActionExecuting(context);
        }
    }
}
