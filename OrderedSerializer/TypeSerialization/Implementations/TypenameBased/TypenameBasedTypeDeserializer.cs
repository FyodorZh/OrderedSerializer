using System;

namespace OrderedSerializer.TypeSerializers
{
    public class TypenameBasedTypeDeserializer : ITypeDeserializer
    {
        public Type Deserialize(IReader reader)
        {
            string typeName = reader.ReadString();
            var type = Type.GetType(typeName);
            return type;
        }
    }
}