using System.Collections.Generic;

namespace PDP_RBAC1.Model
{
    public class Session
    {
        public User User { get; internal set; }

        public List<Permission> Permissions { get; internal set; }
    }
}