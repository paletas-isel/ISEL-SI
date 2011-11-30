using System;
using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class UserCollection : ConfigurationElementCollection
    {
        public void Add(UserElement serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return new UserElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserElement) element).Name;
        }

        #endregion

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}