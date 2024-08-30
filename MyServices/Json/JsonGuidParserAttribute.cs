using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServices.Extensions;

namespace MyServices.Json
{
    public class JsonGuidParserAttribute : JsonParserAttribute
    {
        public override void ParseAndSet<T>(ref Utf8JsonReader reader, T item, string? propertyName)
        {
            reader.Read();
            var value = reader.GetString();
            var guid = Guid.ParseExact(value, "D");
            propertyName.SetProperty(item, guid);
        }
    }
}
