using System;
using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class RolesCollection : ConfigurationElementCollection
    {
        public void Add(RoleElement serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return new RoleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RoleElement) element).Name;
        }

        #endregion

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}