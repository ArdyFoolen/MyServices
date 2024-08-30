using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServices.Extensions;

namespace MyServices.Json
{
    public class JsonIntParserAttribute : JsonParserAttribute
    {
        public override void ParseAndSet<T>(ref Utf8JsonReader reader, T item, string? propertyName)
        {
            reader.Read();
            var value = reader.GetInt32();
            propertyName.SetProperty(item, value);
        }
    }
}
