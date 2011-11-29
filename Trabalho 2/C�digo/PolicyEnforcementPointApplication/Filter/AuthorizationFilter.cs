using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using PolicyDecisionPointRBAC1;
using PolicyDecisionPointRBAC1.Model;

namespace PolicyEnforcementPointApplication.Filter
{
    public class AuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        public string[] NeededPermissions { get; set; }

        #region Implementation of IAuthorizationFilter

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            IPrincipal user = filterContext.HttpContext.User;
            if(user.Identity.IsAuthenticated)
            {
                PolicyDecisionPoint p = PolicyDecisionPoint.GetInstance();

                Session s = p.CreateSession(user);

                if(!p.HasPermission(s, NeededPermissions))
                {
                    throw new InsufficientPrivilegesException();
                }
            }
        }

        #endregion
    }
}