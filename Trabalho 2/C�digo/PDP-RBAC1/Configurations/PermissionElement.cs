using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class PermissionElement : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["Name"]; }

            set { this["Name"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}