using System.Collections.Generic;
using PDP_RBAC1.Model;

namespace PDP_RBAC1.Configurations.Helpers
{
    public static class IEnumerableHelpers
    {
        public static void AddToUserCollection(this IEnumerable<User> users, UserCollection collection)
        {
            foreach (User user in users)
            {
                UserElement element = new UserElement { Name = user.Name };
                AddToSimpleRoleCollection(user.Roles, element.Roles);
                collection.Add(element);
            }
        }

        public static void AddToRoleCollection(this IEnumerable<Role> roles, RolesCollection collection)
        {
            foreach (Role role in roles)
            {
                RoleElement element = new RoleElement { Name = role.Name };
                AddToSimpleRoleCollection(role.Juniors, element.Juniors);
                AddToPermissionCollection(role.Permissions, element.Permissions);
                collection.Add(element);
            }
        }

        private static void AddToSimpleRoleCollection(IEnumerable<Role> roles, SimpleRolesCollection collection)
        {
            foreach (Role role in roles)
            {
                SimpleRoleElement element = new SimpleRoleElement { Name = role.Name };
                collection.Add(element);
            }
        }

        public static void AddToPermissionCollection(this IEnumerable<Permission> permissions, PermissionCollection collection)
        {
            foreach (Permission permission in permissions)
            {
                PermissionElement element = new PermissionElement { Name = permission.Name };
                collection.Add(element);
            }
        }
    }
}