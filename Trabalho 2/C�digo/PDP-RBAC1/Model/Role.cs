using System.Collections.Generic;

namespace PolicyDecisionPointRBAC1.Model
{
    public class Role
    {
        public string Name { get; internal set; }

        public List<Role> Juniors { get; internal set; }

        public List<Permission> Permissions { get; internal set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Role)) return false;
            Role p = obj as Role;
            return p.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}