using System.Configuration;

namespace PDP_RBAC1.Configurations
{
    public class PermissionElement : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["name"]; }

            set { this["Name"] = value; }
        }
    }
}