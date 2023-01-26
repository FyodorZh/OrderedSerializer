using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OrderedSerializer.TypeSerializers
{
    public class GuidBasedTypeDeserializer : ITypeDeserializer
    {
        private static readonly Type _dataStructType = typeof(IDataStruct);

        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public GuidBasedTypeDeserializer()
        {
        }

        public GuidBasedTypeDeserializer(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                RegisterAssembly(assembly);
            }
        }

        public void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && _dataStructType.IsAssignableFrom(type))
                {
                    var attribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
                    if (attribute != null && Guid.TryParse(attribute.Value, out _))
                    {
                        _types.Add(attribute.Value, type);
                    }
                }
            }
        }

        public void RegisterType(Type type)
        {
            if (!type.IsClass || !_dataStructType.IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"{type} must be a class that is inherited from IDataStruct");
            }

            var attribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
            if (attribute == null || !Guid.TryParse(attribute.Value, out _))
            {
                throw new InvalidOperationException($"{type} must have valid GUID");
            }

            _types.Add(attribute.Value, type);
        }

        public Type? Deserialize(IReader reader)
        {
            byte version = reader.ReadByte();
            string? guid = reader.ReadString();

            if (guid != null && _types.TryGetValue(guid, out var type))
            {
                return type;
            }

            return null;
        }
    }
}