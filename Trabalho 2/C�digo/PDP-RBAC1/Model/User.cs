using System.Collections.Generic;

namespace PDP_RBAC1.Model
{
    public class User
    {
        public string Name { get; internal set; }

        public List<Role> Roles { get; internal set; }
    }
}