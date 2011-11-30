using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using PolicyDecisionPointRBAC1.Configurations.Helpers;
using PolicyDecisionPointRBAC1.Configurations;
using PolicyDecisionPointRBAC1.Model;

namespace PolicyDecisionPointRBAC1
{
    public class PolicyDecisionPoint
    {
        private volatile static PolicyDecisionPoint _instance;

        private IEnumerable<User> _users;
        private IEnumerable<Role> _roles;
        private IEnumerable<Permission> _permissions;

        public static PolicyDecisionPoint GetInstance(IEnumerable<string> users, IEnumerable<string> roles, IEnumerable<string> permissions,
            IEnumerable<string> rh, IEnumerable<string> userAssignment, IEnumerable<string> permissionAssignment)
        {
            if(_instance == null)
            {
                _instance = new PolicyDecisionPoint(users, roles, permissions, rh, userAssignment, permissionAssignment);
                
                return _instance;
            }

            throw new ArgumentException();
        }

        public static PolicyDecisionPoint GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PolicyDecisionPoint();
            }

            return _instance;
        }

        private PolicyDecisionPoint(IEnumerable<string> users, IEnumerable<string> roles, IEnumerable<string> permissions,
            IEnumerable<string> rh, IEnumerable<string> userAssignment, IEnumerable<string> permissionAssignment)
        {
            //Order matters! :(
            _permissions = ParsePermissions(permissions);
            _roles = ParseRoles(roles, _permissions, rh, permissionAssignment);
            _users = ParseUsers(users, _roles, userAssignment);
        }

        private PolicyDecisionPoint()
        {
            LoadPolicy();
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

        private static IEnumerable<Role> ParseRoles(IEnumerable<string> roles, IEnumerable<Permission> permissions, IEnumerable<string> rh, IEnumerable<string> permissionAssignment)
        {
            List<Role> r = new List<Role>();
            List<string> rhCopy = new List<string>(rh);

            foreach (string role in roles)
            {
                r.Add(new Role { Name = role, Juniors = new List<Role>(), Permissions = new List<Permission>()});
            }

            Regex regex = new Regex(
                            "((?<Senior>[A-Za-z0-9]+) > (?<Junior>[A-Za-z0-9]+))|((?<Junior>[A-Za-z0-9]+) < (?<Senior>[A-Za-z0-9]+))", RegexOptions.IgnorePatternWhitespace);
                    
            while (rhCopy.Count() != 0)
            {
                string toRemove = null;
                foreach (string rHelement in rhCopy)
                {
                    Match match = regex.Match(rHelement);

                    if (match.Success)
                    {
                        string junior = match.Groups["Junior"].Value;
                        string senior = match.Groups["Senior"].Value;

                        Role seniorRole = new Role {Name = senior};
                        Role juniorRole = new Role {Name = junior};

                        if (!r.Contains(juniorRole)) continue;

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

                        toRemove = rHelement;
                    }
                }

                if(toRemove != null)
                {
                    rhCopy.Remove(toRemove);
                }
            }

            regex = new Regex("((?<role>[A-Za-z0-9]+),(?<permission>[A-Za-z0-9]+))");

            foreach (string assignment in permissionAssignment)
            {
                Match match = regex.Match(assignment);

                if (match.Success)
                {
                    Role role = r[r.IndexOf(new Role { Name = match.Groups["role"].Value })];
                    Permission permission = permissions.Single(b => b.Equals(new Permission { Name = match.Groups["permission"].Value }));

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
                u.Add(new User {Name = user, Roles = new List<Role>()});
            }

            Regex regex = new Regex(@"\((?<user>[A-Za-z0-9]+),(?<role>[A-Za-z0-9]+)\)");

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

        private IEnumerable<Permission> GetAllPermissions(Role role)
        {
            List<Permission> permissions = new List<Permission>();

            permissions.AddRange(role.Permissions);

            foreach (var junior in role.Juniors)
            {
                permissions.AddRange(GetAllPermissions(junior));
            }

            return permissions;
        }

        public Permission GetPermission(string name)
        {
            return _permissions.Single(p => p.Name.Equals(name));
        }

        public bool HasPermission(IPrincipal session, params Permission[] neededPermissions)
        {
            User user = (session is User) ? session as User : _users.Single(u => u.Name.Equals(session.Identity.Name));

            List<Permission> availablePermissions = new List<Permission>();

            foreach (Role role in user.Roles)
            {
                if(session.IsInRole(role.Name))
                    availablePermissions.AddRange(GetAllPermissions(role));
            }

            if(availablePermissions.Count == 0 && user.Roles.Count != 0)
                foreach (Role role in user.Roles)
                {
                    availablePermissions.AddRange(GetAllPermissions(role));
                }

            foreach(Permission permission in neededPermissions)
            {
                if (!availablePermissions.Contains(permission)) return false;
            }

            return true;
        }

        public void SavePolicy()
        {
            Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            PDPSection saveSection = (PDPSection) c.GetSection("PDPPolicy");

            _users.AddToUserCollection(saveSection.Users);
            _roles.AddToRoleCollection(saveSection.Roles);
            _permissions.AddToPermissionCollection(saveSection.Permissions);
            c.Save(ConfigurationSaveMode.Full);
        }

        private void LoadPolicy()
        {
            Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            PDPSection loadSection = (PDPSection)c.GetSection("PDPPolicy");

            List<Permission> permissions = new List<Permission>();

            foreach (var permission in loadSection.Permissions)
            {
                PermissionElement pe = (PermissionElement) permission;
                permissions.Add(new Permission { Name = pe.Name });
            }

            _permissions = permissions;

            List<Role> roles = new List<Role>();

            foreach (var role in loadSection.Roles)
            {
                RoleElement re = (RoleElement)role;

                Role r = new Role {Name = re.Name, Juniors = new List<Role>(), Permissions = new List<Permission>()};
                roles.Add(r);

                foreach (var permission in re.Permissions)
                {
                    PermissionElement pe = (PermissionElement) permission;
                    Permission p = permissions[permissions.IndexOf(new Permission {Name = pe.Name})];
                    r.Permissions.Add(p);
                }
            }

            foreach (var role in loadSection.Roles)
            {
                RoleElement re = (RoleElement)role;

                Role r = roles[roles.IndexOf(new Role {Name = re.Name})];

                foreach (var junior in re.Juniors)
                {
                    SimpleRoleElement re1 = (SimpleRoleElement)junior;
                    Role p = roles[roles.IndexOf(new Role { Name = re1.Name })];
                    r.Juniors.Add(p);
                }
            }

            _roles = roles;

            List<User> users = new List<User>();

            foreach (var user in loadSection.Users)
            {
                UserElement ue = (UserElement) user;

                User u = new User {Name = ue.Name, Roles = new List<Role>()};
                users.Add(u);

                foreach (var role in ue.Roles)
                {
                    SimpleRoleElement re = (SimpleRoleElement)role;
                    Role p = roles[roles.IndexOf(new Role { Name = re.Name })];
                    u.Roles.Add(p);
                }
            }

            _users = users;
        }

    }
}