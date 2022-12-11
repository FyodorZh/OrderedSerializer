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
                throw new InvalidOperationException($"'{type}' must have GUID attribute");
            }

            writer.WriteByte(0); // version for the future
            writer.WriteString(attribute.Value);
        }
    }
}