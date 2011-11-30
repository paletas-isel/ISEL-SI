using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PolicyEnforcementPointApplication.Filter;

namespace PolicyEnforcementPointApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        [AuthorizationFilter( NeededPermissions = new [] { "P2" })]
        public ActionResult About()
        {
            return View();
        }
    }
}
