using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleTestJson
{
    public class JsonDateTimeOffsetParserAttribute : JsonParserAttribute
    {
        public override void ParseAndSet<T>(ref Utf8JsonReader reader, T item, string? propertyName)
        {
            reader.Read();
            var value = reader.GetString();
            var dateTimeOffset = DateTimeOffset.ParseExact(value, "yyyy-MM-dd HH:mm:ss zzz",
                                 CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            propertyName.SetProperty(item, dateTimeOffset);
        }
    }
}
