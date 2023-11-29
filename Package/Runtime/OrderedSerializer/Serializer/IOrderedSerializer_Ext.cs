using System;
using System.Collections.Generic;

namespace OrderedSerializer
{
    public static class IOrderedSerializer_Ext
    {
        public static void Add<T>(this IPrimitiveSerializer<T> serializer, ref T? value)
            where T : struct
        {
            if (serializer.IsWriter)
            {
                bool isNull = value == null;
                serializer.Writer.WriteBool(isNull);
                if (!isNull)
                {
                    T tmp = value!.Value;
                    serializer.Add(ref tmp);
                }
            }
            else
            {
                bool isNull = serializer.Reader.ReadBool();
                if (isNull)
                {
                    value = null;
                }
                else
                {
                    T tmp = default(T);
                    serializer.Add(ref tmp);
                    value = tmp;
                }
            }
        }
        
        public static void Add<T>(this IOrderedSerializer serializer, ref T? value)
            where T : struct, IDataStruct
        {
            if (serializer.IsWriter)
            {
                bool isNull = value == null;
                serializer.Writer.WriteBool(isNull);
                if (!isNull)
                {
                    T tmp = value!.Value;
                    serializer.AddStruct(ref tmp);
                }
            }
            else
            {
                bool isNull = serializer.Reader.ReadBool();
                if (isNull)
                {
                    value = null;
                }
                else
                {
                    T tmp = default(T);
                    serializer.AddStruct(ref tmp);
                    value = tmp;
                }
            }
        }

        public static void Add<T>(this ISerializer serializer, ref T value)
            where T : struct, Enum
        {
            if (serializer.IsWriter)
            {
                switch (value.GetTypeCode())
                {
                    case TypeCode.Byte:
                        serializer.Writer.WriteByte((byte)(object)value);
                        break;
                    case TypeCode.Int16:
                        serializer.Writer.WriteShort((short)(object)value);
                        break;
                    case TypeCode.Int32:
                        serializer.Writer.WriteInt((int)(object)value);
                        break;
                    case TypeCode.Int64:
                        serializer.Writer.WriteLong((long)(object)value);
                        break;
                    case TypeCode.SByte:
                        serializer.Writer.WriteSByte((sbyte)(object)value);
                        break;
                    case TypeCode.UInt16:
                        serializer.Writer.WriteUShort((ushort)(object)value);
                        break;
                    case TypeCode.UInt32:
                        serializer.Writer.WriteUInt((uint)(object)value);
                        break;
                    case TypeCode.UInt64:
                        serializer.Writer.WriteULong((ulong)(object)value);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (value.GetTypeCode())
                {
                    case TypeCode.Byte:
                        value = (T)(object)serializer.Reader.ReadByte();
                        break;
                    case TypeCode.Int16:
                        value = (T)(object)serializer.Reader.ReadShort();
                        break;
                    case TypeCode.Int32:
                        value = (T)(object)serializer.Reader.ReadInt();
                        break;
                    case TypeCode.Int64:
                        value = (T)(object)serializer.Reader.ReadLong();
                        break;
                    case TypeCode.SByte:
                        value = (T)(object)serializer.Reader.ReadSByte();
                        break;
                    case TypeCode.UInt16:
                        value = (T)(object)serializer.Reader.ReadUShort();
                        break;
                    case TypeCode.UInt32:
                        value = (T)(object)serializer.Reader.ReadUInt();
                        break;
                    case TypeCode.UInt64:
                        value = (T)(object)serializer.Reader.ReadULong();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private static void WriteList<T>(IPrimitiveSerializer<T?> serializer,
            IReadOnlyList<T?>? list)
        {
            if (list == null)
            {
                serializer.Writer.WriteInt(0);
            }
            else
            {
                int count = list.Count;
                serializer.Writer.WriteInt(count + 1);
                for (int i = 0; i < count; ++i)
                {
                    var tmp = list[i];
                    serializer.Add(ref tmp);
                }
            }
        }

        private static T?[]? ReadAsArray<T>(IPrimitiveSerializer<T?> serializer)
        {
            int count = serializer.Reader.ReadInt();
            if (count <= 0)
            {
                return null;
            }

            count -= 1;

            if (count == 0)
            {
                return Array.Empty<T?>();
            }
            
            var array = new T?[count];
            for (int i = 0; i < count; ++i)
            {
                serializer.Add(ref array[i]);
            }

            return array;
        }
        
        private static List<T?>? ReadAsList<T>(IPrimitiveSerializer<T?> serializer)
        {
            int count = serializer.Reader.ReadInt();
            if (count <= 0)
            {
                return null;
            }

            count -= 1;
            
            var list = new List<T?>(count);
            for (int i = 0; i < count; ++i)
            {
                var element = default(T);
                serializer.Add(ref element);
                list.Add(element);
            }

            return list;
        }

        public static void Add<T>(this IPrimitiveSerializer<T?> serializer, ref T?[]? array)
        {
            if (serializer.IsWriter)
            {
                WriteList(serializer, array);
            }
            else
            {
                array = ReadAsArray(serializer);
            }
        }
        
        public static void Add<T>(this IPrimitiveSerializer<T?> serializer, ref List<T?>? list)
        {
            if (serializer.IsWriter)
            {
                WriteList(serializer, list);
            }
            else
            {
                list = ReadAsList(serializer);
            }
        }
        
        public static void Add<T>(this IPrimitiveSerializer<T?> serializer, ref IReadOnlyList<T?>? list)
        {
            if (serializer.IsWriter)
            {
                WriteList(serializer, list);
            }
            else
            {
                list = ReadAsArray(serializer);
            }
        }
    }
}