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
        public class LimitConfigurationElement : ConfigurationElement
        {
            private const string NameElement = "name";
            private const string ValueElement = "value";

            [ConfigurationProperty(NameElement, IsKey = true, IsRequired = true)]
            public string Name
            {
                get { return (string)base[NameElement]; }
            }

            [ConfigurationProperty("value", IsRequired = false, DefaultValue = "")]
            public string Value
            {
                get { return (string)base[ValueElement]; }
            }
        }
    }
}
