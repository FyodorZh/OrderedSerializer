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

        public bool IsWriter => false;

        public int Version { get; }

        public Deserializer(IReader reader, IFactory factory)
        {
            _reader = reader;
            _factory = factory;

            Version = reader.ReadInt();
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

        public void AddClass<T>(ref T value)
            where T : class, IDataClass
        {
            int flag = _reader.ReadInt();

            if (flag == 0) // NULL
            {
                value = null;
            }
            else if (flag > 0) // NEW INSTANCE
            {
                int instanceId = -_reader.ReadInt() - 1;

                _reader.BeginSection();
                value = _factory.Construct(flag - 1) as T;
                if (value != null)
                {
                    _instanceMap.Add(instanceId, value);
                    value.Serialize(this);
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