using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OrderedSerializer.TypeSerializers
{
    public class GuidBasedTypeDeserializer : ITypeDeserializer
    {
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public GuidBasedTypeDeserializer(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
                    if (attribute != null)
                    {
                        _types.Add(attribute.Value, type);
                    }
                }
            }
        }

        public Type Deserialize(IReader reader)
        {
            string guid = reader.ReadString();

            if (_types.TryGetValue(guid, out var type))
            {
                return type;
            }

            return null;
        }
    }
}