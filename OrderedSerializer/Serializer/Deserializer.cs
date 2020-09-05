using System;
using System.Collections.Generic;

namespace OrderedSerializer
{
    public interface IFactory
    {
        object Construct(int typeId);
    }

    public class Deserializer : IOrderedSerializer
    {
        private readonly IReader _reader;

        private readonly IFactory _factory;

        private readonly Dictionary<int, object> _instanceMap = new Dictionary<int, object>();

        private readonly Stack<byte> _versions = new Stack<byte>();
        private byte _version;

        public bool IsWriter => false;

        public byte Version => _version;

        public Deserializer(IReader dataReader, IReader typeDataReader, ITypeDeserializer typeDeserializer)
        {
            _reader = dataReader;

            TypeMap typeMap = new TypeMap();
            typeMap.Deserialize(typeDataReader, typeDeserializer);
            _factory = typeMap;

            _version = dataReader.ReadByte();
        }

        public void Add(ref bool value)
        {
            switch (_reader.ReadByte())
            {
                case 0:
                    value = false;
                    break;
                case 1:
                    value = true;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void Add(ref byte value)
        {
            value = _reader.ReadByte();
        }

        public void Add(ref char value)
        {
            value = _reader.ReadChar();
        }

        public void Add(ref int value)
        {
            value = _reader.ReadInt();
        }

        public void Add(ref long value)
        {
            value = _reader.ReadLong();
        }

        public void Add(ref string value)
        {
            value = _reader.ReadString();
        }

        public void AddStruct<T>(ref T value)
            where T : struct, IDataStruct
        {
            value.Serialize(this);
        }

        public void AddVersionedStruct<T>(ref T value)
            where T : struct, IDataStruct, IVersionedData
        {
            var version = _reader.ReadByte();
            _versions.Push(_version);
            _version = version;
            value.Serialize(this);
            _version = _versions.Pop();
        }

        public void AddClass<T>(ref T value)
            where T : class, IDataStruct
        {
            int flag = _reader.ReadInt();

            if (flag == 0) // NULL
            {
                value = null;
            }
            else if (flag > 0) // NEW INSTANCE
            {
                int typeId = flag - 1;
                int instanceId = -_reader.ReadInt() - 1;

                _reader.BeginSection();
                value = _factory.Construct(typeId) as T;
                if (value != null)
                {
                    _instanceMap.Add(instanceId, value);
                    if (value is IVersionedData versionedData)
                    {
                        var version = _reader.ReadByte();

                        _versions.Push(version);
                        _version = version;
                        value.Serialize(this);
                        _version = _versions.Pop();
                    }
                    else
                    {
                        value.Serialize(this);
                    }
                }
                else
                {
                    // We don't know this type
                }
                _reader.EndSection();
            }
            else // REFERENCE TO OTHER INSTANCE
            {
                int instanceId = -flag - 1;

                value = (T)_instanceMap[instanceId];
            }
        }
    }
}