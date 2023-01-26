using System;

namespace OrderedSerializer
{
    public interface ITypeSerializer
    {
        void Serialize(IWriter writer, Type type);
    }
}