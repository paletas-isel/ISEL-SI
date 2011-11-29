using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace PolicyDecisionPointRBAC1.Model
{
    public class User : IPrincipal
    {
        private IIdentity _identity;

        public string Name
        {
            get { return _identity.Name; } 
            
            internal set { _identity = new GenericIdentity(value); }
        }

        public List<Role> Roles { get; internal set; }

        public override bool Equals(object obj)
        {
            if (!(obj is User)) return false;
            User p = obj as User;
            return p.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #region Implementation of IPrincipal

        public bool IsInRole(string role)
        {
            return IsRolePresent(role, Roles);
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        #endregion

        private static bool IsRolePresent(string role, IEnumerable<Role> roles)
        {
            foreach (Role r in roles)
            {
                if (r.Name.Equals(role)) return true;

                return IsRolePresent(role, r.Juniors);
            }

            return false;
        }
    }
}