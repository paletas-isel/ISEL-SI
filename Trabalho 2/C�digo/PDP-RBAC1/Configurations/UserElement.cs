using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class UserElement : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["Name"]; }

            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Roles", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SimpleRolesCollection), AddItemName = "add")]
        public SimpleRolesCollection Roles
        {
            get
            {
                return (SimpleRolesCollection)base["Roles"];
            }
        }
    }
}
