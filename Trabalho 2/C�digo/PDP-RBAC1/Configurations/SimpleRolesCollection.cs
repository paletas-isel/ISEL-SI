using System.Configuration;

namespace PolicyDecisionPointRBAC1.Configurations
{
    public class SimpleRolesCollection : ConfigurationElementCollection
    {
        public void Add(SimpleRoleElement serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement()
        {
            return new SimpleRoleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SimpleRoleElement)element).Name;
        }

        #endregion

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}