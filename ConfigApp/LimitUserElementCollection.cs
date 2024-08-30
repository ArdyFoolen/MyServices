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
        [ConfigurationCollection(typeof(LimitUserConfigurationElement), AddItemName = "limituser")]
        public class LimitUserElementCollection : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new LimitUserConfigurationElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((LimitUserConfigurationElement)element).UserId;
            }

            [ConfigurationProperty("limit", IsRequired = true, DefaultValue = 0)]
            public int DefaultLimit
            {
                get { return (int)base["limit"]; }
            }

            public IEnumerable<int> GetLimitUsers()
            {
                return BaseGetAllKeys().Select(s => int.Parse((string)s));
            }
        }
    }
}
