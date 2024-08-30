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
        public class LimitUserConfigurationElement : ConfigurationElement
        {
            private const string NameElement = "id";

            [ConfigurationProperty(NameElement, IsKey = true, IsRequired = true)]
            public string UserId
            {
                get { return (string)base[NameElement]; }
            }
        }
    }
}
