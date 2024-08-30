using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServices.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum ToEnum<TEnum>(this string value)
        {
            foreach (var field in typeof(TEnum).GetFields())
            {
                if (field.FieldType.IsEnum)
                {
                    var commandAttribute = Attribute.GetCustomAttribute(field, typeof(CommandAttribute)) as CommandAttribute;
                    if (commandAttribute?.Value.Equals(value) ?? false)
                        return (TEnum)field.GetValue(null);
                }
            }

            throw new ArgumentException($"CommandAttribute Not found with {typeof(TEnum)} and value {value}");
        }
    }
}
