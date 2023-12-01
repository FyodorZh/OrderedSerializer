using System;
using System.Collections.Generic;

namespace OrderedSerializer
{
    public static partial class IOrderedSerializer_Ext // Nullable
    {
        /// <summary>
        /// Nullable "primitive" value type 
        /// </summary>
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
        
        /// <summary>
        /// Nullable "DataStruct" value type 
        /// </summary>
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

        

        public static void Add<TSerializer, TKey, TValue>(this TSerializer serializer, ref KeyValuePair<TKey, TValue?> kv)
            where TSerializer : IPrimitiveSerializer<TKey>, IPrimitiveSerializer<TValue?>
        {
            if (serializer.IsWriter)
            {
                var k = kv.Key;
                var v = kv.Value;
                serializer.Add(ref k);
                serializer.Add(ref v);
            }
            else
            {
                TKey k = default(TKey)!;
                var v = default(TValue);
                serializer.Add(ref k);
                serializer.Add(ref v);
                kv = new KeyValuePair<TKey, TValue?>(k, v);
            }
        }
    }
}