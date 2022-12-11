using System;
using OrderedSerializer.BinaryBackend;
using OrderedSerializer.TypeSerializers;

namespace OrderedSerializer
{
    internal class CopyViaSerialization
    {
        [ThreadStatic]
        private static HierarchicalSerializer _serializer;
        
        [ThreadStatic]
        private static HierarchicalDeserializer _deserializer;

        static void CheckSerializer()
        {
            if (_serializer == null)
            {
                var _writer = new BinaryWriter();
                var _reader = new BinaryReaderBasedOnWriter(_writer);
                _serializer = new HierarchicalSerializer(_writer, new TypenameBasedTypeSerializer());
                _deserializer = new HierarchicalDeserializer(_reader, new TypenameBasedTypeDeserializer());
            }

        }

        public static T CopyClass<T>(T source) where T : class, IDataStruct
        {
            CheckSerializer();

            _serializer.Reset();
            _serializer.AddClass(ref source); 
            _deserializer.Reset();
            T copy = default(T);
            _deserializer.AddClass(ref copy);
            return copy;
        }

        public static T CopyStruct<T>(T source) where T : struct, IDataStruct
        {
            CheckSerializer();
            _serializer.Reset();
            _serializer.AddStruct(ref source);
            _deserializer.Reset();
            T copy = default(T);
            _deserializer.AddStruct(ref copy);
            return copy;
        }
    }


    public static class CopyViaSerialization_ClassExt
    {
        public static T Copy<T>(this T source) where T : class, IDataStruct
        {
            return CopyViaSerialization.CopyClass(source);
        }
    }
    public static class CopyViaSerialization_StructExt
    {   
        public static T Copy<T>(this T source) where T : struct, IDataStruct
        {
            return CopyViaSerialization.CopyStruct(source);
        }
    }
}