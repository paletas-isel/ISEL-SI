using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class PDPSection : ConfigurationSection
    {
        [ConfigurationProperty("Users", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(UserCollection), AddItemName = "add")]
        public UserCollection Users
        {
            get
            {
                return (UserCollection)base["Users"];
            }
        }
        
        [ConfigurationProperty("Roles", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RolesCollection), AddItemName = "add")]
        public RolesCollection Roles
        {
            get
            {
                return (RolesCollection)base["Roles"];
            }
        }

        [ConfigurationProperty("Permissions", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(PermissionCollection), AddItemName = "add")]
        public PermissionCollection Permissions
        {
            get
            {
                return (PermissionCollection)base["Permissions"];
            }
        }
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}