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

        private IEnumerable<Permission> _permissions;

        #region Implementation of IAuthorizationFilter

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            IPrincipal user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                PolicyDecisionPoint p = PolicyDecisionPoint.GetInstance();

                if(_permissions == null)
                {
                    List<Permission> permissions = new List<Permission>();

                    foreach (var neededPermission in NeededPermissions)
                    {
                        Permission permission = p.GetPermission(neededPermission);
                        permissions.Add(permission);
                        permission.Demand();
                    }

                    _permissions = permissions;
                }
                else
                {
                    foreach (var permission in _permissions)
                    {
                        permission.Demand();
                    }
                }

                return;
            }

            throw new InsufficientPrivilegesException();
        }

        #endregion
    }
}