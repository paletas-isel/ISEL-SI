using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PDP_RBAC1.Model;

namespace PDP_RBAC1
{
    public class PolicyDecisionPoint
    {
        private IEnumerable<User> _users;
        private IEnumerable<Role> _roles;
        private IEnumerable<Permission> _permissions;
        private IEnumerable<Session> _sessions;

        public PolicyDecisionPoint(IEnumerable<string> users, IEnumerable<string> roles, IEnumerable<string> permissions,
            IEnumerable<string> userAssignment, IEnumerable<string> permissionAssignment)
        {
            //Order matters! :(
            _permissions = ParsePermissions(permissions);
            _roles = ParseRoles(roles, _permissions, permissionAssignment);
            _users = ParseUsers(users, _roles, userAssignment);
        }

        private static IEnumerable<Permission> ParsePermissions(IEnumerable<string> permissions)
        {
            List<Permission> p = new List<Permission>();

            foreach (string permission in permissions)
            {
                p.Add(new Permission { Name = permission });
            }

            return p;
        }

        private static IEnumerable<Role> ParseRoles(IEnumerable<string> roles, IEnumerable<Permission> permissions, IEnumerable<string> permissionAssignment)
        {
            List<Role> r = new List<Role>();
            List<string> rolesCopy = new List<string>(roles);
            
            Regex regex =
                        new Regex(
                            "[((?<Senior>[A-Za-z0-9]+) > (?<Junior>[A-Za-z0-9]+))((?<Junior>[A-Za-z0-9]+) < (?<Senior>[A-Za-z0-9]+))]");
                    
            while (rolesCopy.Count() != 0)
            {
                string toRemove = null;
                foreach (string role in rolesCopy)
                {
                    Match match = regex.Match(role);
                    if (!match.Success) r.Add(new Role {Name = role});
                    else
                    {
                        string junior = match.Groups["Junior"].Value;
                        string senior = match.Groups["Senior"].Value;

                        Role seniorRole = new Role {Name = senior};
                        Role juniorRole = new Role {Name = junior};

                        if (!r.Contains(new Role {Name = junior})) continue;

                        if (r.Contains(seniorRole))
                        {
                            seniorRole = r[r.IndexOf(seniorRole)];
                            seniorRole.Juniors.Add(r[r.IndexOf(juniorRole)]);
                        }
                        else
                        {
                            seniorRole.Juniors.Add(juniorRole);
                            r.Add(seniorRole);
                        }

                        toRemove = role;
                    }
                }

                if(toRemove != null)
                {
                    rolesCopy.Remove(toRemove);
                }
            }

            regex = new Regex("((?<role>),(?<permission>))");

            foreach (string assignment in permissionAssignment)
            {
                Match match = regex.Match(assignment);

                if (match.Success)
                {
                    Role role = r[r.IndexOf(new Role { Name = match.Groups["role"].Value })];
                    Permission permission = permissions.Single(b => b.Equals(new Permission() { Name = match.Groups["permission"].Value }));

                    role.Permissions.Add(permission);
                }
            }

            return r;
        }

        private static IEnumerable<User> ParseUsers(IEnumerable<string> users, IEnumerable<Role> roles, IEnumerable<string> userAssignment)
        {
            List<User> u = new List<User>();

            foreach (string user in users)
            {
                u.Add(new User {Name = user});
            }

            Regex regex = new Regex("((?<user>),(?<role>))");

            foreach (string assignment in userAssignment)
            {
                Match match = regex.Match(assignment);

                if(match.Success)
                {
                    User user = u[u.IndexOf(new User {Name = match.Groups["user"].Value})];
                    Role role = roles.Single(b => b.Equals(new Role { Name = match.Groups["role"].Value }));

                    user.Roles.Add(role);
                }
            }

            return u;
        }
    
        public Session CreateSession(User user)
        {
            Session s = new Session();
            s.User = user;
            s.Permissions = new List<Permission>();

            foreach (Permission permission in user.Roles.SelectMany(u => u.Permissions))
            {
                s.Permissions.Add(permission);
            }

            return s;
        }

        public bool HasPermission(Session session, IEnumerable<Permission> neededPermissions)
        {
            foreach(Permission permission in neededPermissions)
            {
                if (!session.Permissions.Contains(permission)) return false;
            }

            return true;
        }

    }
}