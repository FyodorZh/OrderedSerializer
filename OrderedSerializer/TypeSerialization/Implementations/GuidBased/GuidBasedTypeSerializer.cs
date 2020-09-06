using System;
using System.Runtime.InteropServices;

namespace OrderedSerializer.TypeSerializers
{
    public class GuidBasedTypeSerializer : ITypeSerializer
    {
        public void Serialize(IWriter writer, Type type)
        {
            var attribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
            if (attribute == null)
            {
                throw new InvalidOperationException();
            }
            writer.WriteString(attribute.Value);
        }
    }
}