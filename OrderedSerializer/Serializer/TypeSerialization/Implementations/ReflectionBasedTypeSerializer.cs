using System;

namespace OrderedSerializer.TypeSerializers
{
    public class ReflectionBasedTypeSerializer : ITypeSerializer, ITypeDeserializer
    {
        public void Serialize(IWriter writer, Type type)
        {
            string typeName = type.AssemblyQualifiedName;
            writer.WriteString(typeName);
        }

        public Type Deserialize(IReader reader)
        {
            string typeName = reader.ReadString();
            return Type.GetType(typeName);
        }
    }
}