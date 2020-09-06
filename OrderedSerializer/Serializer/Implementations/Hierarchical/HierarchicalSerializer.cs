using System;
using System.Collections.Generic;

namespace OrderedSerializer
{
    public class HierarchicalSerializer : PrimitiveSerializer, IOrderedSerializer
    {
        private readonly ITypeSerializer _typeSerializer;

        private readonly Dictionary<Type, short> _typeMap = new Dictionary<Type, short>();

        public byte Version => throw new InvalidOperationException();


        public HierarchicalSerializer(IWriter writer, ITypeSerializer typeSerializer, byte defaultVersion = 0)
            : base(writer)
        {
            _typeSerializer = typeSerializer;
            _writer.WriteByte(defaultVersion);
        }

        public void AddStruct<T>(ref T value)
            where T : struct, IDataStruct
        {
            value.Serialize(this);
        }

        public void AddVersionedStruct<T>(ref T value)
            where T : struct, IDataStruct, IVersionedData
        {
            _writer.WriteByte(value.Version);
            value.Serialize(this);
        }

        public void AddClass<T>(ref T value)
            where T : class, IDataStruct
        {
            if (value == null)
            {
                _writer.WriteShort(0);
            }
            else
            {
                var type = value.GetType();
                if (!_typeMap.TryGetValue(type, out short typeId))
                {
                    typeId = (short)_typeMap.Count;
                    checked
                    {
                        typeId += 1;
                    }

                    _typeMap.Add(type, typeId);
                    _writer.WriteShort(typeId);
                    _typeSerializer.Serialize(_writer, type);
                }
                else
                {
                    _writer.WriteShort(typeId);
                }

                _writer.BeginSection();
                SerializeClass(value);
                _writer.EndSection();
            }
        }

        protected virtual void SerializeClass(IDataStruct value)
        {
            if (value is IVersionedData versionedData)
            {
                _writer.WriteByte(versionedData.Version);
            }
            value.Serialize(this);
        }
    }
}