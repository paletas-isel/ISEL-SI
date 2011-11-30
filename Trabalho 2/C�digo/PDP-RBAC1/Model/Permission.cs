using System;
using System.Security;
using PolicyDecisionPointRBAC1.Configurations;

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
            throw new NotImplementedException();
        }

        #endregion
    }
}