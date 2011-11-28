using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PDP_RBAC1.Configurations
{
    public class UserElement : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["name"]; }

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
