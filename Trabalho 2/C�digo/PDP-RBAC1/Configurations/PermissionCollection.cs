using System;
using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class PermissionCollection : ConfigurationElementCollection
    {
        public void Add(PermissionElement serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return new PermissionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PermissionElement) element).Name;
        }

        #endregion

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}