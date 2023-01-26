using System;

namespace OrderedSerializer
{
    public interface ITypeDeserializer
    {
        Type? Deserialize(IReader reader);
    }
}