using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class RoleElement : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["Name"]; }

            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Juniors", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SimpleRolesCollection), AddItemName = "add")]
        public SimpleRolesCollection Juniors
        {
            get
            {
                return (SimpleRolesCollection)base["Juniors"];
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