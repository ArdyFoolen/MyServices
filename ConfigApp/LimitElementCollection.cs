using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigApp
{
    public partial class ExternalAPIConfigurationSection : ConfigurationSection
    {
        [ConfigurationCollection(typeof(LimitConfigurationElement), AddItemName = "limit")]
        public class LimitElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new LimitConfigurationElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((LimitConfigurationElement)element).Name;
            }

            public LimitConfigurationElement GetLimit(string key)
            {
                return BaseGet(key) as LimitConfigurationElement;
            }

            public LimitUserElementCollection GetLimitUsers()
            {
                return BaseGet("APILinksLimitUsers") as LimitUserElementCollection;
            }
        }
    }
}
