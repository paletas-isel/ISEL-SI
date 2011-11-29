using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PolicyDecisionPointRBAC1;

namespace PolicyEnforcementPointApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            List<string> users = new List<string> { "U1", "U2", "U3", "U4" };

            List<string> roles = new List<string> { "R1", "R2", "R3", "R4" };

            List<string> permissions = new List<string> { "P1", "P2", "P3" };

            List<string> rh = new List<string> { "R1<R2", "R1<R3", "R2<R4", "R3<R4" };

            List<string> ua = new List<string> { "(U1,R1)", "(U2,R2)", "(U3,R3)", "(U4,R4)" };

            List<string> pa = new List<string> { "(R1,P1)", "(R2,P2)", "(R3,P3)" };

            PolicyDecisionPoint.GetInstance(users, roles, permissions, rh, ua, pa);
        }
    }
}