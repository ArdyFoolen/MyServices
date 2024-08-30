using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyServices.Json;

namespace MyServices.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type parent)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes(); // Maybe select some other assembly here, depending on what you need
            return types.Where(t => parent.IsAssignableFrom(t));
        }

        public static DerivedTypeAttribute? GetDerivedTypeAttributeFrom(this Type parent, string? descriptor)
            => parent.GetDerivedTypes()
                .SelectMany(t => t.GetCustomAttributes(true))
                .Where(a => a is DerivedTypeAttribute)
                .Select(a => a as DerivedTypeAttribute)
                .FirstOrDefault(d => descriptor?.ToLowerInvariant().Equals(d?.TypeDescriptor.ToLowerInvariant()) ?? false);

        public static JsonParserAttribute? GetJsonParserAttributeFrom(this Type type, string? propertyName)
            => type?.GetProperty(propertyName ?? string.Empty)?
            .GetCustomAttributes(false)
            .Where(a => typeof(JsonParserAttribute).IsAssignableFrom(a.GetType()))
            .Select(a => a as JsonParserAttribute)
            .FirstOrDefault();

        /// <summary>
        /// Use reflection to set value to property with name on an item object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="item"></param>
        /// <param name="value"></param>
        public static void SetProperty<T>(this string? propertyName, T item, object value)
            => item?.GetType()?.GetProperty(propertyName ?? string.Empty)?.SetValue(item, value);
    }
}
