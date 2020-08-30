using System;

namespace BinarySerializer
{
    public interface ITypeSerializer
    {
        void Serialize(IWriter writer, Type type);
    }

    public interface ITypeDeserializer
    {
        Type Deserialize(IReader reader);
    }
}