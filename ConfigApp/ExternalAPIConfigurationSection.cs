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
        [ConfigurationProperty("limits", IsRequired = true, IsDefaultCollection = true)]
        public LimitElementCollection Limits
        {
            get { return (LimitElementCollection)this["limits"]; }
        }

        [ConfigurationProperty("limitusers", IsRequired = false, IsDefaultCollection = true)]
        public LimitUserElementCollection LimitUsers
        {
            get { return (LimitUserElementCollection)this["limitusers"]; }
        }
    }
}
