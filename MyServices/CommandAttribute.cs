using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServices
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        private string value;

        public CommandAttribute(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get
            {
                return value;
            }
        }
    }
}
