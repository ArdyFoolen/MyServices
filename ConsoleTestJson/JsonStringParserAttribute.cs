using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleTestJson
{
    public class JsonStringParserAttribute : JsonParserAttribute
    {
        public override void ParseAndSet<T>(ref Utf8JsonReader reader, T item, string? propertyName)
        {
            reader.Read();
            var value = reader.GetString();
            propertyName.SetProperty(item, value ?? string.Empty);
        }
    }
}
