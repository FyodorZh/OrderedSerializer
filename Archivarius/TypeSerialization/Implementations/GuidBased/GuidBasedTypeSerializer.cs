using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OrderedSerializer.TypeSerializers
{
    public class GuidBasedTypeSerializer : ITypeSerializer
    {
        [ThreadStatic] private static List<string>? _ids;
        
        public void Serialize(IWriter writer, Type type)
        {
            _ids ??= new List<string>();
            _ids.Clear();
            EncodeTypeRecursive(_ids, type);
            
            writer.WriteByte(0); // version for the future
            writer.WriteByte(checked((byte)_ids.Count));
            foreach (var id in _ids)
            {
                writer.WriteString(id);
            }
        }

        private static void EncodeTypeRecursive(List<string> ids, Type type)
        {
            string typeId = GetTypeId(type);
            ids.Add(typeId);
            if (type.IsGenericType)
            {
                foreach (var argument in type.GenericTypeArguments)
                {
                    EncodeTypeRecursive(ids, argument);
                }
            }
        }

        private static string GetTypeId(Type type)
        {
            if (type.IsPrimitive)
            {
                return type.Name;
            }

            if (type.Name == "String")
            {
                return "String";
            }
            
            var attribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
            if (attribute == null)
            {
                throw new InvalidOperationException($"'{type}' must have GUID attribute");
            }

            return attribute.Value;
        }
    }
}