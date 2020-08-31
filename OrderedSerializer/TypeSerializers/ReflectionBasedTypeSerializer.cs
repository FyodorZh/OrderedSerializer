using System;
using System.Reflection;

namespace OrderedSerializer.TypeSerializers
{
    public class ReflectionBasedTypeSerializer : ITypeSerializer, ITypeDeserializer
    {
        public void Serialize(IWriter writer, Type type)
        {
            string typeName = type.FullName;
            writer.WriteString(typeName);
        }

        public Type Deserialize(IReader reader)
        {
            string typeName = reader.ReadString();
            var assembly = Assembly.GetEntryAssembly();
            return assembly.GetType(typeName);
        }
    }
}