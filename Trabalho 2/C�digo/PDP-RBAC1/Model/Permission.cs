using System;
using System.Security;
using System.Security.Principal;
using System.Threading;
using PolicyDecisionPointRBAC1.Configurations;
using PolicyEnforcementPointApplication.Filter;

namespace PolicyDecisionPointRBAC1.Model
{
    public class Permission : IPermission
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Permission)) return false;
            Permission p = obj as Permission;
            return p.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #region Implementation of ISecurityEncodable

        public SecurityElement ToXml()
        {
            return new SecurityElement("permission", Name);
        }

        public void FromXml(SecurityElement e)
        {
            Name = e.Text;
        }

        #endregion

        #region Implementation of IPermission

        public IPermission Copy()
        {
            return new Permission {Name = Name};
        }

        public IPermission Intersect(IPermission target)
        {
            throw new NotImplementedException();
        }

        public IPermission Union(IPermission target)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IPermission target)
        {
            throw new NotImplementedException();
        }

        public void Demand()
        {
            IPrincipal user = Thread.CurrentPrincipal;
            if (user.Identity.IsAuthenticated)
            {
                PolicyDecisionPoint p = PolicyDecisionPoint.GetInstance();

                if (!p.HasPermission(user, this))
                {
                    throw new InsufficientPrivilegesException();
                }
            }
        }

        #endregion
    }
}