using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Extensions
{
    public static class Utf8JsonWriterExtrensions
    {
        public static void WriteTypeDescriptor<T>(this Utf8JsonWriter writer, T value)
        {
            var attribute = value.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is DerivedTypeAttribute) as DerivedTypeAttribute;
            if (attribute != null)
                writer.WriteString("$type", attribute.TypeDescriptor);
        }
    }
}
