using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServices.Extensions;

namespace MyServices.Json
{
    public class JsonParserFactory
    {
        public JsonParserAttribute? GetParser<T>(T item, string? propertyName)
            => item?.GetType()?.GetJsonParserAttributeFrom(propertyName ?? string.Empty) ?? new JsonStringParserAttribute();
    }
}
