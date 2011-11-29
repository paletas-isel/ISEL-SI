using System.Collections.Generic;

namespace PolicyDecisionPointRBAC1.Model
{
    public class User
    {
        public string Name { get; internal set; }

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
    }
}