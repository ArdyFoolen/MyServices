using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyServices.Json
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class JsonParserAttribute : Attribute
    {
        public abstract void ParseAndSet<T>(ref Utf8JsonReader reader, T item, string? propertyName);
    }
}
