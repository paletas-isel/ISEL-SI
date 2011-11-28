using System;
using System.Configuration;

namespace PDP_RBAC1.Configurations
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
    }
}