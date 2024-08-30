using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestJson
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DerivedTypeAttribute : Attribute
    {
        private Type type;
        private string typeDescriptor;

        public DerivedTypeAttribute(Type type, string typeDescriptor)
        {
            this.type = type;
            this.typeDescriptor = typeDescriptor;
        }

        public string TypeDescriptor
        {
            get
            {
                return typeDescriptor;
            }
        }

        public T? Create<T>(params object?[] services) where T : new()
        {
            while (services.Length > 0)
            {
                var constructor = type.GetConstructor(services.Select(o => o.GetType()).ToArray());
                if (constructor != null)
                    return (T)constructor.Invoke(services);
                services = services.Take(services.Length - 1).ToArray();
            }

            return Create<T>();
        }

        private T? Create<T>() where T : new()
        {
            var constructor = type.GetConstructor(Array.Empty<Type>());
            if (constructor != null)
                return (T)constructor.Invoke(Array.Empty<object>());
            return default(T);
        }
    }
}
